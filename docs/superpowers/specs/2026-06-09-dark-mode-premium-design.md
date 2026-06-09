# 深色模式 + 高级感 UI — 设计文档

> 2026-06-09 · 技术能力提升平台前端升级

## 1. 概述

为 Vue 3 前端（11 页面）添加深色模式支持，采用 CSS 变量（Design Tokens）架构实现主题切换，同时提升整体 UI 的高级感/企业级品质。

### 目标

- 深色模式：默认跟随系统 `prefers-color-scheme`，用户可手动切换并持久化
- 高级感：简约企业风（浅色）+ 科技蓝调风（深色）融合
- 动效：颜色平滑过渡 + 页面淡入淡出 + 卡片微动效 + 仪表盘粒子背景

## 2. 技术方案

**方案 A：CSS 变量重构**

- 所有颜色提取为 `--color-*` CSS 自定义属性
- 通过 `[data-theme="dark"]` 选择器切换变量值
- Pinia store 管理主题状态，`document.documentElement.setAttribute` 驱动
- localStorage 持久化 + `matchMedia` 监听系统变化

## 3. CSS 变量体系

### 基础色板
```
--color-primary:        #2563eb (浅) / #3b82f6 (深)
--color-primary-hover:  #1d4ed8 / #60a5fa
--color-primary-light:  #eff6ff / #1e3a5f
--color-success:        #16a34a
--color-warning:        #f59e0b
--color-danger:         #dc2626
```

### 语义色（浅 → 深）
```
--bg-base:        #f8fafc → #0b1120
--bg-surface:     #ffffff → #1e293b
--bg-elevated:    #ffffff → #334155
--text-primary:   #0f172a → #f1f5f9
--text-secondary: #475569 → #94a3b8
--text-muted:     #94a3b8 → #64748b
--border-default: #e2e8f0 → #334155
--shadow-sm/md/lg: 浅色轻阴影 → 深色重阴影
```

## 4. 主题切换架构

### 文件变更

| 操作 | 文件 | 说明 |
|------|------|------|
| 新增 | `src/stores/theme.ts` | Pinia store，管理 theme 状态、localStorage、matchMedia |
| 新增 | `src/composables/useTheme.ts` | `useTheme()` 便捷 composable |
| 新增 | `src/components/ThemeToggle.vue` | 🌙/☀️ 切换按钮，全局 Header 使用 |
| 修改 | `src/App.vue` | onMounted 初始化主题属性 |
| 修改 | `src/style.css` | 全部重写：CSS 变量 + 双主题 + 新样式 |

### 数据流
```
首次加载: localStorage → matchMedia → setAttribute
手动切换: toggle() → store.theme → setAttribute → localStorage.set
系统变化: matchMedia listener → store.theme（仅在无手动偏好时）
```

## 5. 逐页面升级

### 全局
- 引入 Inter 字体（Google Fonts CDN）
- 全局间距增大 20%，圆角统一 --radius-sm/md/lg
- 所有可交互元素 `transition: all 0.2s ease`
- 根元素 `transition: background-color 0.3s, color 0.2s`

### 登录/注册
- 卡片 `box-shadow` 内发光微光边框
- 输入框 focus 时边框渐变色（`border-image: linear-gradient(...)`）
- 按钮 loading 态骨架

### 仪表盘
- 顶部欢迎卡片渐变背景 + Canvas 粒子动画（仅浅色模式）
- 快捷入口图标 hover 浮动（`translateY(-3px)`）
- 技术栈卡片 hover 边框发光（`box-shadow` 扩散 + 彩色边框）

### 技术图谱
- 节点卡片 hover 上浮 `translateY(-4px)` + `shadow-lg`
- 图标 hover 放大 1.1x + 旋转 5deg

### 节点详情
- Tab 切换下滑线 `transition: left 0.3s`
- 资源列表 hover 左边框蓝色高亮
- Markdown 正文行高 1.8 + 段落间距优化

### 学习面板
- 统计数字渐变色（`background-clip: text`）
- 技能进度条渐变色填充 + `transition: width 0.6s`
- 打卡按钮 ripple 波纹效果

### 笔记广场/详情
- 笔记卡片作者头像圆形占位
- 讨论嵌套回复左侧竖线视觉引导
- Markdown 代码块深色主题适配

### 在线代码编辑器
- 深色模式下编辑器区背景 `#0d1117`
- 运行按钮脉冲动画（`@keyframes pulse`）
- 输出区等宽字体，错误区红底

### 管理后台
- 统计卡片数字渐变色
- 审核按钮 hover scale(1.05)

## 6. 粒子背景（Canvas）

仪表盘首页欢迎卡片下方使用 Canvas 绘制漂浮粒子：
- 50-80 个淡蓝半透明圆点
- 缓慢随机移动 + 鼠标微弱吸引
- 仅浅色模式启用，深色模式关闭
- 使用 `requestAnimationFrame` 驱动

文件：`src/components/ParticleBackground.vue`

## 7. 字体

```html
<link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
```

```css
body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
```

## 8. 实施步骤

1. 创建 `style.css` 完整重写（CSS 变量 + 双主题 + 全局样式）
2. 创建 `src/stores/theme.ts` + `src/composables/useTheme.ts`
3. 创建 `src/components/ThemeToggle.vue`
4. 更新 `src/App.vue`、`src/main.ts`、`index.html`（Inter 字体）
5. 更新 11 个页面：将 scoped 样式中硬编码颜色替换为 `var(--color-*)`
6. 创建 `src/components/ParticleBackground.vue`，集成到 DashboardView
7. 添加各页面微动效和高级感细节
8. `npm run build` 验证构建通过
