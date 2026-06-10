<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';
import ThemeToggle from '@/components/ThemeToggle.vue';

const auth = useAuthStore();
const router = useRouter();

function handleLogout() { auth.logout(); router.push('/login'); }
</script>

<template>
  <div class="dashboard">
    <header class="app-header">
      <div class="header-left">
        <h1>技术能力提升平台</h1>
        <nav class="nav-links">
          <router-link to="/tech-tree">图谱</router-link>
          <router-link to="/learning">学习</router-link>
          <router-link to="/notes">笔记</router-link>
          <router-link to="/code">代码</router-link>
          <router-link v-if="auth.isAdmin" to="/admin">管理</router-link>
        </nav>
      </div>
      <div class="header-right">
        <ThemeToggle />
        <span v-if="auth.user" class="user-info">{{ auth.user.username }}</span>
        <button @click="handleLogout" class="btn-ghost">退出</button>
      </div>
    </header>

    <main class="dashboard-main">
      <div class="welcome-card stagger-1">
        <h2>技术能力提升平台</h2>
        <p>.NET 微服务 &middot; IoT &middot; APS &middot; 数字孪生 — 系统化提升技术实力</p>
      </div>

      <div class="quick-actions">
        <router-link to="/tech-tree" class="action-card stagger-2">
          <span class="action-icon">▣</span>
          <div>
            <h3>技术图谱</h3>
            <p>.NET 生态技术栈全景浏览</p>
          </div>
        </router-link>
        <router-link to="/learning" class="action-card stagger-3">
          <span class="action-icon">◈</span>
          <div>
            <h3>学习追踪</h3>
            <p>打卡 · 技能雷达 · 评测</p>
          </div>
        </router-link>
        <router-link to="/code" class="action-card stagger-4">
          <span class="action-icon">▸</span>
          <div>
            <h3>C# 在线运行</h3>
            <p>.NET 代码在线编译执行</p>
          </div>
        </router-link>
      </div>

      <div class="tech-stack-grid">
        <div class="tech-card"><h3>.NET 6 微服务</h3><p>ASP.NET Core · EF Core · YARP</p></div>
        <div class="tech-card"><h3>数据与存储</h3><p>PostgreSQL · MySQL · Redis</p></div>
        <div class="tech-card"><h3>消息与通信</h3><p>RabbitMQ · MQTT</p></div>
        <div class="tech-card"><h3>业务领域</h3><p>SaaS · IoT · APS · 数字孪生</p></div>
      </div>
    </main>
  </div>
</template>
