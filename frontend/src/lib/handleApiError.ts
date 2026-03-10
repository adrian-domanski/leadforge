import axios from 'axios';
import { toast } from 'sonner';

interface ApiError {
  message?: string;
  errors?: Record<string, string[]>;
}

export function handleApiError(error: unknown) {
  if (axios.isAxiosError<ApiError>(error)) {
    const data = error.response?.data;

    // FluentValidation errors
    if (data?.errors) {
      const firstError = Object.values(data.errors).flat()[0];

      toast.error(firstError ?? 'Validation error');
      return;
    }

    // Domain / custom API error
    if (data?.message) {
      toast.error(data.message);
      return;
    }

    toast.error('Request failed');
    return;
  }

  if (error instanceof Error) {
    toast.error(error.message);
    return;
  }

  toast.error('Unexpected error');
}
