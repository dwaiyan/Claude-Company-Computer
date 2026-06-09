<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRouter } from 'vue-router';
import { learningApi } from '@/api/learning'; import type { LearningProfile, CheckInDto } from '@/api/learning';
import ThemeToggle from '@/components/ThemeToggle.vue';

const router = useRouter(); const profile = ref<LearningProfile | null>(null);
const checkIns = ref<CheckInDto[]>([]); const note = ref(''); const minutes = ref(30); const loading = ref(true);

onMounted(async () => {
  try { const [p, c] = await Promise.all([learningApi.getProfile(), learningApi.getCheckIns()]); profile.value = p; checkIns.value = c; }
  finally { loading.value = false; }
});

async function doCheckIn() {
  await learningApi.checkIn(minutes.value, note.value || undefined); note.value = '';
  const [p, c] = await Promise.all([learningApi.getProfile(), learningApi.getCheckIns()]); profile.value = p; checkIns.value = c;
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>我的学习</h1><nav class="nav-links"><router-link to="/">←</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div v-if="loading" class="loading">加载中...</div>
      <template v-else-if="profile">
        <div class="stats-row stagger-1">
          <div class="stat-card"><div class="stat-num">{{ profile.streakDays }}</div><div class="stat-label">连续打卡</div></div>
          <div class="stat-card"><div class="stat-num">{{ profile.weeklyMinutes }}</div><div class="stat-label">本周分钟</div></div>
          <div class="stat-card"><div class="stat-num">{{ profile.totalCheckIns }}</div><div class="stat-label">总打卡</div></div>
        </div>

        <div class="card stagger-2">
          <h2>今日打卡</h2>
          <div style="display:flex;gap:10px;">
            <input v-model.number="minutes" type="number" min="1" placeholder="学习时长(分钟)" class="form-input" style="flex:1;" />
            <input v-model="note" type="text" placeholder="学习笔记(可选)" class="form-input" style="flex:2;" />
            <button @click="doCheckIn" class="btn-primary" style="width:auto;">打卡</button>
          </div>
        </div>

        <div class="card stagger-3">
          <h2>技能雷达</h2>
          <div v-if="profile.skillRadar.length" class="skill-list">
            <div v-for="s in profile.skillRadar" :key="s.nodeId" class="skill-row">
              <span class="skill-name">{{ s.nodeTitle || s.nodeId.slice(0,8) }}</span>
              <div class="skill-bar"><div class="skill-fill" :style="{ width: s.selfScore * 20 + '%' }"></div></div>
              <span class="skill-score">{{ s.selfScore }}/5</span>
            </div>
          </div>
          <p v-else class="empty">去 <a href="#" @click.prevent="router.push('/tech-tree')">技术图谱</a> 开始学习</p>
        </div>
      </template>
    </div>
  </div>
</template>

<style scoped>
.form-input { padding: 8px 12px; background: var(--bg-surface); border: 1px solid var(--border-default); border-radius: var(--radius-md); font-family: var(--font-body); font-size: 14px; color: var(--text-primary); transition: border-color var(--transition-fast); }
.form-input:focus { outline: none; border-color: var(--color-primary); box-shadow: 0 0 0 2px var(--color-primary-glow); }
.skill-list { display: flex; flex-direction: column; gap: 12px; }
.skill-row { display: flex; align-items: center; gap: 12px; }
.skill-name { width: 80px; font-size: 11px; font-family: var(--font-display); letter-spacing: 0.03em; color: var(--text-secondary); }
.skill-bar { flex: 1; height: 6px; background: var(--bg-muted); border-radius: 3px; overflow: hidden; }
.skill-fill { height: 100%; background: linear-gradient(90deg, var(--color-primary), var(--color-accent)); border-radius: 3px; transition: width 0.8s var(--transition-base); }
.skill-score { font-family: var(--font-display); font-size: 11px; color: var(--text-muted); }
</style>
