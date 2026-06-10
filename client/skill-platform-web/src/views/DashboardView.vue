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
        <h1>技術能力提升平台</h1>
        <nav class="nav-links">
          <router-link to="/tech-tree">图谱</router-link>
          <router-link to="/learning">学习</router-link>
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
        <h2>技術能力提升平台</h2>
        <p>.NET マイクロサービス · IoT · APS · デジタルツイン</p>
      </div>

      <div class="quick-actions">
        <router-link to="/tech-tree" class="action-card stagger-2">
          <span class="action-icon">▣</span>
          <div>
            <h3>技術図譜</h3>
            <p>.NET エコシステム技術スタック</p>
          </div>
        </router-link>
        <router-link to="/learning" class="action-card stagger-3">
          <span class="action-icon">◈</span>
          <div>
            <h3>学習記録</h3>
            <p>チェックイン · スキルレーダー · 評価</p>
          </div>
        </router-link>
        <router-link to="/code" class="action-card stagger-4">
          <span class="action-icon">▸</span>
          <div>
            <h3>C# オンライン</h3>
            <p>.NET コードのオンライン実行</p>
          </div>
        </router-link>
      </div>

      <div class="tech-stack-grid">
        <div class="tech-card"><h3>.NET 6 マイクロサービス</h3><p>ASP.NET Core · EF Core · YARP</p></div>
        <div class="tech-card"><h3>データ と ストレージ</h3><p>PostgreSQL · MySQL · Redis</p></div>
        <div class="tech-card"><h3>メッセージ と 通信</h3><p>RabbitMQ · MQTT</p></div>
        <div class="tech-card"><h3>ビジネス領域</h3><p>SaaS · IoT · APS · デジタルツイン</p></div>
      </div>
    </main>
  </div>
</template>
