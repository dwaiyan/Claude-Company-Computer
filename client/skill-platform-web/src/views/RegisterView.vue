<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = useRouter(); const auth = useAuthStore();
const username = ref(''); const email = ref(''); const password = ref('');
const department = ref(''); const error = ref(''); const loading = ref(false);

async function handleRegister() {
  error.value = '';
  if (password.value.length < 6) { error.value = 'パスワードは6文字以上必要です'; return; }
  loading.value = true;
  try { await auth.register({ username: username.value, email: email.value, password: password.value, department: department.value || undefined }); router.push('/'); }
  catch (e: any) { error.value = e.response?.data?.message || '登録失敗'; }
  finally { loading.value = false; }
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-card">
      <h1>技術能力提升平台</h1>
      <h2>新規アカウント登録</h2>
      <form @submit.prevent="handleRegister">
        <div class="form-group"><label for="username">ユーザー名</label><input id="username" v-model="username" type="text" placeholder="名前" required /></div>
        <div class="form-group"><label for="email">メールアドレス</label><input id="email" v-model="email" type="email" placeholder="your@company.com" required /></div>
        <div class="form-group"><label for="password">パスワード</label><input id="password" v-model="password" type="password" placeholder="6文字以上" required /></div>
        <div class="form-group"><label for="department">部署 (任意)</label><input id="department" v-model="department" type="text" placeholder="例：バックエンド開発部" /></div>
        <p v-if="error" class="error-message">{{ error }}</p>
        <button type="submit" :disabled="loading" class="btn-primary">{{ loading ? '登録中...' : 'アカウント登録' }}</button>
      </form>
      <p class="auth-link">すでにアカウントをお持ちの方 <router-link to="/login">ログイン</router-link></p>
    </div>
  </div>
</template>
