<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRoute } from 'vue-router';
import { contentApi } from '@/api/content'; import { learningApi } from '@/api/learning';
import type { TechNodeDto, ResourceDto, InterviewQuestionDto } from '@/api/content';
import type { AssessmentDto } from '@/api/learning';
import ThemeToggle from '@/components/ThemeToggle.vue';

const route = useRoute(); const nodeId = route.params.nodeId as string;
const node = ref<TechNodeDto | null>(null); const resources = ref<ResourceDto[]>([]);
const questions = ref<InterviewQuestionDto[]>([]); const assessments = ref<AssessmentDto[]>([]);
const activeTab = ref<'resources' | 'questions' | 'assessments'>('resources');

onMounted(async () => {
  const [n, r, q] = await Promise.all([contentApi.getNode(nodeId), contentApi.getResources(nodeId), contentApi.getQuestions(nodeId)]);
  node.value = n; resources.value = r; questions.value = q;
  try { assessments.value = await learningApi.getAssessments(nodeId); } catch { /* */ }
});
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>节点详情</h1><nav class="nav-links"><router-link to="/tech-tree">←</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div v-if="node">
        <h1 style="font-size:24px;margin-bottom:6px;">{{ node.title }}</h1>
        <p style="color:var(--text-muted);font-size:13px;margin-bottom:4px;">难度: {{ '▮'.repeat(node.level) }} · {{ node.resourceCount }} 资源 · {{ node.questionCount }} 面试题</p>
        <p style="color:var(--text-secondary);margin-bottom:28px;">{{ node.description }}</p>

        <div class="tabs">
          <button :class="{ active: activeTab === 'resources' }" @click="activeTab = 'resources'">资源</button>
          <button :class="{ active: activeTab === 'questions' }" @click="activeTab = 'questions'">面试题</button>
          <button :class="{ active: activeTab === 'assessments' }" @click="activeTab = 'assessments'">评测</button>
        </div>

        <div v-if="activeTab === 'resources'">
          <div v-for="r in resources" :key="r.id" class="card" style="padding:16px 20px;">
            <span class="type-badge">{{ r.type }}</span>
            <a :href="r.url" target="_blank">{{ r.title }}</a>
            <span style="font-size:12px;color:var(--text-muted);">难度: {{ '▮'.repeat(r.difficulty) }}</span>
          </div>
          <p v-if="!resources.length" class="empty">暂无学习资源</p>
        </div>

        <div v-if="activeTab === 'questions'">
          <div v-for="q in questions" :key="q.id" class="card" style="padding:16px 20px;">
            <div style="font-weight:500;color:var(--text-primary);margin-bottom:6px;">{{ q.question }}</div>
            <div style="font-size:13px;color:var(--text-muted);">▸ {{ q.answerTip }}</div>
          </div>
          <p v-if="!questions.length" class="empty">暂无面试题</p>
        </div>

        <div v-if="activeTab === 'assessments'">
          <div v-for="a in assessments" :key="a.id" class="card" style="padding:16px 20px;">
            <h3 style="font-size:15px;font-weight:400;">{{ a.title }}</h3>
            <p style="font-size:12px;color:var(--text-muted);">{{ a.type }} · {{ a.timeLimit }}分钟 · 及格: {{ a.passScore }}分</p>
          </div>
          <p v-if="!assessments.length" class="empty">暂无评测</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.type-badge { display: inline-block; padding: 2px 8px; background: var(--color-primary-glow); color: var(--color-primary); border-radius: var(--radius-sm); font-family: var(--font-display); font-size: 10px; letter-spacing: 0.05em; text-transform: uppercase; margin-right: 12px; }
</style>
