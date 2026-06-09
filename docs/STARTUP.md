# 技术能力提升平台 — 启动指南

> 从零启动全平台的一站式操作手册

## 环境要求

| 工具 | 最低版本 | 验证命令 |
|------|----------|----------|
| .NET SDK | 6.0 | `dotnet --version` |
| Node.js | 18 | `node --version` |
| Docker Desktop | 20.10+ | `docker --version` |
| Git | 任意 | `git --version` |

## 一、克隆项目

```bash
git clone <repo-url> skill-platform
cd skill-platform
```

## 二、启动基础设施

```bash
docker compose -f docker/docker-compose.dev.yml up -d
```

等待所有容器就绪（约 30 秒），验证：

```bash
docker compose -f docker/docker-compose.dev.yml ps
```

预期输出：4 个容器均为 `Up` 状态。

| 容器 | 端口 | 用途 |
|------|------|------|
| postgres:15-alpine | 5432 | Identity / Content / Collaboration |
| mysql:8.0 | 3306 | Learning |
| redis:7-alpine | 6379 | Notification |
| rabbitmq:3-management | 5672, 15672 | 消息队列 / 管理面板 |

## 三、启动后端服务

> 每个服务需要独立终端窗口。按依赖顺序启动。

### 终端 1：Identity（认证服务）

```bash
dotnet run --project src/Services/Identity/SkillPlatform.Identity.Api
```

等待输出：`Now listening on: http://localhost:5001`

首次启动会自动执行 EF 迁移和种子数据（admin/member 角色）。

### 终端 2：Content（内容服务）

```bash
dotnet run --project src/Services/Content/SkillPlatform.Content.Api
```

等待输出：`Now listening on: http://localhost:5002`

### 终端 3：Learning（学习服务）

```bash
dotnet run --project src/Services/Learning/SkillPlatform.Learning.Api
```

等待输出：`Now listening on: http://localhost:5003`

### 终端 4：Collaboration（协同服务）

```bash
dotnet run --project src/Services/Collaboration/SkillPlatform.Collaboration.Api
```

等待输出：`Now listening on: http://localhost:5004`

### 终端 5：Notification（通知服务）

```bash
dotnet run --project src/Services/Notification/SkillPlatform.Notification.Api
```

等待输出：`Now listening on: http://localhost:5005`

### 终端 6：CodeRunner（代码执行服务）

```bash
dotnet run --project src/Services/CodeRunner/SkillPlatform.CodeRunner.Api
```

等待输出：`Now listening on: http://localhost:5006`

## 四、启动网关

```bash
dotnet run --project src/Gateway/SkillPlatform.Gateway
```

等待输出：`Now listening on: http://localhost:8080`

## 五、启动前端

```bash
cd client/skill-platform-web
npm install        # 首次需安装依赖
npm run dev
```

等待输出：`Local: http://localhost:5173`

## 六、验证全流程

### 6.1 健康检查

```bash
# 各服务健康检查
curl http://localhost:5001/swagger
curl http://localhost:5002/swagger
curl http://localhost:5003/swagger
curl http://localhost:5004/swagger
curl http://localhost:5005/swagger
curl http://localhost:5006/swagger
```

### 6.2 网关路由检查

```bash
# 通过网关访问各服务
curl http://localhost:8080/api/auth/login
curl http://localhost:8080/api/tech-trees
```

### 6.3 注册用户 + 登录

```bash
# 注册
curl -s -X POST http://localhost:8080/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "管理员",
    "email": "admin@company.com",
    "password": "Admin@123",
    "department": "技术部",
    "title": "架构师"
  }' | python -m json.tool

# 登录
curl -s -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@company.com",
    "password": "Admin@123"
  }' | python -m json.tool
```

将返回的 `accessToken` 保存为环境变量：

```bash
export TOKEN="<复制 accessToken>"
```

### 6.4 测试受保护接口

```bash
curl -s http://localhost:8080/api/users/me \
  -H "Authorization: Bearer $TOKEN" | python -m json.tool
```

### 6.5 运行单元测试

```bash
dotnet test
```

预期：5 通过，0 失败。

### 6.6 浏览器验证

打开 http://localhost:5173，手动验证：

1. 访问页面 → 自动跳转登录页
2. 点击"立即注册" → 注册新账号
3. 登录 → 进入仪表盘
4. 进入"技术图谱" → 浏览节点
5. 进入"我的学习" → 打卡
6. 进入"在线代码" → 写一段 C# 并运行
7. 如用 admin 账号登录 → 可见"管理"入口

## 七、常见问题

### Q1: Docker 报错 "port already in use"

