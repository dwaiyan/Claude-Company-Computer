<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRoute } from 'vue-router';
import { collabApi } from '@/api/collaboration'; import type { NoteDto, DiscussionDto } from '@/api/collaboration';
import ThemeToggle from '@/components/ThemeToggle.vue';

const route = useRoute(); const noteId = route.params.id as string;
const note = ref<NoteDto | null>(null); const discussions = ref<DiscussionDto[]>([]); const newComment = ref('');

onMounted(async () => { note.value = await collabApi.getNote(noteId); discussions.value = await collabApi.getDiscussions(note.value!.nodeId); });

async function addDiscussion() {
  if (!newComment.value.trim() || !note.value) return;
  await collabApi.addDiscussion({ content: newComment.value, nodeId: note.value.nodeId });
  discussions.value = await collabApi.getDiscussions(note.value!.nodeId); newComment.value = '';
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>笔记详情</h1><nav class="nav-links"><router-link to="/tech-tree">←</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div v-if="note">
        <div class="card" style="padding:32px;">
          <h1 style="font-size:22px;font-weight:400;margin-bottom:16px;">{{ note.title }}</h1>
          <div v-html="note.content" class="md-body" style="line-height:1.8;font-size:14px;color:var(--text-secondary);"></div>
          <p style="font-size:11px;color:var(--text-muted);margin-top:20px;">◉ {{ note.viewCount }} 次阅读 · ♡ {{ note.likeCount }}</p>
        </div>

        <div class="card" style="padding:24px;margin-top:20px;">
          <h2>讨论</h2>
          <div style="display:flex;gap:10px;margin-bottom:20px;">
            <textarea v-model="newComment" placeholder="发表讨论..." rows="3" style="flex:1;padding:10px;background:var(--bg-surface);border:1px solid var(--border-default);border-radius:var(--radius-md);font-family:var(--font-body);font-size:14px;color:var(--text-primary);resize:vertical;"></textarea>
            <button @click="addDiscussion" class="btn-primary" style="width:auto;align-self:flex-end;">发送</button>
          </div>
          <div v-for="d in discussions" :key="d.id" style="padding:12px 0;border-bottom:1px solid var(--border-subtle);">
            <p style="font-size:14px;color:var(--text-primary);">{{ d.content }}</p>
            <span style="font-size:11px;color:var(--text-muted);">{{ d.createdAt?.split('T')[0] }}</span>
            <div v-for="r in d.replies" :key="r.id" style="margin:8px 0 0 24px;padding:10px;border-left:2px solid var(--color-primary);font-size:13px;color:var(--text-secondary);">
              <p>{{ r.content }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
