<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { collabApi } from '@/api/collaboration';
import type { NoteDto, DiscussionDto } from '@/api/collaboration';

const route = useRoute();
const noteId = route.params.id as string;
const note = ref<NoteDto | null>(null);
const discussions = ref<DiscussionDto[]>([]);
const newComment = ref('');

onMounted(async () => {
  note.value = await collabApi.getNote(noteId);
  discussions.value = await collabApi.getDiscussions(note.value.nodeId);
});

async function addDiscussion() {
  if (!newComment.value.trim() || !note.value) return;
  await collabApi.addDiscussion({ content: newComment.value, nodeId: note.value.nodeId });
  discussions.value = await collabApi.getDiscussions(note.value!.nodeId);
  newComment.value = '';
}
</script>

<template>
  <div class="page-container">
    <router-link to="/tech-tree" class="back-link">← 返回</router-link>
    <div v-if="note">
      <article class="note-content">
        <h1>{{ note.title }}</h1>
        <div v-html="note.content" class="markdown-body"></div>
      </article>

      <section class="discussions">
        <h2>讨论</h2>
        <div class="comment-form">
          <textarea v-model="newComment" placeholder="发表讨论..." rows="3"></textarea>
          <button @click="addDiscussion" class="btn-primary-sm">发送</button>
        </div>
        <div v-for="d in discussions" :key="d.id" class="comment">
          <p>{{ d.content }}</p>
          <span class="time">{{ d.createdAt?.split('T')[0] }}</span>
          <div v-for="r in d.replies" :key="r.id" class="reply">
            <p>{{ r.content }}</p>
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<style scoped>
.page-container { max-width: 800px; margin: 0 auto; padding: 32px 24px; }
.back-link { color: #1a73e8; text-decoration: none; font-size: 14px; }
.note-content { background: #fff; border-radius: 10px; padding: 32px; margin: 16px 0; box-shadow: 0 1px 6px rgba(0,0,0,0.06); }
.note-content h1 { font-size: 22px; margin-bottom: 16px; }
.discussions { margin-top: 24px; }
.discussions h2 { font-size: 18px; margin-bottom: 16px; }
.comment-form { display: flex; gap: 10px; margin-bottom: 20px; }
.comment-form textarea { flex: 1; padding: 10px; border: 1px solid #d9d9d9; border-radius: 6px; font-size: 14px; resize: vertical; }
.btn-primary-sm { padding: 8px 20px; background: #1a73e8; color: #fff; border: none; border-radius: 6px; cursor: pointer; font-size: 13px; }
.comment { background: #fff; border-radius: 8px; padding: 14px 18px; margin-bottom: 10px; box-shadow: 0 1px 4px rgba(0,0,0,0.04); }
.comment p { font-size: 14px; }
.time { font-size: 11px; color: #aaa; }
.reply { margin: 8px 0 0 24px; padding: 10px; background: #f8f9fa; border-radius: 6px; font-size: 13px; }
</style>
