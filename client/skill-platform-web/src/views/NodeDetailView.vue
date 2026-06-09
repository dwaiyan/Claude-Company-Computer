<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { contentApi } from '@/api/content';
import { learningApi } from '@/api/learning';
import type { TechNodeDto, ResourceDto, InterviewQuestionDto } from '@/api/content';
import type { AssessmentDto } from '@/api/learning';

const route = useRoute();
const nodeId = route.params.nodeId as string;
const node = ref<TechNodeDto | null>(null);
const resources = ref<ResourceDto[]>([]);
const questions = ref<InterviewQuestionDto[]>([]);
const assessments = ref<AssessmentDto[]>([]);
const activeTab = ref<'resources' | 'questions' | 'assessments'>('resources');

onMounted(async () => {
  const [n, r, q] = await Promise.all([
    contentApi.getNode(nodeId),
    contentApi.getResources(nodeId),
    contentApi.getQuestions(nodeId),
  ]);
  node.value = n;
  resources.value = r;
  questions.value = q;

  try {
    assessments.value = await learningApi.getAssessments(nodeId);
  } catch { /* no assessments yet */ }
});
</script>

<template>
  <div class="page-container">
    <div v-if="node" class="node-detail">
      <router-link to="/tech-tree" class="back-link">← 返回技术图谱</router-link>

      <h1>{{ node.title }}</h1>
      <p class="desc">{{ node.description }}</p>

      <div class="meta">难度等级: {{ '⭐'.repeat(node.level) }} · {{ node.resourceCount }} 资源 · {{ node.questionCount }} 面试题</div>

      <div class="tabs">
        <button :class="{ active: activeTab === 'resources' }" @click="activeTab = 'resources'">学习资源</button>
        <button :class="{ active: activeTab === 'questions' }" @click="activeTab = 'questions'">面试题</button>
        <button :class="{ active: activeTab === 'assessments' }" @click="activeTab = 'assessments'">评测</button>
      </div>

      <div v-if="activeTab === 'resources'" class="tab-content">
        <div v-for="r in resources" :key="r.id" class="resource-item">
          <span class="type-badge">{{ r.type }}</span>
          <a :href="r.url" target="_blank">{{ r.title }}</a>
          <span class="diff">难度: {{ '⭐'.repeat(r.difficulty) }}</span>
        </div>
        <p v-if="!resources.length" class="empty">暂无学习资源</p>
      </div>

      <div v-if="activeTab === 'questions'" class="tab-content">
        <div v-for="q in questions" :key="q.id" class="question-item">
          <div class="q-title">{{ q.question }}</div>
          <div class="q-tip">💡 {{ q.answerTip }}</div>
        </div>
        <p v-if="!questions.length" class="empty">暂无面试题</p>
      </div>

      <div v-if="activeTab === 'assessments'" class="tab-content">
        <div v-for="a in assessments" :key="a.id" class="assessment-item">
          <h3>{{ a.title }}</h3>
          <p>类型: {{ a.type }} · 时间: {{ a.timeLimit }}分钟 · 及格: {{ a.passScore }}分</p>
        </div>
        <p v-if="!assessments.length" class="empty">暂无评测</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.page-container { max-width: 800px; margin: 0 auto; padding: 32px 24px; }
.back-link { color: #1a73e8; text-decoration: none; font-size: 14px; }
.back-link:hover { text-decoration: underline; }
.node-detail h1 { font-size: 24px; margin: 16px 0 8px; }
.desc { color: #666; margin-bottom: 8px; }
.meta { font-size: 13px; color: #888; margin-bottom: 24px; }
.tabs { display: flex; gap: 0; border-bottom: 2px solid #eee; margin-bottom: 20px; }
.tabs button { padding: 10px 20px; border: none; background: none; cursor: pointer; font-size: 14px; color: #666; border-bottom: 2px solid transparent; margin-bottom: -2px; }
.tabs button.active { color: #1a73e8; border-bottom-color: #1a73e8; }
.tab-content { background: #fff; border-radius: 8px; padding: 20px; }
.resource-item, .question-item, .assessment-item { padding: 14px 0; border-bottom: 1px solid #f0f0f0; }
.resource-item:last-child, .question-item:last-child { border-bottom: none; }
.type-badge { display: inline-block; padding: 2px 8px; background: #e8f0fe; color: #1a73e8; border-radius: 4px; font-size: 11px; margin-right: 10px; }
.resource-item a { color: #333; text-decoration: none; }
.resource-item a:hover { color: #1a73e8; }
.diff { margin-left: 10px; font-size: 12px; color: #888; }
.q-title { font-weight: 500; margin-bottom: 6px; }
.q-tip { font-size: 13px; color: #888; }
.empty { color: #bbb; text-align: center; padding: 20px; }
</style>
