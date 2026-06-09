# 团队开发手册

## 新人 Onboarding（预计 2 天）

### Day 1：环境搭建 → 跑通全流程

1. 安装 .NET 6 SDK, Node.js 18+, Docker Desktop
2. Clone 项目，启动 Docker Compose（PG + MySQL + Redis + RabbitMQ）
3. 按 README 启动 6 个后端服务 + 网关 + 前端
4. 注册账号 → 浏览技术图谱 → 打一次卡 → 写一篇笔记

### Day 2：理解架构 → 阅读文档

1. 阅读 [架构决策记录](../architecture/) — 理解为什么这样设计
2. 阅读 [微服务演进指南](microservice-evolution-guide.md) — 理解拆分方法论
3. 看代码：从 Controller → Service → Repository → DbContext 的调用链
4. 运行并读懂 5 个单元测试

## 开发规范

### 项目命名

```
SkillPlatform.{Service}.{Layer}

Layer:
  Api      — Web API 层（Controller + Program.cs）
  Core     — 领域层（Entities / Interfaces / DTOs）
  Infra    — 基础设施层（DbContext / Repository）
  Tests    — 测试
```

### 代码规范

- **一个文件一个职责** — Controller 不写业务逻辑，Repository 不做 DTO 映射
- **接口驱动** — Core 层定义接口，Infra 层实现
- **DTO 分离** — 前端接收/返回的数据不直接暴露 Entity
- **EF Core 迁移** — 每个有状态服务独立管理迁移

### 添加新 API 的步骤

1. Core: 定义 Entity + Interface + DTO
2. Infra: 实现 Repository
3. Api: 添加 Controller + 注册 DI
4. 创建迁移: `dotnet dotnet-ef migrations add ...`
5. Gateway: 添加路由配置
6. 前端: 添加 API 模块 + 页面

### Git Commit 规范

```
feat:  新功能
fix:   修复
docs:  文档
test:  测试
refactor: 重构
scaffold: 脚手架
```

例子:
```
feat: add Content service (tech trees, nodes, resources)
docs: add ADR for microservices architecture
```

## 常见问题

### Q: 为什么有 PostgreSQL 又有 MySQL？
A: 教学目的。展示 EF Core 的多数据库提供程序能力。实际项目中统一用一种即可。

### Q: 为什么 CodeRunner 没有数据库？
A: CodeRunner 是无状态服务 — 接收代码、编译、运行、返回结果、清理临时文件。不需要持久化。

### Q: 服务间如何通信？
A: 查询走 HTTP REST（同步），通知走 RabbitMQ（异步）。Polly 提供重试/断路保护。

### Q: 如何添加一个新微服务？
A: 参见上方"添加新 API 的步骤"，然后：
1. 在 docker-compose 中确认依赖的基础设施已就绪
2. 在 Gateway 中添加路由配置
3. 更新本手册

## 环境变量参考

| 变量 | 默认值 | 说明 |
|------|--------|------|
| ConnectionStrings__DefaultConnection (PG) | Host=localhost:5432;Database=identity;Username=skilldev;Password=SkillDev@2026 | PostgreSQL 连接 |
| ConnectionStrings__DefaultConnection (MySQL) | Server=localhost:3306;Database=learning;User=skilldev;Password=SkillDev@2026 | MySQL 连接 |
| ConnectionStrings__Redis | localhost:6379 | Redis 连接 |
| Jwt:SecretKey | SkillPlatform@2026!...MinLength32 | JWT 签名密钥 (生产环境需更换) |
| Jwt:ExpiresInMinutes | 60 | Token 有效期 |
