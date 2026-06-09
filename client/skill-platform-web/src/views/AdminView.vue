<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';
import http from '@/api/http';

const auth = useAuthStore();
const router = useRouter();

interface AssessmentItem { id: string; title: string; type: string; nodeId: string; timeLimit: number; passScore: number; questions: any[]; createdAt: string; }
interface ContributionItem { id: string; userId: string; type: string; target: string; status: string; createdAt: string; }

const assessments = ref<AssessmentItem[]>([]);
const contributions = ref<ContributionItem[]>([]);
const stats = ref({ total: 0, totalRecords: 0, avgScore: 0 });

onMounted(async () => {
  if (!auth.isAdmin) { router.push('/'); return; }
  const [a, c, s] = await Promise.all([
    http.get('/api/admin/assessments').then(r => r.data),
    http.get('/api/contributions/pending').then(r => r.data),
    http.get('/api/admin/assessments/stats').then(r => r.data),
  ]);
  assessments.value = a; contributions.value = c; stats.value = s;
});

async function reviewContrib(id: string, status: string) {
  await http.put(`/api/contributions/${id}/review`, { status, comment: '' });
  contributions.value = contributions.value.filter(c => c.id !== id);
}
</script>

<template>
  <div class="page-container">
    <header class="page-header"><h1>管理后台</h1></header>

    <div class="stats-row">
      <div class="stat-card"><div class="n">{{ stats.total }}</div><div>评测</div></div>
      <div class="stat-card"><div class="n">{{ stats.totalRecords }}</div><div>记录</div></div>
      <div class="stat-card"><div class="n">{{ stats.avgScore }}</div><div>均分</div></div>
    </div>

    <section class="card">
      <h2>待审核贡献</h2>
      <div v-for="c in contributions" :key="c.id" class="item-row">
        <span>{{ c.type }}: {{ c.target?.substring(0, 50) }}</span>
        <div class="actions">
          <button @click="reviewContrib(c.id, 'approved')" class="btn-sm btn-approve">通过</button>
          <button @click="reviewContrib(c.id, 'rejected')" class="btn-sm btn-reject">拒绝</button>
        </div>
      </div>
      <p v-if="!contributions.length" class="empty">暂无待审核项</p>
    </section>
  </div>
</template>

<style scoped>
.page-container { max-width: 960px; margin: 0 auto; padding: 32px 24px; }
.page-header h1 { font-size: 24px; color: #1a73e8; }
.stats-row { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; margin: 20px 0; }
.stat-card { background: #fff; border-radius: 10px; padding: 20px; text-align: center; box-shadow: 0 1px 6px rgba(0,0,0,0.06); }
.stat-card .n { font-size: 28px; font-weight: 600; color: #1a73e8; }
.card { background: #fff; border-radius: 10px; padding: 24px; margin-bottom: 20px; box-shadow: 0 1px 6px rgba(0,0,0,0.06); }
.card h2 { font-size: 16px; margin-bottom: 16px; }
.item-row { display: flex; justify-content: space-between; align-items: center; padding: 10px 0; border-bottom: 1px solid #f0f0f0; font-size: 14px; }
.actions { display: flex; gap: 8px; }
.btn-sm { padding: 4px 14px; border: none; border-radius: 4px; cursor: pointer; font-size: 12px; }
.btn-approve { background: #e6f4ea; color: #1e8e3e; }
.btn-reject { background: #fce8e6; color: #d93025; }
.empty { color: #bbb; font-size: 13px; padding: 10px 0; }
</style>
