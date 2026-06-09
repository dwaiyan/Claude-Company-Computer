<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';

const auth = useAuthStore();
const router = useRouter();

function handleLogout() {
  auth.logout();
  router.push('/login');
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header">
      <h1>技术能力提升平台</h1>
      <nav class="nav-links">
        <router-link to="/tech-tree">技术图谱</router-link>
        <router-link to="/learning">我的学习</router-link>
      </nav>
      <div class="user-info">
        <span v-if="auth.user">你好，{{ auth.user.username }}</span>
        <button @click="handleLogout" class="btn-link">退出</button>
      </div>
    </header>

    <main class="dashboard-main">
      <div class="welcome-card">
        <h2>技术能力提升平台</h2>
        <p>.NET 微服务 · IoT · APS · 数字孪生 — 系统化提升技术能力</p>
      </div>

      <div class="quick-actions">
        <router-link to="/tech-tree" class="action-card">
          <span class="action-icon">📚</span>
          <h3>技术图谱</h3>
          <p>浏览 .NET 生态技术栈</p>
        </router-link>
        <router-link to="/learning" class="action-card">
          <span class="action-icon">📊</span>
          <h3>我的学习</h3>
          <p>打卡 · 技能雷达 · 评测</p>
        </router-link>
      </div>

      <div class="tech-stack-grid">
        <div class="tech-card">
          <h3>.NET 6 微服务</h3>
          <p>ASP.NET Core, EF Core, YARP</p>
        </div>
        <div class="tech-card">
          <h3>数据存储</h3>
          <p>PostgreSQL, MySQL, Redis</p>
        </div>
        <div class="tech-card">
          <h3>消息队列</h3>
          <p>RabbitMQ, MQTT</p>
        </div>
        <div class="tech-card">
          <h3>业务领域</h3>
          <p>SaaS · IoT · APS · 数字孪生</p>
        </div>
      </div>
    </main>
  </div>
</template>