```bash
# 检查端口占用
netstat -ano | findstr "5432"
netstat -ano | findstr "3306"
netstat -ano | findstr "6379"

# 停止冲突的容器
docker compose -f docker/docker-compose.dev.yml down
```

### Q2: EF 迁移报错 "database does not exist"

```bash
# 手动创建数据库
docker exec -it skill-platform-postgres-1 psql -U skilldev -c "CREATE DATABASE identity;"
```

### Q3: 前端报错 "Network Error"

确认 Gateway 在 8080 端口运行，且 Identity 服务在 5001 端口运行。

### Q4: CodeRunner 执行超时

CodeRunner 使用 `dotnet build` + `dotnet run`，首次编译会较慢。如果持续超时，检查 .NET SDK 版本：

```bash
dotnet --version  # 应为 6.0.x
```

### Q5: 如何重置所有数据？

```bash
# 停止容器并删除数据卷
docker compose -f docker/docker-compose.dev.yml down -v

# 重新启动
docker compose -f docker/docker-compose.dev.yml up -d

# 删除迁移记录后重建
rm -rf src/Services/Identity/SkillPlatform.Identity.Infra/Migrations
rm -rf src/Services/Content/SkillPlatform.Content.Infra/Migrations
rm -rf src/Services/Learning/SkillPlatform.Learning.Infra/Migrations
rm -rf src/Services/Collaboration/SkillPlatform.Collaboration.Infra/Migrations

# 重新创建迁移
dotnet dotnet-ef migrations add InitialCreate --project src/Services/Identity/SkillPlatform.Identity.Infra --startup-project src/Services/Identity/SkillPlatform.Identity.Api
# ...其他服务同理
```

## 八、关闭平台

```bash
# 停止所有服务 (Ctrl+C 每个终端)

# 停止 Docker 容器
docker compose -f docker/docker-compose.dev.yml down

# 如需保留数据卷（下次启动数据仍在）
docker compose -f docker/docker-compose.dev.yml down

# 如需完全清除
docker compose -f docker/docker-compose.dev.yml down -v
```

## 九、一键启动脚本（可选）

将以下内容保存为 `start-all.sh`（Git Bash 环境）：

```bash
#!/bin/bash
set -e

echo "🚀 启动技术能力提升平台..."
echo ""

echo "1/4 启动 Docker 基础设施..."
docker compose -f docker/docker-compose.dev.yml up -d
echo "⏳ 等待 PostgreSQL 就绪..."
sleep 5

echo "2/4 启动后端服务..."
dotnet run --project src/Services/Identity/SkillPlatform.Identity.Api &
dotnet run --project src/Services/Content/SkillPlatform.Content.Api &
dotnet run --project src/Services/Learning/SkillPlatform.Learning.Api &
dotnet run --project src/Services/Collaboration/SkillPlatform.Collaboration.Api &
dotnet run --project src/Services/Notification/SkillPlatform.Notification.Api &
dotnet run --project src/Services/CodeRunner/SkillPlatform.CodeRunner.Api &
echo "⏳ 等待服务就绪..."
sleep 10

echo "3/4 启动网关..."
dotnet run --project src/Gateway/SkillPlatform.Gateway &
sleep 3

echo "4/4 启动前端..."
cd client/skill-platform-web
npm run dev &
sleep 5

echo ""
echo "✅ 平台启动完成！"
echo "前端: http://localhost:5173"
echo "网关: http://localhost:8080"
echo "Swagger 文档: http://localhost:5001/swagger (Identity)"
echo ""
echo "按 Ctrl+C 停止所有服务"
wait
```

保存后执行：

```bash
chmod +x start-all.sh
./start-all.sh
```

## 服务端口总览

```
┌──────────────────────────────────────────────┐
│  前端 (Vue 3 Dev)  →  http://localhost:5173  │
│  网关 (YARP)       →  http://localhost:8080  │
├──────────────────────────────────────────────┤
│  Identity          →  http://localhost:5001  │
│  Content           →  http://localhost:5002  │
│  Learning          →  http://localhost:5003  │
│  Collaboration     →  http://localhost:5004  │
│  Notification      →  http://localhost:5005  │
│  CodeRunner        →  http://localhost:5006  │
├──────────────────────────────────────────────┤
│  PostgreSQL        →  localhost:5432         │
│  MySQL             →  localhost:3306         │
│  Redis             →  localhost:6379         │
│  RabbitMQ          →  localhost:5672         │
│  RabbitMQ 管理面板  →  http://localhost:15672 │
└──────────────────────────────────────────────┘
```
