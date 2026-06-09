<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRoute } from 'vue-router';
import { collabApi } from '@/api/collaboration'; import type { NoteDto } from '@/api/collaboration';
import ThemeToggle from '@/components/ThemeToggle.vue';

const route = useRoute(); const nodeId = (route.query.node as string) || ''; const notes = ref<NoteDto[]>([]);
onMounted(async () => { if (nodeId) notes.value = await collabApi.getNotes(nodeId); });
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>团队笔记</h1><nav class="nav-links"><router-link to="/tech-tree">←</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div class="page-header"><p>技术节点下的团队笔记</p></div>
      <div v-if="!notes.length" class="empty">该技术节点暂无笔记，去 <router-link to="/tech-tree">技术图谱</router-link> 浏览</div>
      <div v-for="n in notes" :key="n.id" class="card" style="padding:20px 24px;">
        <router-link :to="`/notes/${n.id}`"><h3 style="font-size:16px;font-weight:400;color:var(--text-primary);">{{ n.title }}</h3></router-link>
        <p style="font-size:12px;color:var(--text-muted);margin-top:6px;">◉ {{ n.viewCount }} · ♡ {{ n.likeCount }} · {{ n.createdAt?.split('T')[0] }}</p>
      </div>
    </div>
  </div>
</template>
