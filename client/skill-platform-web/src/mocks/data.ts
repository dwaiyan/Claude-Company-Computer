import type { UserInfo, AuthResponse } from '@/api/auth';
import type { TechTreeDto, TechNodeDto, ResourceDto, InterviewQuestionDto } from '@/api/content';
import type { LearningProfile, CheckInDto, SkillRadarItem, AssessmentDto } from '@/api/learning';
import type { NoteDto, DiscussionDto } from '@/api/collaboration';
import type { CodeRunResult, LanguageInfo } from '@/api/coderunner';

// ── Auth ──
export const mockUser: UserInfo = {
  id: 'u-001',
  username: 'zhangdw',
  email: 'zhangdw@company.com',
  avatar: null,
  title: '高级工程师',
  department: '智慧实验室',
  roles: ['admin', 'user'],
};

export const mockAuthResponse: AuthResponse = {
  accessToken: 'mock-access-token-xxxxx',
  refreshToken: 'mock-refresh-token-xxxxx',
  expiresIn: 86400,
  user: mockUser,
};

// ── Content: Tech Trees ──
export const mockTechTrees: TechTreeDto[] = [
  {
    id: 'tree-1',
    title: '.NET 生态技术树',
    description: '覆盖后端框架、数据存储、消息通信、前端及业务领域的完整技术图谱',
    icon: '⬡',
    children: [],
  },
];

// ── Content: Tech Nodes ──
export const mockNodes: TechNodeDto[] = [
  {
    id: 'node-1', title: '.NET 6 / ASP.NET Core', description: '跨平台高性能后端框架，构建 RESTful API 与微服务', level: 1, parentId: null,
    children: [], resourceCount: 8, questionCount: 12,
  },
  {
    id: 'node-2', title: 'Entity Framework Core', description: '轻量级 ORM，支持 LINQ 查询、迁移、变更追踪', level: 1, parentId: null,
    children: [], resourceCount: 5, questionCount: 8,
  },
  {
    id: 'node-3', title: '微服务架构', description: '服务拆分、服务发现、分布式事务、Saga 模式', level: 1, parentId: null,
    children: [], resourceCount: 6, questionCount: 10,
  },
  {
    id: 'node-4', title: 'PostgreSQL', description: '高级开源关系型数据库，JSONB、全文搜索、窗口函数', level: 1, parentId: null,
    children: [], resourceCount: 4, questionCount: 6,
  },
  {
    id: 'node-5', title: 'MySQL', description: '广泛应用的关系型数据库，InnoDB 引擎、复制与分片', level: 1, parentId: null,
    children: [], resourceCount: 3, questionCount: 5,
  },
  {
    id: 'node-6', title: 'Redis', description: '高性能内存数据库，缓存、队列、发布订阅、分布式锁', level: 1, parentId: null,
    children: [], resourceCount: 5, questionCount: 7,
  },
  {
    id: 'node-7', title: 'RabbitMQ', description: '消息队列中间件，AMQP 协议、交换机、死信队列', level: 1, parentId: null,
    children: [], resourceCount: 4, questionCount: 6,
  },
  {
    id: 'node-8', title: 'MQTT', description: '轻量级物联网通信协议，发布/订阅模型，QoS 等级', level: 1, parentId: null,
    children: [], resourceCount: 3, questionCount: 4,
  },
  {
    id: 'node-9', title: 'Vue 3', description: '渐进式前端框架，Composition API + 响应式系统 + TypeScript', level: 1, parentId: null,
    children: [], resourceCount: 6, questionCount: 9,
  },
  {
    id: 'node-10', title: 'IoT 平台', description: '设备接入、数据采集、规则引擎、数字孪生', level: 1, parentId: null,
    children: [], resourceCount: 5, questionCount: 8,
  },
  {
    id: 'node-11', title: 'APS 排产', description: '高级计划与排程系统，约束传播、遗传算法、甘特图', level: 1, parentId: null,
    children: [], resourceCount: 4, questionCount: 5,
  },
  {
    id: 'node-12', title: 'CRM 系统', description: '客户关系管理，销售漏斗、数据分析、自动化营销', level: 1, parentId: null,
    children: [], resourceCount: 3, questionCount: 4,
  },
];

