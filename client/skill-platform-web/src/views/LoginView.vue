<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = useRouter();
const auth = useAuthStore();
const email = ref(''); const password = ref('');
const error = ref(''); const loading = ref(false);

async function handleLogin() {
  error.value = ''; loading.value = true;
  try { await auth.login({ email: email.value, password: password.value }); router.push('/'); }
  catch (e: any) { error.value = e.response?.data?.message || '登录失败'; }
  finally { loading.value = false; }
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-card">
      <h1>技术能力提升平台</h1>
      <h2>欢迎回来，请登录你的账号</h2>
      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email">邮箱</label>
          <input id="email" v-model="email" type="email" placeholder="your@company.com" required autocomplete="email" />
        </div>
        <div class="form-group">
          <label for="password">密码</label>
          <input id="password" v-model="password" type="password" placeholder="········" required autocomplete="current-password" />
        </div>
        <p v-if="error" class="error-message">{{ error }}</p>
        <button type="submit" :disabled="loading" class="btn-primary">
          {{ loading ? '验证中...' : '登录' }}
        </button>
      </form>
      <p class="auth-link">还没有账号？<router-link to="/register">创建账号</router-link></p>
    </div>
  </div>
</template>
