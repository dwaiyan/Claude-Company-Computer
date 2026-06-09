# ADR-0002: 数据库策略 — 多数据库异构

**日期:** 2026-06-09  
**状态:** 已采纳

## 背景

平台有 4 个有状态服务需要持久化存储。团队主要使用 PostgreSQL，但在实际项目中可能遇到 MySQL。

## 决策

- **Identity / Content / Collaboration** 使用 **PostgreSQL**（共用一个实例，不同表前缀）
- **Learning** 使用 **MySQL**（Pomelo.EntityFrameworkCore.MySql 提供程序）
- **Notification** 使用 **Redis**（List 数据结构存储通知队列）

### 为什么 Learning 用 MySQL？

1. **教学目的** — 展示 EF Core 的多数据库提供程序能力
2. **真实场景模拟** — 很多遗留 MES/ERP 系统运行在 MySQL 上
3. **EF Core 抽象** — 业务代码不感知底层数据库差异，切换只需改连接字符串和提供程序

## 后果

- 开发环境需同时运行 PostgreSQL 和 MySQL（Docker Compose 已包含）
- EF Core 的 `UseNpgsql()` vs `UseMySql()` 配置略有差异
- 迁移命令需分别对两个数据库执行

## 代码示例

```csharp
// PostgreSQL (Content 服务)
services.AddDbContext<ContentDbContext>(options =>
    options.UseNpgsql(connectionString));

// MySQL (Learning 服务)
services.AddDbContext<LearningDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
```
