<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { useRoute } from 'vue-router';
import { collabApi } from '@/api/collaboration'; import type { NoteDto, DiscussionDto } from '@/api/collaboration';
import { useMarkdown } from '@/composables/useMarkdown';
import ThemeToggle from '@/components/ThemeToggle.vue';

const route = useRoute(); const noteId = route.params.id as string;
const note = ref<NoteDto | null>(null); const discussions = ref<DiscussionDto[]>([]);
const newComment = ref(''); const { render } = useMarkdown();
const renderedContent = ref('');

onMounted(async () => {
  note.value = await collabApi.getNote(noteId);
  renderedContent.value = render(note.value!.content);
  discussions.value = await collabApi.getDiscussions(note.value!.nodeId);
});

async function addDiscussion() {
  if (!newComment.value.trim() || !note.value) return;
  await collabApi.addDiscussion({ content: newComment.value, nodeId: note.value.nodeId });
  discussions.value = await collabApi.getDiscussions(note.value!.nodeId); newComment.value = '';
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>笔记详情</h1><nav class="nav-links"><router-link to="/notes">← 返回</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div v-if="note">
        <article class="card" style="padding:40px;">
          <h1 style="font-size:26px;font-weight:400;margin-bottom:8px;color:var(--text-primary);">{{ note.title }}</h1>
          <p style="font-size:12px;color:var(--text-muted);margin-bottom:28px;">
            {{ note.createdAt?.split('T')[0] }} · ◉ {{ note.viewCount }} 阅读 · ♡ {{ note.likeCount }}
          </p>
          <div class="md-body" v-html="renderedContent"></div>
        </article>

        <div class="card" style="padding:28px;margin-top:24px;">
          <h2 style="font-size:18px;font-weight:400;margin-bottom:20px;">讨论 ({{ discussions.length }})</h2>
          <div style="display:flex;gap:10px;margin-bottom:24px;">
            <textarea v-model="newComment" placeholder="发表你的看法..." rows="3" style="flex:1;padding:10px;background:var(--bg-surface);border:1px solid var(--border-default);border-radius:var(--radius-md);font-family:var(--font-body);font-size:14px;color:var(--text-primary);resize:vertical;"></textarea>
            <button @click="addDiscussion" class="btn-primary" style="width:auto;align-self:flex-end;">发送</button>
          </div>
          <div v-if="!discussions.length" class="empty">暂无讨论，来发表第一条吧</div>
          <div v-for="d in discussions" :key="d.id" style="padding:16px 0;border-bottom:1px solid var(--border-subtle);">
            <div style="display:flex;align-items:center;gap:8px;margin-bottom:6px;">
              <div style="width:24px;height:24px;border-radius:50%;background:var(--bg-muted);display:flex;align-items:center;justify-content:center;font-size:11px;color:var(--text-muted);">U</div>
              <span style="font-size:12px;color:var(--text-secondary);">{{ d.userId }}</span>
              <span style="font-size:11px;color:var(--text-muted);">{{ d.createdAt?.split('T')[0] }}</span>
            </div>
            <p style="font-size:14px;color:var(--text-primary);margin-bottom:4px;">{{ d.content }}</p>
            <div v-for="r in d.replies" :key="r.id" style="margin:8px 0 0 28px;padding:10px 14px;border-left:2px solid var(--color-accent);font-size:13px;color:var(--text-secondary);background:var(--bg-muted);border-radius:0 var(--radius-sm) var(--radius-sm) 0;">
              <div style="display:flex;align-items:center;gap:6px;margin-bottom:4px;">
                <span style="font-size:11px;color:var(--text-muted);">{{ r.userId }}</span>
                <span style="font-size:10px;color:var(--text-muted);">{{ r.createdAt?.split('T')[0] }}</span>
              </div>
              {{ r.content }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.md-body :deep(h1) { font-size: 22px; font-weight: 400; margin: 28px 0 12px; color: var(--text-primary); }
.md-body :deep(h2) { font-size: 18px; font-weight: 400; margin: 24px 0 10px; color: var(--text-primary); border-bottom: 1px solid var(--border-subtle); padding-bottom: 6px; }
.md-body :deep(h3) { font-size: 16px; font-weight: 400; margin: 20px 0 8px; color: var(--text-primary); }
.md-body :deep(p) { margin: 10px 0; line-height: 1.9; color: var(--text-secondary); }
.md-body :deep(ul), .md-body :deep(ol) { margin: 10px 0; padding-left: 24px; color: var(--text-secondary); }
.md-body :deep(li) { margin: 4px 0; line-height: 1.8; }
.md-body :deep(code) { background: var(--bg-muted); padding: 2px 6px; border-radius: var(--radius-sm); font-family: var(--font-mono); font-size: 13px; color: var(--color-accent); }
.md-body :deep(pre) { background: #0d1117; color: #c8d6e5; padding: 16px 20px; border-radius: var(--radius-md); overflow-x: auto; margin: 14px 0; font-size: 13px; line-height: 1.6; }
.md-body :deep(pre code) { background: none; padding: 0; color: inherit; }
.md-body :deep(blockquote) { border-left: 3px solid var(--color-accent); margin: 14px 0; padding: 4px 16px; color: var(--text-muted); font-style: italic; }
.md-body :deep(a) { color: var(--color-accent); text-decoration: underline; }
.md-body :deep(strong) { color: var(--text-primary); font-weight: 600; }
.md-body :deep(hr) { border: none; border-top: 1px solid var(--border-subtle); margin: 28px 0; }
</style>