// ── Content: Resources (per node) ──
const mockResourceLibrary: Record<string, ResourceDto[]> = {
  'node-1': [
    { id: 'r-1', title: 'ASP.NET Core 官方文档', url: 'https://learn.microsoft.com/aspnet/core', type: 'doc', difficulty: 1 },
    { id: 'r-2', title: 'Building Microservices with .NET', url: 'https://example.com/microservices-net', type: 'book', difficulty: 3 },
    { id: 'r-3', title: 'Middleware 管道深入解析', url: 'https://example.com/middleware', type: 'article', difficulty: 2 },
    { id: 'r-4', title: '.NET 6 新特性实战', url: 'https://example.com/dotnet6', type: 'video', difficulty: 2 },
    { id: 'r-4b', title: '依赖注入最佳实践', url: 'https://example.com/di-patterns', type: 'article', difficulty: 2 },
    { id: 'r-4c', title: 'ASP.NET Core 性能调优指南', url: 'https://example.com/perf-tuning', type: 'doc', difficulty: 3 },
    { id: 'r-4d', title: 'Minimal API vs Controller — 选型对比', url: 'https://example.com/api-compare', type: 'video', difficulty: 1 },
    { id: 'r-4e', title: 'C# 10 新特性在 Web 开发中的应用', url: 'https://example.com/csharp10-web', type: 'article', difficulty: 2 },
  ],
  'node-2': [
    { id: 'r-5', title: 'EF Core 官方文档', url: 'https://learn.microsoft.com/ef/core', type: 'doc', difficulty: 1 },
    { id: 'r-6', title: 'EF Core 性能优化指南', url: 'https://example.com/efcore-perf', type: 'article', difficulty: 3 },
    { id: 'r-6b', title: '从 Dapper 迁移到 EF Core', url: 'https://example.com/dapper-to-ef', type: 'article', difficulty: 2 },
    { id: 'r-6c', title: 'EF Core 中的值对象与自有类型', url: 'https://example.com/owned-types', type: 'doc', difficulty: 3 },
    { id: 'r-6d', title: 'DbContext 生命周期管理', url: 'https://example.com/dbcontext-lifecycle', type: 'video', difficulty: 1 },
  ],
  'node-3': [
    { id: 'r-7', title: '微服务架构设计模式', url: 'https://example.com/microservices-patterns', type: 'book', difficulty: 4 },
    { id: 'r-8', title: 'Saga 分布式事务实战', url: 'https://example.com/saga-pattern', type: 'article', difficulty: 3 },
    { id: 'r-8b', title: '服务发现与注册 — Consul 实战', url: 'https://example.com/consul', type: 'article', difficulty: 3 },
    { id: 'r-8c', title: 'API Gateway 模式深度解析', url: 'https://example.com/api-gateway', type: 'doc', difficulty: 2 },
    { id: 'r-8d', title: '微服务拆分策略 — 从单体到分布式', url: 'https://example.com/decompose', type: 'video', difficulty: 3 },
    { id: 'r-8e', title: 'gRPC vs REST — 微服务通信选型', url: 'https://example.com/grpc-vs-rest', type: 'article', difficulty: 2 },
  ],
  'node-6': [
    { id: 'r-redis-1', title: 'Redis 官方文档', url: 'https://redis.io/docs/', type: 'doc', difficulty: 1 },
    { id: 'r-redis-2', title: 'Redis 实战 — 缓存与队列', url: 'https://example.com/redis-in-action', type: 'book', difficulty: 2 },
    { id: 'r-redis-3', title: 'StackExchange.Redis 最佳实践', url: 'https://example.com/stackexchange-redis', type: 'article', difficulty: 2 },
    { id: 'r-redis-4', title: 'Redis 集群与哨兵模式', url: 'https://example.com/redis-cluster', type: 'doc', difficulty: 3 },
    { id: 'r-redis-5', title: 'Redis Streams 消息队列实战', url: 'https://example.com/redis-streams', type: 'video', difficulty: 2 },
    { id: 'r-redis-6', title: '缓存穿透/击穿/雪崩 — 解决方案', url: 'https://example.com/cache-problems', type: 'article', difficulty: 3 },
  ],
  'node-9': [
    { id: 'r-vue-1', title: 'Vue 3 官方文档', url: 'https://cn.vuejs.org/', type: 'doc', difficulty: 1 },
    { id: 'r-vue-2', title: 'Composition API 从入门到精通', url: 'https://example.com/composition-api', type: 'article', difficulty: 2 },
    { id: 'r-vue-3', title: 'Pinia 状态管理实战', url: 'https://example.com/pinia', type: 'video', difficulty: 2 },
    { id: 'r-vue-4', title: 'Vue 3 + TypeScript 项目最佳实践', url: 'https://example.com/vue3-ts', type: 'doc', difficulty: 3 },
    { id: 'r-vue-5', title: 'Vite 构建工具深入', url: 'https://example.com/vite-deep', type: 'article', difficulty: 2 },
    { id: 'r-vue-6', title: 'Vue Router 4 路由守卫与懒加载', url: 'https://example.com/vue-router4', type: 'article', difficulty: 1 },
  ],
  'default': [
    { id: 'r-d1', title: '入门教程', url: 'https://example.com/start', type: 'doc', difficulty: 1 },
    { id: 'r-d2', title: '进阶指南', url: 'https://example.com/advanced', type: 'article', difficulty: 3 },
    { id: 'r-d3', title: '实战项目演练', url: 'https://example.com/practice', type: 'video', difficulty: 2 },
  ],
};

