import http from './http';
import { mockAuthResponse } from '@/mocks/data';

export interface LoginRequest { email: string; password: string; }
export interface RegisterRequest { username: string; email: string; password: string; department?: string; title?: string; }
export interface UserInfo { id: string; username: string; email: string; avatar: string | null; title: string | null; department: string | null; roles: string[]; }
export interface AuthResponse { accessToken: string; refreshToken: string; expiresIn: number; user: UserInfo; }

const MOCK = (window as any).__MOCK_MODE__;

function delay<T>(data: T): Promise<T> {
  return new Promise(r => setTimeout(() => r(data), 80 + Math.random() * 120));
}

export const authApi = {
  login(_data: LoginRequest): Promise<AuthResponse> {
    if (MOCK) return delay(mockAuthResponse);
    return http.post('/api/auth/login', _data).then((res) => res.data);
  },
  register(_data: RegisterRequest): Promise<AuthResponse> {
    if (MOCK) return delay(mockAuthResponse);
    return http.post('/api/auth/register', _data).then((res) => res.data);
  },
  getCurrentUser(): Promise<UserInfo> {
    if (MOCK) return delay(mockAuthResponse.user);
    return http.get('/api/users/me').then((res) => res.data);
  },
};
