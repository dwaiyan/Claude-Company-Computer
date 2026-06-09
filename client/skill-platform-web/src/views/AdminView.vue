<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth'; import http from '@/api/http';
import ThemeToggle from '@/components/ThemeToggle.vue';

const auth = useAuthStore(); const router = useRouter();
interface AssessmentItem { id: string; title: string; type: string; nodeId: string; timeLimit: number; passScore: number; questions: any[]; createdAt: string; }
interface ContribItem { id: string; userId: string; type: string; target: string; status: string; createdAt: string; }
const assessments = ref<AssessmentItem[]>([]); const contribs = ref<ContribItem[]>([]); const stats = ref({ total: 0, totalRecords: 0, avgScore: 0 });

onMounted(async () => {
  if (!auth.isAdmin) { router.push('/'); return; }
  const [a, c, s] = await Promise.all([http.get('/api/admin/assessments').then(r=>r.data), http.get('/api/contributions/pending').then(r=>r.data), http.get('/api/admin/assessments/stats').then(r=>r.data)]);
  assessments.value = a; contribs.value = c; stats.value = s;
});

async function reviewContrib(id: string, status: string) {
  await http.put(`/api/contributions/${id}/review`, { status, comment: '' });
  contribs.value = contribs.value.filter(c => c.id !== id);
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>管理后台</h1><nav class="nav-links"><router-link to="/">←</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div class="page-header"><h1>管理后台</h1></div>

      <div class="stats-row stagger-1">
        <div class="stat-card"><div class="stat-num">{{ stats.total }}</div><div class="stat-label">评测总数</div></div>
        <div class="stat-card"><div class="stat-num">{{ stats.totalRecords }}</div><div class="stat-label">提交记录</div></div>
        <div class="stat-card"><div class="stat-num">{{ stats.avgScore }}</div><div class="stat-label">平均分</div></div>
      </div>

      <div class="card stagger-2">
        <h2>待审核贡献</h2>
        <div v-for="c in contribs" :key="c.id" style="display:flex;justify-content:space-between;align-items:center;padding:12px 0;border-bottom:1px solid var(--border-subtle);font-size:13px;">
          <span style="font-family:var(--font-display);font-size:11px;letter-spacing:0.04em;color:var(--text-secondary);">{{ c.type }}: {{ c.target?.substring(0, 40) }}</span>
          <div style="display:flex;gap:8px;">
            <button @click="reviewContrib(c.id, 'approved')" style="padding:4px 14px;border:none;border-radius:var(--radius-sm);background:rgba(5,150,105,0.1);color:var(--color-accent);cursor:pointer;font-family:var(--font-display);font-size:11px;">通过</button>
            <button @click="reviewContrib(c.id, 'rejected')" style="padding:4px 14px;border:none;border-radius:var(--radius-sm);background:rgba(220,38,38,0.08);color:var(--color-danger);cursor:pointer;font-family:var(--font-display);font-size:11px;">拒绝</button>
          </div>
        </div>
        <p v-if="!contribs.length" class="empty">暂无待审核项</p>
      </div>
    </div>
  </div>
</template>