export function getMockResources(nodeId: string): ResourceDto[] {
  return mockResourceLibrary[nodeId] || mockResourceLibrary['default'];
}

// ── Content: Interview Questions ──
const mockQuestionLibrary: Record<string, InterviewQuestionDto[]> = {
  'node-1': [
    { id: 'q-1', question: 'ASP.NET Core 的请求处理管道是怎样的？中间件的执行顺序是什么？', answerTip: '从 HttpContext 进入管道开始，按注册顺序依次执行，最后到达终端中间件。异常处理中间件应放在管道最前面', difficulty: 2, category: '基础' },
    { id: 'q-2', question: '依赖注入的三种生命周期有什么区别？', answerTip: 'Singleton（全局单例，整个应用生命周期）、Scoped（每请求一个实例，请求结束释放）、Transient（每次获取新实例）。注意 Singleton 不能依赖 Scoped 服务', difficulty: 1, category: '基础' },
    { id: 'q-3', question: '如何实现自定义 Middleware？', answerTip: '实现 IMiddleware 接口或使用 Use() 扩展方法，通过 RequestDelegate 调用下一个中间件。基于约定的中间件需在构造函数中注入 RequestDelegate', difficulty: 2, category: '实战' },
    { id: 'q-1b', question: 'appsettings.json 的配置加载优先级是怎样的？', answerTip: 'appsettings.{Environment}.json > appsettings.json > 环境变量 > 命令行参数 > Key Vault。后加载的覆盖先加载的同名配置', difficulty: 1, category: '基础' },
    { id: 'q-1c', question: '如何实现全局异常处理？', answerTip: '使用 UseExceptionHandler 中间件或自定义 IExceptionHandler。可配合 ProblemDetails 返回 RFC 7807 标准错误响应', difficulty: 2, category: '实战' },
  ],
  'node-2': [
    { id: 'q-4', question: 'EF Core 中 AsNoTracking() 的作用是什么？', answerTip: '禁用变更追踪，提升只读查询性能（约 2-3x）。适用于查询后不需要更新的场景。注意 AsNoTracking 的实体不会被 SaveChanges 保存', difficulty: 1, category: '性能' },
    { id: 'q-5', question: '什么是 N+1 查询问题？如何解决？', answerTip: '导航属性懒加载导致多次数据库查询（1 次主查询 + N 次关联查询）。使用 Include()/ThenInclude() 预加载相关数据，或使用 Explicit Loading 按需加载', difficulty: 2, category: '性能' },
    { id: 'q-5b', question: 'EF Core 中的并发冲突如何处理？', answerTip: '使用 RowVersion/Timestamp 列实现乐观并发控制。SaveChanges 时若并发令牌不匹配抛出 DbUpdateConcurrencyException，需实现重试或合并策略', difficulty: 3, category: '实战' },
    { id: 'q-5c', question: 'Shadow Properties 的使用场景是什么？', answerTip: '不需要在实体类中暴露的属性，通过 Fluent API 配置。常用于外键、审计字段（CreatedAt/UpdatedAt）、软删除标记等', difficulty: 2, category: '基础' },
  ],
  'node-3': [
    { id: 'q-m1', question: '如何决定服务的合理粒度？', answerTip: '按业务能力（Bounded Context）划分，遵循单一职责。一个微服务不应跨多个业务域。DDD 中的聚合根是很好的划分依据', difficulty: 3, category: '架构' },
    { id: 'q-m2', question: '分布式事务 Saga 模式有哪两种实现？', answerTip: '编排（Orchestration）：中央协调器指挥各步骤；协同（Choreography）：各服务通过事件驱动自主执行。编排更易理解和调试，协同更解耦但更复杂', difficulty: 3, category: '架构' },
    { id: 'q-m3', question: '微服务间如何实现数据一致性？', answerTip: '最终一致性 + 事件溯源。使用 Outbox Pattern 确保数据库更新和消息发送的原子性。避免分布式事务（2PC），改用补偿事务', difficulty: 3, category: '实战' },
  ],
  'node-6': [
    { id: 'q-redis-1', question: 'Redis 的五种基本数据类型是什么？', answerTip: 'String（字符串）、Hash（哈希表）、List（列表）、Set（集合）、Sorted Set（有序集合）。每种类型有各自适用场景：缓存用 String、对象用 Hash、队列用 List、去重用 Set、排行榜用 Sorted Set', difficulty: 1, category: '基础' },
    { id: 'q-redis-2', question: '缓存穿透、缓存击穿、缓存雪崩分别是什么？', answerTip: '穿透：查不存在的数据，绕过缓存打到 DB；击穿：热点 key 过期瞬间大量请求打到 DB；雪崩：大量 key 同时过期。解决方案：布隆过滤器、互斥锁、随机 TTL', difficulty: 2, category: '实战' },
    { id: 'q-redis-3', question: 'Redis 持久化 RDB 和 AOF 有什么区别？', answerTip: 'RDB：定时快照，恢复快但可能丢失最近数据；AOF：追加写日志，数据安全但文件大恢复慢。生产环境建议混合使用', difficulty: 2, category: '基础' },
  ],
  'node-9': [
    { id: 'q-vue-1', question: 'Vue 3 Composition API 相比 Options API 有什么优势？', answerTip: '更好的逻辑复用（composables）、更好的类型推导（TypeScript）、更灵活的代码组织。同一功能相关代码可以放在一起而不是分散在 data/methods/computed 中', difficulty: 1, category: '基础' },
    { id: 'q-vue-2', question: 'ref 和 reactive 的区别和使用场景？', answerTip: 'ref 用于基础类型和需要替换整个对象的场景，通过 .value 访问；reactive 用于对象类型，直接访问属性。ref 可重新赋值而 reactive 不能。建议统一使用 ref', difficulty: 1, category: '基础' },
    { id: 'q-vue-3', question: 'Vue 3 的响应式原理是什么？', answerTip: '基于 ES6 Proxy 实现，相比 Vue 2 的 Object.defineProperty：支持数组索引和 length 的响应式、支持 Map/Set、性能更好、不需要递归遍历', difficulty: 2, category: '原理' },
  ],
  'default': [
    { id: 'q-d1', question: '请描述该技术的核心原理', answerTip: '从架构设计和关键机制两方面回答', difficulty: 2, category: '概览' },
    { id: 'q-d2', question: '该技术在你的项目中如何应用？', answerTip: '结合实际业务场景说明', difficulty: 2, category: '实战' },
    { id: 'q-d3', question: '性能优化的关键点有哪些？', answerTip: '从查询优化、缓存策略、资源管理三个维度考虑', difficulty: 3, category: '性能' },
  ],
};

