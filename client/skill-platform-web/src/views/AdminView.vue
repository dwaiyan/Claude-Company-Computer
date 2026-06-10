<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth'; import http from '@/api/http';
import ThemeToggle from '@/components/ThemeToggle.vue';

const auth = useAuthStore(); const router = useRouter();
const MOCK = (window as any).__MOCK_MODE__;

interface AssessmentItem { id: string; title: string; type: string; nodeId: string; timeLimit: number; passScore: number; questions: any[]; createdAt: string; }
interface ContribItem { id: string; userId: string; type: string; target: string; status: string; createdAt: string; }
const assessments = ref<AssessmentItem[]>([]); const contribs = ref<ContribItem[]>([]); const stats = ref({ total: 0, totalRecords: 0, avgScore: 0 });

const mockAssessments: AssessmentItem[] = [
  { id: 'as-1', title: 'ASP.NET Core 基础评测', type: '选择', nodeId: 'node-1', timeLimit: 30, passScore: 70, questions: [], createdAt: '2026-06-01' },
  { id: 'as-2', title: '微服务架构设计评测', type: '设计', nodeId: 'node-3', timeLimit: 60, passScore: 75, questions: [], createdAt: '2026-06-03' },
];
const mockContribs: ContribItem[] = [
  { id: 'c-1', userId: 'u-002', type: '资源', target: 'Docker Compose 实战部署指南', status: 'pending', createdAt: '2026-06-09' },
  { id: 'c-2', userId: 'u-003', type: '笔记', target: 'Polly 弹性策略深度解析', status: 'pending', createdAt: '2026-06-08' },
];
const mockStats = { total: 15, totalRecords: 342, avgScore: 78 };

onMounted(async () => {
  if (!auth.isAdmin) { router.push('/'); return; }
  if (MOCK) { assessments.value = mockAssessments; contribs.value = mockContribs; stats.value = mockStats; return; }
  const [a, c, s] = await Promise.all([http.get('/api/admin/assessments').then(r=>r.data), http.get('/api/contributions/pending').then(r=>r.data), http.get('/api/admin/assessments/stats').then(r=>r.data)]);
  assessments.value = a; contribs.value = c; stats.value = s;
});

async function reviewContrib(id: string, status: string) {
  if (MOCK) { contribs.value = contribs.value.filter(c => c.id !== id); return; }
  await http.put(`/api/contributions/${id}/review`, { status, comment: '' });
  contribs.value = contribs.value.filter(c => c.id !== id);
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>管理后台</h1><nav class="nav-links"><router-link to="/">← 返回</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div class="page-header"><h1>管理后台</h1></div>

      <div class="stats-row stagger-1">
        <div class="stat-card"><div class="stat-num">{{ stats.total }}</div><div class="stat-label">评测总数</div></div>
        <div class="stat-card"><div class="stat-num">{{ stats.totalRecords }}</div><div class="stat-label">提交记录</div></div>
        <div class="stat-card"><div class="stat-num">{{ stats.avgScore }}</div><div class="stat-label">平均分</div></div>
      </div>

      <div class="card stagger-2">
        <h2>待审核贡献</h2>
        <div v-for="c in contribs" :key="c.id" style="display:flex;justify-content:space-between;align-items:center;padding:14px 0;border-bottom:1px solid var(--border-subtle);font-size:13px;">
          <span style="font-family:var(--font-body);font-size:12px;font-weight:300;letter-spacing:0.03em;color:var(--text-secondary);">{{ c.type }}: {{ c.target?.substring(0, 40) }}</span>
          <div style="display:flex;gap:8px;">
            <button @click="reviewContrib(c.id, 'approved')" style="padding:4px 14px;border:1px solid var(--color-accent);border-radius:var(--radius-sm);background:transparent;color:var(--color-accent);cursor:pointer;font-family:var(--font-body);font-size:11px;font-weight:400;">通过</button>
            <button @click="reviewContrib(c.id, 'rejected')" style="padding:4px 14px;border:1px solid var(--color-danger);border-radius:var(--radius-sm);background:transparent;color:var(--color-danger);cursor:pointer;font-family:var(--font-body);font-size:11px;font-weight:400;">拒绝</button>
          </div>
        </div>
        <p v-if="!contribs.length" class="empty">暂无待审核项</p>
      </div>
    </div>
  </div>
</template>
