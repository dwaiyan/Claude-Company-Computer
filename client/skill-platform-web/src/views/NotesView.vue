<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { collabApi } from '@/api/collaboration';
import type { NoteDto } from '@/api/collaboration';

const route = useRoute();
const nodeId = (route.query.node as string) || '';
const notes = ref<NoteDto[]>([]);

onMounted(async () => {
  if (nodeId) {
    notes.value = await collabApi.getNotes(nodeId);
  }
});
</script>

<template>
  <div class="page-container">
    <header class="page-header"><h1>团队笔记</h1></header>
    <div v-if="!notes.length" class="empty">该技术节点暂无笔记</div>
    <div v-for="n in notes" :key="n.id" class="note-card">
      <router-link :to="`/notes/${n.id}`"><h3>{{ n.title }}</h3></router-link>
      <p class="meta">👁 {{ n.viewCount }} · ❤️ {{ n.likeCount }} · {{ n.createdAt?.split('T')[0] }}</p>
    </div>
  </div>
</template>

<style scoped>
.page-container { max-width: 800px; margin: 0 auto; padding: 32px 24px; }
.page-header h1 { font-size: 24px; color: #1a73e8; }
.note-card { background: #fff; border-radius: 8px; padding: 16px 20px; margin-bottom: 12px; box-shadow: 0 1px 4px rgba(0,0,0,0.06); }
.note-card a { text-decoration: none; color: #333; }
.note-card a:hover { color: #1a73e8; }
.note-card h3 { font-size: 16px; margin-bottom: 6px; }
.meta { font-size: 12px; color: #888; }
.empty { text-align: center; color: #bbb; padding: 40px; }
</style>