export function getMockQuestions(nodeId: string): InterviewQuestionDto[] {
  return mockQuestionLibrary[nodeId] || mockQuestionLibrary['default'];
}

// ── Learning: Profile ──
export const mockSkillRadar: SkillRadarItem[] = [
  { nodeId: 'node-1', nodeTitle: 'ASP.NET Core', selfScore: 85, level: 4 },
  { nodeId: 'node-2', nodeTitle: 'EF Core', selfScore: 72, level: 3 },
  { nodeId: 'node-3', nodeTitle: '微服务架构', selfScore: 60, level: 2 },
  { nodeId: 'node-6', nodeTitle: 'Redis', selfScore: 78, level: 3 },
  { nodeId: 'node-7', nodeTitle: 'RabbitMQ', selfScore: 55, level: 2 },
  { nodeId: 'node-9', nodeTitle: 'Vue 3', selfScore: 68, level: 3 },
  { nodeId: 'node-10', nodeTitle: 'IoT 平台', selfScore: 45, level: 1 },
];

export const mockLearningProfile: LearningProfile = {
  userId: 'u-001',
  streakDays: 23,
  totalCheckIns: 156,
  weeklyMinutes: 480,
  skillRadar: mockSkillRadar,
};

export const mockCheckIns: CheckInDto[] = [
  { id: 'ci-1', date: '2026-06-09', durationMinutes: 90, note: '学习了 ASP.NET Core 中间件管道' },
  { id: 'ci-2', date: '2026-06-08', durationMinutes: 60, note: 'EF Core 迁移与种子数据' },
  { id: 'ci-3', date: '2026-06-07', durationMinutes: 120, note: '微服务 Saga 模式实践' },
  { id: 'ci-4', date: '2026-06-06', durationMinutes: 45, note: 'Redis 缓存策略回顾' },
  { id: 'ci-5', date: '2026-06-05', durationMinutes: 75, note: 'RabbitMQ 死信队列配置' },
  { id: 'ci-6', date: '2026-06-04', durationMinutes: 50, note: 'Vue 3 Composition API 练习' },
  { id: 'ci-7', date: '2026-06-03', durationMinutes: 100, note: 'YARP 网关路由规则调试' },
];

