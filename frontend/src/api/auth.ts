import { api } from './client';
import {
  LoginRequest,
  AuthResponse,
  RegisterRequest,
  MeResponse,
} from '@/types/api';

export const login = async (data: LoginRequest) => {
  const res = await api.post<AuthResponse>('/auth/login', data);
  return res.data;
};

export const register = async (
  data: RegisterRequest,
): Promise<AuthResponse> => {
  const res = await api.post<AuthResponse>('/auth/register', data);
  return res.data;
};

export const getMe = async (): Promise<MeResponse> => {
  const res = await api.get('/auth/me');
  return res.data;
};
