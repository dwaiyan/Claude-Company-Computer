<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { contentApi } from '@/api/content';
import type { TechTreeDto } from '@/api/content';

const router = useRouter();
const trees = ref<TechTreeDto[]>([]);
const loading = ref(true);

onMounted(async () => {
  try {
    trees.value = await contentApi.getTechTrees();
  } finally {
    loading.value = false;
  }
});

function goToNode(nodeId: string) {
  router.push(`/tech-tree/${nodeId}`);
}
</script>

<template>
  <div class="page-container">
    <header class="page-header">
      <h1>技术图谱</h1>
      <p>.NET 生态技术栈全景</p>
    </header>

    <div v-if="loading" class="loading">加载中...</div>

    <div v-else class="tree-grid">
      <div v-for="tree in trees" :key="tree.id" class="tree-card" @click="goToNode(tree.id)">
        <div class="tree-icon">{{ tree.icon || '📚' }}</div>
        <h2>{{ tree.title }}</h2>
        <p>{{ tree.description }}</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.page-container {
  max-width: 960px;
  margin: 0 auto;
  padding: 32px 24px;
}

.page-header {
  margin-bottom: 32px;
}

.page-header h1 {
  font-size: 24px;
  color: #1a73e8;
}

.page-header p {
  color: #888;
  margin-top: 4px;
}

.tree-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 20px;
}

.tree-card {
  background: #fff;
  border-radius: 12px;
  padding: 28px;
  cursor: pointer;
  box-shadow: 0 1px 6px rgba(0, 0, 0, 0.06);
  transition: all 0.2s;
}

.tree-card:hover {
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.12);
  transform: translateY(-2px);
}

.tree-icon {
  font-size: 32px;
  margin-bottom: 12px;
}

.tree-card h2 {
  font-size: 17px;
  margin-bottom: 6px;
  color: #333;
}

.tree-card p {
  font-size: 13px;
  color: #888;
}

.loading {
  text-align: center;
  color: #888;
  padding: 40px;
}
</style>
