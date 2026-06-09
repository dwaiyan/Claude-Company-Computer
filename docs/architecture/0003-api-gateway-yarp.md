# ADR-0003: YARP 作为 API 网关

**日期:** 2026-06-09  
**状态:** 已采纳

## 背景

微服务架构需要统一的入口：路由转发、CORS、日志、限流。

## 决策

采用 **YARP (Yet Another Reverse Proxy)**，Microsoft 官方出品的 .NET 反向代理库。

### 当前路由表

| 路径 | 目标服务 | 端口 |
|------|----------|------|
| /api/auth/*, /api/users/* | Identity | 5001 |
| /api/tech-trees/* | Content | 5002 |
| /api/learning/*, /api/admin/* | Learning | 5003 |
| /api/notes/*, /api/discussions/*, /api/contributions/* | Collaboration | 5004 |
| /api/notifications/* | Notification | 5005 |
| /api/code/* | CodeRunner | 5006 |

### 为什么选 YARP？

- 纯 .NET 实现，零额外基础设施
- 配置式路由（JSON），也可编程式
- 内置健康检查、负载均衡
- 与 ASP.NET Core 中间件管道无缝集成（CORS、Auth、日志）

### 不做

- **不选 Nginx/Kong/Traefik** — 增加运维复杂度，对内部工具过度设计
- **不在网关做认证** — 各服务独立验证 JWT，网关仅转发
