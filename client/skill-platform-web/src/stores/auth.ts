import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { authApi } from '@/api/auth';
import type { UserInfo, LoginRequest, RegisterRequest } from '@/api/auth';

export const useAuthStore = defineStore('auth', () => {
  const user = ref<UserInfo | null>(
    JSON.parse(localStorage.getItem('user') || 'null')
  );
  const accessToken = ref<string | null>(localStorage.getItem('accessToken'));
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'));

  const isLoggedIn = computed(() => !!accessToken.value);
  const isAdmin = computed(() => user.value?.roles?.includes('admin') ?? false);

  async function login(data: LoginRequest) {
    const response = await authApi.login(data);
    setSession(response);
  }

  async function register(data: RegisterRequest) {
    const response = await authApi.register(data);
    setSession(response);
  }

  function setSession(response: { accessToken: string; refreshToken: string; user: UserInfo }) {
    accessToken.value = response.accessToken;
    refreshToken.value = response.refreshToken;
    user.value = response.user;

    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('user', JSON.stringify(response.user));
  }

  function logout() {
    accessToken.value = null;
    refreshToken.value = null;
    user.value = null;

    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
  }

  return { user, accessToken, isLoggedIn, isAdmin, login, register, logout };
});