export const mockAssessments: AssessmentDto[] = [
  { id: 'as-1', title: 'ASP.NET Core 基础评测', type: '选择', nodeId: 'node-1', questions: JSON.parse('[]'), timeLimit: 30, passScore: 70 },
  { id: 'as-2', title: 'ASP.NET Core 进阶架构设计', type: '设计', nodeId: 'node-1', questions: JSON.parse('[]'), timeLimit: 45, passScore: 75 },
  { id: 'as-3', title: 'EF Core 性能优化评测', type: '选择', nodeId: 'node-2', questions: JSON.parse('[]'), timeLimit: 20, passScore: 65 },
  { id: 'as-4', title: '微服务架构设计评测', type: '设计', nodeId: 'node-3', questions: JSON.parse('[]'), timeLimit: 60, passScore: 75 },
  { id: 'as-5', title: 'Redis 缓存策略实战', type: '混合', nodeId: 'node-6', questions: JSON.parse('[]'), timeLimit: 30, passScore: 70 },
  { id: 'as-6', title: 'Vue 3 组件开发评测', type: '选择', nodeId: 'node-9', questions: JSON.parse('[]'), timeLimit: 25, passScore: 68 },
];

// ── Collaboration: Notes ──
export const mockNotes: NoteDto[] = [
  {
    id: 'note-1', title: 'ASP.NET Core 中间件管道详解', nodeId: 'node-1', userId: 'u-001',
    content: `## 请求处理管道

ASP.NET Core 的请求处理管道由一系列中间件组成，每个中间件可以：
- 处理请求并传递给下一个中间件
- 处理请求并短路（不传递给下一个）
- 在下一个中间件返回后处理响应

\`\`\`csharp
app.Use(async (context, next) =>
{
    // 请求前逻辑
    await next.Invoke();
    // 响应后逻辑
});
\`\`\`

### 关键点
- 中间件注册顺序决定执行顺序
- UseExceptionHandler 应放在管道最前面
- UseStaticFiles 应在路由之前`,
    status: 'published', viewCount: 245, likeCount: 32, createdAt: '2026-06-08T10:30:00Z',
  },
  {
    id: 'note-2', title: '微服务间通信方案对比', nodeId: 'node-3', userId: 'u-001',
    content: `## 同步 vs 异步通信

### HTTP REST (同步)
- 简单直接，易于调试
- 适合查询类操作
- 需要注意超时和重试

### RabbitMQ (异步)
- 解耦服务，削峰填谷
- 适合事件驱动场景
- 需要考虑消息幂等性`,
    status: 'published', viewCount: 189, likeCount: 28, createdAt: '2026-06-07T14:20:00Z',
  },
  {
    id: 'note-3', title: 'Redis 缓存实战心得', nodeId: 'node-6', userId: 'u-001',
    content: `## 缓存策略

1. **Cache Aside**: 先查缓存，miss 再查 DB，更新 DB 后删缓存
2. **Write Through**: 同时更新缓存和 DB
3. **Write Behind**: 先写缓存，异步刷到 DB

推荐使用 StackExchange.Redis 客户端。`,
    status: 'published', viewCount: 132, likeCount: 18, createdAt: '2026-06-06T09:00:00Z',
  },
  {
    id: 'note-4', title: 'Vue 3 状态管理选型', nodeId: 'node-9', userId: 'u-001',
    content: `## Pinia vs Vuex

Pinia 优势：
- 完整的 TypeScript 支持
- 无需 mutations，直接修改状态
- 扁平化结构，不再需要 modules 嵌套
- 更好的 DevTools 支持`,
    status: 'published', viewCount: 98, likeCount: 15, createdAt: '2026-06-05T16:45:00Z',
  },
];

