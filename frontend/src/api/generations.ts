import { toast } from 'sonner';
import { api } from './client';
import { GeneratePostRequest, GeneratePostResponse } from '@/types/api';

export const generatePost = async (
  data: GeneratePostRequest,
): Promise<GeneratePostResponse> => {
  const res = await api.post<GeneratePostResponse>('/generation', data);
  return res.data;
};

export const getGenerations = async (page: number, pageSize: number = 10) => {
  const res = await api.get('/generation', {
    params: { page, pageSize },
  });

  return res.data;
};

export const deleteGeneration = async (id: string) => {
  await api.delete(`/generation/${id}`);
  toast.success('Generation deleted');
};
