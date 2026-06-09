<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = useRouter(); const auth = useAuthStore();
const username = ref(''); const email = ref(''); const password = ref('');
const department = ref(''); const error = ref(''); const loading = ref(false);

async function handleRegister() {
  error.value = '';
  if (password.value.length < 6) { error.value = '密码至少6位'; return; }
  loading.value = true;
  try { await auth.register({ username: username.value, email: email.value, password: password.value, department: department.value || undefined }); router.push('/'); }
  catch (e: any) { error.value = e.response?.data?.message || '注册失败'; }
  finally { loading.value = false; }
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-card">
      <h1>Skill_Platform</h1>
      <h2>创建你的学习账号</h2>
      <form @submit.prevent="handleRegister">
        <div class="form-group"><label for="username">用户名</label><input id="username" v-model="username" type="text" placeholder="你的名字" required /></div>
        <div class="form-group"><label for="email">邮箱</label><input id="email" v-model="email" type="email" placeholder="your@company.com" required /></div>
        <div class="form-group"><label for="password">密码</label><input id="password" v-model="password" type="password" placeholder="至少6位" required /></div>
        <div class="form-group"><label for="department">部门 (选填)</label><input id="department" v-model="department" type="text" placeholder="如：后端开发部" /></div>
        <p v-if="error" class="error-message">{{ error }}</p>
        <button type="submit" :disabled="loading" class="btn-primary">{{ loading ? '注册中...' : '创建账号' }}</button>
      </form>
      <p class="auth-link">已有账号？<router-link to="/login">去登录</router-link></p>
    </div>
  </div>
</template>