// ── Collaboration: Discussions ──
export const mockDiscussions: DiscussionDto[] = [
  {
    id: 'disc-1', nodeId: 'node-1', userId: 'u-001',
    content: '大家在生产环境中使用 Minimal API 还是 Controller-based API？各有什么优劣？',
    parentId: null, createdAt: '2026-06-09T08:00:00Z',
    replies: [
      { id: 'disc-2', nodeId: 'node-1', userId: 'u-002', content: '我们团队用 Controller，因为项目较大，需要更好的组织性。Minimal API 适合微服务和小型端点。', parentId: 'disc-1', createdAt: '2026-06-09T09:30:00Z', replies: [] },
      { id: 'disc-3', nodeId: 'node-1', userId: 'u-003', content: 'Minimal API + Carter 库可以实现模块化，推荐试试。', parentId: 'disc-1', createdAt: '2026-06-09T10:15:00Z', replies: [] },
    ],
  },
  {
    id: 'disc-4', nodeId: 'node-7', userId: 'u-001',
    content: 'RabbitMQ 集群方案推荐？Kubernetes 上部署有什么注意事项？',
    parentId: null, createdAt: '2026-06-08T11:00:00Z',
    replies: [
      { id: 'disc-5', nodeId: 'node-7', userId: 'u-002', content: 'K8s 上推荐用 RabbitMQ Cluster Operator，注意持久化卷的配置。', parentId: 'disc-4', createdAt: '2026-06-08T14:20:00Z', replies: [] },
    ],
  },
];

// ── CodeRunner ──
export const mockLanguages: LanguageInfo[] = [
  { id: 'csharp', name: 'C# (.NET 6)', template: 'using System;\n\nConsole.WriteLine("Hello, SkillPlatform!");\n' },
];

export function mockRunCode(code: string): CodeRunResult {
  if (code.includes('error') || code.includes('throw')) {
    return { success: false, output: '', errors: 'CS0029: 无法将类型 "string" 隐式转换为 "int"', compilationSucceeded: false, executionTimeMs: 0 };
  }
  return {
    success: true,
    output: `Hello, SkillPlatform!\n执行成功 · ${new Date().toLocaleTimeString()}\n`,
    errors: '',
    compilationSucceeded: true,
    executionTimeMs: Math.floor(Math.random() * 200) + 15,
  };
}
