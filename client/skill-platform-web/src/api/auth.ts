import http from './http';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  department?: string;
  title?: string;
}

export interface UserInfo {
  id: string;
  username: string;
  email: string;
  avatar: string | null;
  title: string | null;
  department: string | null;
  roles: string[];
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: UserInfo;
}

export const authApi = {
  login(data: LoginRequest): Promise<AuthResponse> {
    return http.post('/api/auth/login', data).then((res) => res.data);
  },

  register(data: RegisterRequest): Promise<AuthResponse> {
    return http.post('/api/auth/register', data).then((res) => res.data);
  },

  getCurrentUser(): Promise<UserInfo> {
    return http.get('/api/users/me').then((res) => res.data);
  },
};
