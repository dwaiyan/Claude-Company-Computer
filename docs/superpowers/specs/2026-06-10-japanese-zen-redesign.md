# 日式禅意 Editorial — 前端 Redesign 设计文档

> 2026-06-10 · 技术能力提升平台前端重设计

## 1. 设计方向

**日式禅意 Editorial（Japanese Zen Editorial）** — 以侘寂美学为核心的编辑级排版风格。

### 美学原则

| 原则 | 含义 | 设计表现 |
|------|------|----------|
| 不對称 (Asymmetry) | 有意的不平衡 | 网格中有意打破对称，左重右轻或上重下轻 |
| 余白 (Negative Space) | 慷慨的留白 | 页面呼吸感强，段落间距大（1.8x 行高） |
| 渋味 (Shibui) | 克制的色彩 | 只用朱红 + 藍两个强调色，无荧光色 |
| 自然 (Natural) | 材质感 | 暖白底如和纸，墨色字如書道，冷黑底如墨汁 |

### 拒绝

- ❌ 圆润卡片（max 4px border-radius）
- ❌ 荧光色 / 渐变背景
- ❌ 大量阴影堆叠
- ❌ 通用 AI 字体（Inter, Roboto, Arial）
- ❌ 刻板和风装饰（樱花、波浪纹、毛笔笔刷）

---

## 2. 设计系统

### 2.1 色彩体系

#### 浅色 · 生成り (Warm Parchment)

```
--bg-base:        #f8f7f4    和纸暖白
--bg-surface:     #ffffff    卡片白
--bg-elevated:    #ffffff    浮层白
--bg-muted:       #f3f1ec    灰白（如榻榻米）

--text-primary:   #2c2416    墨色（微暖黑）
--text-secondary: #8b7355    茶色
--text-muted:     #c4b998    淡茶

--border-default: #e5e2da    纸边色
--border-subtle:  #f0ede6    微纸边

--color-accent:   #8b4513    朱红（链接、强调）
--color-accent2:  #5b7fbd    藍色（次要强调、信息）

--shadow-sm:  none           (不用阴影)
--shadow-md:  0 2px 8px rgba(44,36,22,0.04)  极淡墨影
```

#### 深色 · 藍墨 (Midnight Indigo)

```
--bg-base:        #0d1117    冷黑（月光夜色）
--bg-surface:     #161b22    卡片黑
--bg-elevated:    #1c2128    浮层黑
--bg-muted:       #11161c    微黑

--text-primary:   #c8d6e5    冷白
--text-secondary: #8b9dc3    靛灰
--text-muted:     #5c6a82    暗靛

--border-default: #21262d    墨蓝边
--border-subtle:  #1a1f26

--color-accent:   #5b7fbd    藍色（主强调）
--color-accent2:  #c2410c    朱红（警告、危险）
```

### 2.2 字体

| 用途 | 字体 | 备选 |
|------|------|------|
| 标题 | 'Noto Serif CJK SC', 'Source Han Serif SC' | Georgia, serif |
| 正文 | 'Noto Sans CJK SC', 'Source Han Sans SC' | system-ui, sans-serif |
| 代码 | 'SF Mono', 'Consolas' | monospace |

```html
<link href="https://fonts.googleapis.com/css2?family=Noto+Serif+SC:wght@300;400;700&family=Noto+Sans+SC:wght@300;400;500;700&display=swap" rel="stylesheet">
```

### 2.3 间距与圆角

```
--space-xs: 4px
--space-sm: 8px
--space-md: 16px
--space-lg: 24px
--space-xl: 32px
--space-2xl: 48px
--space-3xl: 64px

--radius-sm: 2px    极微圆角（直角感）
--radius-md: 4px
--radius-lg: 6px    最大圆角（拒绝胶囊感）
```

### 2.4 动效

- 过渡：`250ms cubic-bezier(0.4, 0, 0.2, 1)` — 柔和不突兀
- 页面入场：`fadeInUp` 400ms + 交错延迟（stagger 0.08s）
- 暗色切换：`300ms` 背景色 + `200ms` 文字色
- hover：仅 `color` / `opacity` 变化，不做 scale（已修）
- **不做**：bounce、spring、粒子背景、scanline

---

## 3. 页面布局策略（混合编辑）

