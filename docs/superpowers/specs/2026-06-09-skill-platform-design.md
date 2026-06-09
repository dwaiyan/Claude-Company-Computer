# 技术能力提升平台 — 设计文档

> 2026-06-09 · 智慧实验室 SaaS 团队内部平台

## 1. 概述

### 1.1 背景

团队技术栈以 .NET 6 微服务为核心，覆盖 IoT、数字孪生、APS 排产、CRM 等业务领域。需要一个内部平台帮助团队成员系统化提升技术能力，同时该平台本身也是微服务架构的实践示范。

### 1.2 目标

构建一个**技术学习 + 实战评测 + 协同共建**的内部平台，具备以下特征：

- 技术图谱导航 — 按层级组织学习路径
- 学习追踪 — 技能雷达、打卡、评测
- 团队协同 — 笔记、讨论、贡献审核
- 微服务架构 — 5 个独立服务，与团队技术方向一致

### 1.3 目标用户

公司内部团队成员，管理员编排内容，成员学习 + 贡献。

---

## 2. 技术栈

| 层级 | 技术 |
|------|------|
| 后端框架 | .NET 6, ASP.NET Core Web API |
| 数据库 | PostgreSQL (大部分服务), MySQL (Learning 服务) |
| 缓存 | Redis |
| 消息队列 | RabbitMQ |
| 网关 | YARP |
| ORM | Entity Framework Core |
| 前端 | Vue 3 + Vite + Pinia + Axios |
| 容器 | Docker Compose (开发 & 部署) |
| 日志 | Serilog → Elasticsearch |
| 韧性 | Polly (重试/断路器/超时) |
| 测试 | xUnit + Moq + WebApplicationFactory |

---

## 3. 架构设计

### 3.1 服务拓扑

```
Vue 3 SPA (Nginx)
       │
       ▼
API Gateway (YARP)  ← 路由 · 认证 · 限流
  ┌────┼────────┬──────────┬──────────┐
  ▼    ▼        ▼          ▼          ▼
Identity  Content  Learning  Collaboration  Notification
(PG)      (PG)     (MySQL)   (PG)          (Redis)
  │    │    │      │    │      │    │    │
  └────┼────┼──────┼────┼──────┼────┼────┘
       │         │         │
       ▼         ▼         ▼
  ┌─────────────────────────────┐
  │        RabbitMQ             │
  │   事件总线 (异步解耦)        │
  └─────────────────────────────┘
                │
                ▼
             Redis
           统一缓存
```

### 3.2 服务职责

| 服务 | 数据库 | 职责 |
|------|--------|------|
| Identity | PostgreSQL | 用户、角色、权限、JWT 认证与刷新 |
| Content | PostgreSQL | 技术树/节点、学习资源、面试题库 |
| Learning | MySQL | 学习路线、技能雷达、打卡、评测与记录 |
| Collaboration | PostgreSQL | 笔记、讨论、贡献审核 |
| Notification | Redis | 站内通知，RabbitMQ 消费者，MQTT 预留 |

### 3.3 服务间通信

- **同步**：HTTP REST (内网) — 查询用户信息、内容校验
- **异步**：RabbitMQ — 事件通知（新笔记 → 通知订阅者、审核结果 → 通知贡献者）
- **重试/降级**：Polly — 3 次重试 + 30s 熔断 + 返回缓存/默认值

### 3.4 共享包

`SkillPlatform.Common` NuGet 包包含：

- 统一错误码与响应模型
- ProblemDetails 中间件
- JWT 验证扩展
- Serilog 结构化日志帮助类
- TraceId 传播

---

## 4. 功能模块

### 4.1 技术图谱

```
.NET 生态技术树
├── 后端框架 (.NET 6 / ASP.NET Core / 微服务 / WPF)
├── 数据与存储 (EF Core / MySQL / PostgreSQL / Redis)
├── 消息与通信 (RabbitMQ / MQTT)
├── 前端 (Vue 3 / Flutter)
└── 业务领域 (SaaS / IoT / 数字孪生 / APS / MES/ERP / AGV / CRM)
```

每个节点包含：学习资源 → 面试题 → 实战任务 → 能力自评 → 团队笔记

### 4.2 学习追踪

- 技能雷达图（掌握/应用/实践/教学/创新五维）
- 学习路线完成度
- 连续打卡天数
- 每周学习时长统计

### 4.3 实战评测

- 选择题（知识覆盖）
- 代码题（.NET 在线编译）
- 场景设计题（提交架构方案）
- 项目案例（Step-by-step 微服务实践）

### 4.4 协同共建

成员提交 → 管理员审核 → 发布到技术图谱节点；专家可直发

### 4.5 通知系统

学习提醒、审核结果、新内容发布、打卡催促；WebSocket 实时推送 + 预留邮件

---

## 5. 数据模型

### 5.1 Identity (PostgreSQL)

```
Users (id, username, email, password_hash, avatar, title, department, timestamps)
Roles (id, name, description)
Permissions (id, code, description)
UserRoles / RolePermissions (关联表)
```

### 5.2 Content (PostgreSQL)

