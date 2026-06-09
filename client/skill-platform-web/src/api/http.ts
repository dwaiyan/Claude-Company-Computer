import axios from 'axios';
import type { AxiosInstance, AxiosError } from 'axios';

const baseURL = 'http://localhost:8080';

const http: AxiosInstance = axios.create({
  baseURL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

http.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

http.interceptors.response.use(
  (response) => response,
  (error: AxiosError<{ code?: string; message?: string }>) => {
    const message = error.response?.data?.message || error.message || '请求失败';
    console.error(`[API Error] ${message}`);

    if (error.response?.status === 401) {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }

    return Promise.reject(error);
  }
);

export default http;
