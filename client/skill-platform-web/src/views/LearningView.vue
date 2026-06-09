<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { learningApi } from '@/api/learning';
import type { LearningProfile, CheckInDto } from '@/api/learning';

const router = useRouter();
const profile = ref<LearningProfile | null>(null);
const checkIns = ref<CheckInDto[]>([]);
const note = ref('');
const minutes = ref(30);
const loading = ref(true);

onMounted(async () => {
  try {
    const [p, c] = await Promise.all([
      learningApi.getProfile(),
      learningApi.getCheckIns(),
    ]);
    profile.value = p;
    checkIns.value = c;
  } finally {
    loading.value = false;
  }
});

async function doCheckIn() {
  await learningApi.checkIn(minutes.value, note.value || undefined);
  note.value = '';
  const [p, c] = await Promise.all([
    learningApi.getProfile(),
    learningApi.getCheckIns(),
  ]);
  profile.value = p;
  checkIns.value = c;
}

function goToTechTree() {
  router.push('/tech-tree');
}
</script>

<template>
  <div class="page-container">
    <div v-if="loading" class="loading">加载中...</div>

    <template v-else-if="profile">
      <header class="page-header">
        <h1>我的学习</h1>
      </header>

      <div class="stats-row">
        <div class="stat-card">
          <div class="stat-num">{{ profile.streakDays }}</div>
          <div class="stat-label">连续打卡</div>
        </div>
        <div class="stat-card">
          <div class="stat-num">{{ profile.weeklyMinutes }}</div>
          <div class="stat-label">本周分钟</div>
        </div>
        <div class="stat-card">
          <div class="stat-num">{{ profile.totalCheckIns }}</div>
          <div class="stat-label">总打卡</div>
        </div>
      </div>

      <div class="card">
        <h2>今日打卡</h2>
        <div class="checkin-form">
          <input v-model.number="minutes" type="number" min="1" placeholder="学习时长(分钟)" />
          <input v-model="note" type="text" placeholder="学习笔记(可选)" />
          <button @click="doCheckIn" class="btn-primary">打卡</button>
        </div>
      </div>

      <div class="card">
        <h2>技能雷达</h2>
        <div v-if="profile.skillRadar.length" class="radar-list">
          <div v-for="s in profile.skillRadar" :key="s.nodeId" class="radar-item">
            <span>{{ s.nodeTitle || s.nodeId }}</span>
            <div class="bar"><div class="fill" :style="{ width: s.selfScore * 20 + '%' }"></div></div>
          </div>
        </div>
        <p v-else class="empty">暂无技能评估，去 <a href="#" @click.prevent="goToTechTree">技术图谱</a> 学习吧</p>
      </div>
    </template>
  </div>
</template>

<style scoped>
.page-container { max-width: 800px; margin: 0 auto; padding: 32px 24px; }
.page-header h1 { font-size: 24px; color: #1a73e8; }
.stats-row { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; margin: 24px 0; }
.stat-card { background: #fff; border-radius: 10px; padding: 20px; text-align: center; box-shadow: 0 1px 6px rgba(0,0,0,0.06); }
.stat-num { font-size: 28px; font-weight: 600; color: #1a73e8; }
.stat-label { font-size: 13px; color: #888; margin-top: 4px; }
.card { background: #fff; border-radius: 10px; padding: 24px; margin-bottom: 20px; box-shadow: 0 1px 6px rgba(0,0,0,0.06); }
.card h2 { font-size: 16px; margin-bottom: 16px; }
.checkin-form { display: flex; gap: 10px; }
.checkin-form input { flex: 1; padding: 8px 12px; border: 1px solid #d9d9d9; border-radius: 6px; font-size: 14px; }
.checkin-form button { padding: 8px 24px; background: #1a73e8; color: #fff; border: none; border-radius: 6px; cursor: pointer; }
.radar-item { display: flex; align-items: center; gap: 12px; padding: 8px 0; }
.radar-item span { width: 100px; font-size: 13px; }
.bar { flex: 1; height: 8px; background: #eee; border-radius: 4px; overflow: hidden; }
.fill { height: 100%; background: #1a73e8; border-radius: 4px; }
.empty { color: #bbb; font-size: 13px; padding: 10px 0; }
.empty a { color: #1a73e8; }
.loading { text-align: center; color: #888; padding: 40px; }
</style>
