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
      <div class="header-left"><h1>技術図譜</h1><nav class="nav-links"><router-link to="/">← 戻る</router-link></nav></div>
      <div class="header-right"><ThemeToggle /></div>
    </header>
    <div class="page-container">
      <div class="page-header"><p>.NET エコシステム技術スタック全景</p></div>
      <div v-if="loading" class="loading">読込中...</div>
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
.tree-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 1px; background: var(--border-subtle); border: 1px solid var(--border-subtle); border-radius: var(--radius-lg); overflow: hidden; }
.tree-card { background: var(--bg-surface); padding: 40px 32px; cursor: pointer; transition: background var(--transition-fast); }
.tree-card:hover { background: var(--bg-muted); }
.tree-icon { font-size: 32px; margin-bottom: 16px; transition: color var(--transition-fast); display: flex; align-items: center; height: 44px; }
.tree-card:hover .tree-icon { color: var(--color-accent); }
.tree-card h2 { font-size: 18px; font-weight: 400; color: var(--text-primary); margin-bottom: 6px; letter-spacing: 0.02em; }
.tree-card p { font-size: 13px; font-weight: 300; color: var(--text-muted); }
</style>
