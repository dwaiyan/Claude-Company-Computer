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
      <div class="user-info">
        <span v-if="auth.user">你好，{{ auth.user.username }}</span>
        <button @click="handleLogout" class="btn-link">退出</button>
      </div>
    </header>

    <main class="dashboard-main">
      <div class="welcome-card">
        <h2>技术能力提升平台</h2>
        <p>功能模块将在 P2~P4 阶段逐步建设。当前 P1 骨架已就绪。</p>
      </div>

      <div class="tech-stack-grid">
        <div class="tech-card">
          <h3>.NET 6 微服务</h3>
          <p>ASP.NET Core, EF Core, YARP</p>
        </div>
        <div class="tech-card">
          <h3>数据存储</h3>
          <p>PostgreSQL, Redis, RabbitMQ</p>
        </div>
        <div class="tech-card">
          <h3>前端框架</h3>
          <p>Vue 3 + Pinia + Vite</p>
        </div>
        <div class="tech-card">
          <h3>业务领域</h3>
          <p>SaaS · IoT · APS · 数字孪生</p>
        </div>
      </div>
    </main>
  </div>
</template>
