<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRouter } from 'vue-router';
import { contentApi } from '@/api/content'; import type { TechTreeDto } from '@/api/content';
import ThemeToggle from '@/components/ThemeToggle.vue';

const router = useRouter(); const trees = ref<TechTreeDto[]>([]); const loading = ref(true);
onMounted(async () => { try { trees.value = await contentApi.getTechTrees(); } finally { loading.value = false; } });
function goToNode(nodeId: string) { router.push(`/tech-tree/${nodeId}`); }
</script>

<template>
  <div class="dashboard">
    <header class="app-header">
      <div class="header-left"><h1>技术图谱</h1><nav class="nav-links"><router-link to="/">←</router-link></nav></div>
      <div class="header-right"><ThemeToggle /></div>
    </header>
    <div class="page-container">
      <div class="page-header"><p>.NET 生态技术栈全景</p></div>
      <div v-if="loading" class="loading">加载中...</div>
      <div v-else class="tree-grid">
        <div v-for="tree in trees" :key="tree.id" class="tree-card" @click="goToNode(tree.id)">
          <div class="tree-icon">{{ tree.icon || '▣' }}</div>
          <h2>{{ tree.title }}</h2>
          <p>{{ tree.description }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.tree-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 20px; }
.tree-card { background: var(--bg-surface); border: 1px solid var(--border-default); border-radius: var(--radius-lg); padding: 32px; cursor: pointer; transition: all var(--transition-base); }
.tree-card:hover { border-color: var(--color-primary); box-shadow: var(--shadow-glow); transform: translateY(-4px); }
.tree-icon { font-size: 36px; margin-bottom: 14px; transition: transform var(--transition-base); }
.tree-card:hover .tree-icon { transform: scale(1.1) rotate(-5deg); }
.tree-card h2 { font-size: 17px; font-weight: 400; color: var(--text-primary); margin-bottom: 6px; }
.tree-card p { font-size: 13px; color: var(--text-muted); }
</style>
