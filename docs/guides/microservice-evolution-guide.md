# 从模块化单体到微服务 — 演进指南

> 基于技术能力提升平台实战经验总结

## 核心理念

不要一开始就上微服务。从模块化单体开始，在正确的时间节点拆分。

## 拆分时机三问

在决定拆分一个新服务前，问自己：

1. **独立部署需求** — 这个模块需要独立的发布节奏吗？
2. **独立扩展需求** — 这个模块的负载特征与其他模块不同吗？
3. **团队边界** — 是否有独立团队负责这个模块？

**我们的实践：** 平台从第 1 天就按微服务构建（作为教学示范），但回顾来看，P1 只做 Identity + 网关、P2 加 Content + Learning、P3 补 Collaboration + Notification、P4 加 CodeRunner 的渐进式演进也是合理的。

## 四步拆分法

### 第 1 步：模块化（Module）

在代码层面隔离业务边界，建立清晰的接口。

```
src/
└── Modules/
    ├── Identity/   # IUserService, IAuthService
    ├── Content/    # ITechTreeService
    └── Learning/   # ILearningService
```

### 第 2 步：独立数据库（Database per Module）

每个模块拥有自己的数据库 schema 或独立数据库。

```
PostgreSQL:
├── identity.users, identity.roles, ...
├── content.tech_trees, content.tech_nodes, ...
└── collaboration.notes, collaboration.discussions, ...
```

### 第 3 步：独立部署（Service）

提取为独立的 ASP.NET Core 项目，通过 HTTP/gRPC 通信。

```csharp
// 同步通信
var user = await _httpClient.GetFromJsonAsync<UserDto>(
    $"http://identity-service/api/users/{userId}");

// 异步通信
await _messageBus.PublishAsync(new NoteCreatedEvent(noteId, userId));
```

### 第 4 步：网关 + 服务发现（Gateway）

引入 API 网关统一入口，各服务注册到网关。

```json
{
  "ReverseProxy": {
    "Routes": {
      "identity-route": {
        "ClusterId": "identity-cluster",
        "Match": { "Path": "/api/auth/{**catch-all}" }
      }
    },
    "Clusters": {
      "identity-cluster": {
        "Destinations": {
          "identity": { "Address": "http://identity-service/" }
        }
      }
    }
  }
}
```

## 数据一致性策略

### 同步：双写 + 补偿

```csharp
// 注册用户后同步创建学习路径
await _userRepo.AddAsync(user);
try {
    await _httpClient.PostAsJsonAsync("/api/learning/paths", new { userId = user.Id });
} catch {
    // 补偿：记录失败，后续重试
    await _outboxRepo.AddAsync(new OutboxMessage("CreateLearningPath", user));
}
```

### 异步：事件驱动

```
UserRegistered → [RabbitMQ] → CreateDefaultLearningPath
NotePublished   → [RabbitMQ] → NotifySubscribers
```

## 韧性模式

| 问题 | 方案 | 工具 |
|------|------|------|
| 下游服务不可用 | 重试 3 次 + 断路器熔断 30s | Polly |
| 请求超时 | HttpClient.Timeout = 5s | .NET 内置 |
| 突发流量 | 网关限流 | YARP RateLimiting |
| 级联失败 | 降级返回缓存/默认值 | Polly Fallback |

```csharp
services.AddHttpClient("identity")
    .AddTransientHttpErrorPolicy(p =>
        p.WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))))
    .AddTransientHttpErrorPolicy(p =>
        p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

## 可观测性

每个服务暴露：
- `/health` — 健康检查终端
- `Serilog` → Elasticsearch/Kibana — 结构化日志
- `Prometheus` metrics — 请求量/延迟/错误率

## 本平台的演进路径

| 阶段 | 服务数 | 关键里程碑 |
|------|--------|------------|
| P1 | 1 + 网关 | 跑通认证 → 确认架构可行性 |
| P2 | 3 + 网关 | 图谱+学习 → 验证异构数据库 |
| P3 | 5 + 网关 | 协同+通知 → 验证事件驱动 |
| P4 | 6 + 网关 | 在线代码 → 验证无状态服务 |

每次增加服务时验证：路由正确、CORS 正确、JWT 正确、健康检查可用。
