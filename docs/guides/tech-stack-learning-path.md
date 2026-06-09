# 团队技术能力提升路线图

> 基于技术能力提升平台的技术栈，建议按以下路线系统学习

## 第一阶段：基础夯实 (1-2 个月)

### .NET 6 核心
- [ ] CLR 基础：GC、JIT、类型系统
- [ ] 异步编程：Task、async/await、ConfigureAwait
- [ ] LINQ 高级用法：表达式树、IQueryable
- [ ] 依赖注入：生命周期、作用域、Autofac

### ASP.NET Core
- [ ] 中间件管道：RequestDelegate、Use/Map/Run
- [ ] Minimal API vs Controller
- [ ] 模型绑定、验证、过滤器
- [ ] EF Core：Code First、Migration、性能优化

**对应平台节点：** 后端框架 → .NET 6 基础 / ASP.NET Core

## 第二阶段：微服务实战 (2-3 个月)

### 服务治理
- [ ] API 网关：YARP 配置、路由、限流
- [ ] 服务间通信：HttpClientFactory、gRPC
- [ ] 韧性：Polly 重试/断路器/超时
- [ ] 健康检查：ASP.NET Core Health Checks

### 数据管理
- [ ] PostgreSQL：索引优化、JSONB、全文搜索
- [ ] MySQL：字符集、存储引擎、主从复制
- [ ] Redis：缓存策略、分布式锁、Pub/Sub
- [ ] EF Core 多数据库提供程序

### 消息队列
- [ ] RabbitMQ：Exchange/Queue/Binding
- [ ] 可靠投递：确认机制、死信队列
- [ ] MQTT：QoS、Topic、物联网场景

**对应平台节点：** 微服务 / 数据与存储 / 消息与通信

## 第三阶段：业务领域 (2-3 个月)

### SaaS 多租户
- [ ] 租户隔离策略：Database per Tenant / Schema per Tenant
- [ ] 租户识别：域名/Header/JWT Claim
- [ ] 计费与订阅：用量统计、套餐管理

### IoT 物联网
- [ ] 设备管理：注册、认证、状态监控
- [ ] 数据采集：MQTT 上报、时序存储
- [ ] 规则引擎：设备事件触发动作

### 数字孪生
- [ ] 3D 建模：设备几何模型
- [ ] 实时映射：物理→数字状态同步
- [ ] 仿真分析：历史数据回放

### APS 排产
- [ ] 约束求解：工序/资源/时间
- [ ] 甘特图可视化
- [ ] 多目标优化

**对应平台节点：** 业务领域 → SaaS / IoT / 数字孪生 / APS

## 第四阶段：前端 + 客户端 (1-2 个月)

- [ ] Vue 3 Composition API、Pinia、Vite
- [ ] WPF MVVM 模式、数据绑定
- [ ] Flutter Widget、状态管理

**建议：** 每个阶段学完后，在对应平台节点下做能力自评（1-5 分）