| 页面 | 布局策略 | 理由 |
|------|----------|------|
| 登录/注册 | 单列极简 | 焦点在表单 |
| 仪表盘 | 杂志跨页 | 数据 + 入口，信息丰富 |
| 技术图谱 | 网格画廊 | 节点浏览，视觉节奏 |
| 节点详情 | 单列长文 | 连续阅读流 |
| 学习面板 | 杂志跨页 | 统计 + 技能雷达 |
| 笔记广场 | 网格画廊 | 卡片式浏览 |
| 笔记详情 | 单列长文 | Markdown 阅读 |
| 代码编辑器 | 单列工具 | 编辑器聚焦 |
| 管理后台 | 层叠排版 | 数据面板 |

---

## 4. 逐页面设计

### 4.1 登录/注册
- 极简居中卡片 + 大量留白
- 标题用大号 Serif，无副标题装饰
- 输入框底边线（单一线条，无边框包围）
- 按钮纯色块，无圆角 (`border-radius: 2px`)
- 错误提示左侧细竖线（朱红色）

### 4.2 仪表盘
- 欢迎区：大号 Serif 标题 + 细线分隔
- 统计数据跨页双列：左大数字 + 右趋势条
- 快捷入口：3 列网格，每格左右结构（符号 + 标题）
- 技术栈：4 列网格，仅显示名称，hover 变茶色

### 4.3 技术图谱
- 2-3 列瀑布流
- 节点卡片：节点符号 + 标题 + 一行描述
- hover：左边线变朱红 + 标题色变墨色

### 4.4 节点详情
- 单列 680px 最大宽度
- Tab 导航用底部细线指示
- 资源列表：左边框 hover 高亮
- Markdown 正文行高 2.0

### 4.5 学习面板
- 统计数字用 Serif 大字重
- 技能条：细线 + 纯色填充，无渐变
- 打卡按钮：纯色块 + 微 hover 变暗

### 4.6 笔记广场/详情
- 笔记卡片：标题 + 首行摘要 + 底栏统计
- hover：标题色从墨色变朱红
- 详情页：单列阅读，代码块深色背景

### 4.7 代码编辑器
- 编辑器区全宽，暗色背景（#0d1117）
- 输出区等宽字体 + 细线分隔
- 运行按钮纯色 + 微动效

### 4.8 管理后台
- 统计卡片：Serif 大字 + 标签小字
- 审核列表：左竖线状态指示（绿=通过，红=拒绝）

---

## 5. 文件变更

| 操作 | 文件 | 说明 |
|------|------|------|
| 修改 | `index.html` | 更新标题，引入 Google Fonts (Noto Serif/Sans SC) |
| 重写 | `src/style.css` | 完全替换：CSS 变量 + 日式禅意双主题 + 全局样式 |
| 修改 | `src/views/LoginView.vue` | 日式极简表单 |
| 修改 | `src/views/RegisterView.vue` | 日式极简表单 |
| 修改 | `src/views/DashboardView.vue` | 杂志跨页布局 |
| 修改 | `src/views/TechTreeView.vue` | 网格画廊 + hover 效果 |
| 修改 | `src/views/NodeDetailView.vue` | 单列长文布局 |
| 修改 | `src/views/LearningView.vue` | 杂志跨页 + 技能条 |
| 修改 | `src/views/NotesView.vue` | 卡片网格 |
| 修改 | `src/views/NoteDetailView.vue` | 单列阅读 |
| 修改 | `src/views/CodeEditorView.vue` | 编辑器聚焦 |
| 修改 | `src/views/AdminView.vue` | 数据面板 |
| 修改 | `src/components/ThemeToggle.vue` | 日式开关（墨/月图标） |
| 删除 | `src/components/ParticleBackground.vue` | 禅意不做粒子 |

---

## 6. 实施步骤

1. 更新 `index.html`（字体 + 标题）
2. 重写 `style.css`（全局 Design Tokens + 布局系统）
3. 重写 `ThemeToggle.vue`（墨/月切换）
4. 逐页面更新：Login → Register → Dashboard → TechTree → NodeDetail → Learning → Notes → NoteDetail → CodeEditor → Admin
5. 删除 `ParticleBackground.vue`
6. 验证：`npm run build` 构建通过 + 10 个页面手动检查