```
TechTrees (id, title, description, icon, sort_order, parent_id)
TechNodes (id, tree_id, title, description, level, sort_order, parent_id)
Resources (id, node_id, title, url, type, difficulty, created_by)
InterviewQuestions (id, node_id, question, answer_tip, difficulty, category)
```

### 5.3 Learning (MySQL)

```
LearningPaths (id, user_id, node_ids JSON, progress JSON, started_at, target_days)
SkillRadar (id, user_id, node_id, level, self_score, evaluated_at)
CheckIns (id, user_id, date, duration_minutes, note)
Assessments (id, title, type, node_id, questions JSON, time_limit, pass_score)
AssessmentRecords (id, user_id, assessment_id, answers JSON, score, passed, timestamps)
```

### 5.4 Collaboration (PostgreSQL)

```
Notes (id, user_id, node_id, title, content MD, status, view_count, like_count, timestamps)
Discussions (id, node_id, user_id, content, parent_id)
Contributions (id, user_id, type, target, status, reviewed_by, review_comment, created_at)
```

### 5.5 Notification (Redis + RabbitMQ)

```
消息结构 JSON: { userId, type, title, body, url, isRead, createdAt }
Redis List 存储用户通知队列
RabbitMQ 事件驱动消费者写入
```

---

## 6. API 设计

### 6.1 Identity

```
POST   /api/auth/login
POST   /api/auth/refresh
POST   /api/users/register
GET    /api/users
PUT    /api/users/{id}/roles
```

### 6.2 Content

```
GET    /api/tech-trees
GET    /api/tech-nodes?tree={id}
GET    /api/nodes/{id}/resources
POST   /api/resources
GET    /api/interview-questions?node={id}&difficulty={n}
```

### 6.3 Learning

```
GET    /api/learning/profile/{userId}
POST   /api/learning/check-in
GET    /api/learning/streak/{userId}
GET    /api/learning/skill-radar/{userId}
PUT    /api/learning/skill-radar/{userId}
GET    /api/assessments?node={id}
POST   /api/assessments/{id}/submit
GET    /api/assessments/records
```

### 6.4 Collaboration

```
GET    /api/notes?node={id}
POST   /api/notes
GET    /api/notes/{id}
POST   /api/discussions
GET    /api/discussions?node={id}
POST   /api/contributions
GET    /api/contributions/pending
PUT    /api/contributions/{id}/review
```

### 6.5 Notification

```
GET    /api/notifications?userId={id}
PUT    /api/notifications/{id}/read
POST   /api/notifications/topic/subscribe
```

### 6.6 统一错误响应

```json
{
  "code": "CONTENT_NODE_NOT_FOUND",
  "message": "技术节点不存在",
  "detail": "节点ID 123 已被删除或不存在",
  "traceId": "abc-123"
}
```

---

## 7. 前端路由

```
/                         首页仪表盘
/tech-tree               技术图谱总览
/tech-tree/:nodeId       节点详情
/learning/:userId        个人学习面板
/check-in                打卡页
/assessment/:id          评测页面
/notes                   团队笔记广场
/notes/:id               笔记详情
/admin                   管理后台
/profile                 个人设置
```

---

## 8. 韧性 & 可观测性

### 8.1 服务间容错

- Polly 重试策略：3 次指数退避
- 断路器：30 秒熔断窗口
- 降级：返回缓存数据或默认空列表

### 8.2 日志与监控

- Serilog → Elasticsearch 结构化日志
- ASP.NET Core Health Checks → `/health` 终端
- Prometheus 指标预留

---

## 9. 测试策略

| 层级 | 工具 | 覆盖目标 |
|------|------|----------|
| 单元测试 | xUnit + Moq | 业务逻辑 ≥80% |
| 集成测试 | WebApplicationFactory | 数据库/消息队列交互 |
| 契约测试 | 自定义 + xUnit | API Schema 不兼容变更 |
| E2E | Playwright (可选) | 核心用户流程 |

每个微服务自带 `*.Tests` 项目。

---

## 10. 项目结构

```
/dev-server
├── src/
│   ├── Gateway/SkillPlatform.Gateway/
│   ├── Services/{Identity,Content,Learning,Collaboration,Notification}/
│   │   └── SkillPlatform.{Service}.{Api,Core,Infra,Tests}/
│   ├── Shared/SkillPlatform.Common/
│   └── Client/skill-platform-web/    (Vue 3)
├── docker/
│   ├── docker-compose.yml
│   ├── docker-compose.dev.yml
│   └── nginx.conf
├── docs/
│   ├── architecture/
│   └── api/
└── README.md
```

---

## 11. 分阶段路线

| 阶段 | 内容 | 目标 |
|------|------|------|
| P1 · 骨架 | 项目脚手架 + Identity + Gateway + Vue 框架 | 登录注册跑通 |
| P2 · 核心 | Content + Learning 服务 | 技术图谱 + 个人面板 |
| P3 · 协同 | Collaboration + Notification | 笔记/讨论 + 通知 |
| P4 · 深度 | 评测系统 + 技能雷达分析 | 学习深度 |
| P5 · 演进 | 总结微服务拆分经验 → 团队文档 | 知识沉淀 |
