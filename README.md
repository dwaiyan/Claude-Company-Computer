# 技术能力提升平台

智慧实验室 SaaS 团队内部技术学习与协同平台。

## 技术栈

| 层级 | 技术 |
|------|------|
| 后端 | .NET 6, ASP.NET Core Web API, EF Core 6 |
| 网关 | YARP Reverse Proxy |
| 前端 | Vue 3 + TypeScript + Vite + Pinia + Axios |
| 数据库 | PostgreSQL, MySQL, Redis |
| 消息队列 | RabbitMQ |
| 容器 | Docker Compose |
| 测试 | xUnit + Moq |
| 韧性 | Polly (重试 / 断路器 / 降级) |
| 日志 | Serilog |

## 架构

```
Vue 3 SPA (:5173)
    │
    ▼
YARP Gateway (:8080)
    │
    ├── Identity     (:5001, PostgreSQL) ─ 认证 · 用户 · 权限
    ├── Content      (:5002, PostgreSQL) ─ 技术图谱 · 资源 · 面试题
    ├── Learning     (:5003, MySQL)      ─ 打卡 · 技能雷达 · 评测
    ├── Collaboration(:5004, PostgreSQL) ─ 笔记 · 讨论 · 贡献审核
    ├── Notification (:5005, Redis)      ─ 站内通知 · RabbitMQ
    └── CodeRunner   (:5006, 无状态)     ─ C# 在线编译运行
```

## 快速启动

### 前置条件

- .NET 6 SDK
- Node.js 18+
- Docker Desktop

### 1. 启动基础设施

```bash
docker compose -f docker/docker-compose.dev.yml up -d
# PostgreSQL :5432, MySQL :3306, Redis :6379, RabbitMQ :5672/:15672
```

### 2. 启动后端服务（各开一个终端）

```bash
dotnet run --project src/Services/Identity/SkillPlatform.Identity.Api
dotnet run --project src/Services/Content/SkillPlatform.Content.Api
dotnet run --project src/Services/Learning/SkillPlatform.Learning.Api
dotnet run --project src/Services/Collaboration/SkillPlatform.Collaboration.Api
dotnet run --project src/Services/Notification/SkillPlatform.Notification.Api
dotnet run --project src/Services/CodeRunner/SkillPlatform.CodeRunner.Api
```

### 3. 启动网关

```bash
dotnet run --project src/Gateway/SkillPlatform.Gateway
```

### 4. 启动前端

```bash
cd client/skill-platform-web
npm install
npm run dev
```

浏览器打开 http://localhost:5173

### 5. 运行测试

```bash
dotnet test
```

## 项目结构

```
├── src/
│   ├── Gateway/SkillPlatform.Gateway/          # YARP 网关
│   ├── Services/
│   │   ├── Identity/    {Api,Core,Infra,Tests}  # 认证服务
│   │   ├── Content/     {Api,Core,Infra}        # 内容服务
│   │   ├── Learning/    {Api,Core,Infra}        # 学习服务
│   │   ├── Collaboration/{Api,Core,Infra}       # 协同服务
│   │   ├── Notification/{Api}                   # 通知服务
│   │   └── CodeRunner/  {Api}                   # 代码执行
│   ├── Shared/SkillPlatform.Common/             # 共享库
│   └── Client/skill-platform-web/               # Vue 3 前端
├── docker/                                      # Docker 编排
└── docs/                                        # 文档
    ├── architecture/     # ADR 架构决策记录
    └── guides/           # 团队指南
```

## 文档

- [架构决策记录](docs/architecture/) — 微服务架构/数据库策略/网关选型
- [微服务演进指南](docs/guides/microservice-evolution-guide.md) — 从单体到微服务的拆分方法论
- [技术学习路线](docs/guides/tech-stack-learning-path.md) — 团队技能提升路线图

## 服务 API 总览

| 服务 | 端口 | API 前缀 |
|------|------|----------|
| Identity | 5001 | /api/auth, /api/users |
| Content | 5002 | /api/tech-trees |
| Learning | 5003 | /api/learning, /api/admin |
| Collaboration | 5004 | /api/notes, /api/discussions, /api/contributions |
| Notification | 5005 | /api/notifications |
| CodeRunner | 5006 | /api/code |
