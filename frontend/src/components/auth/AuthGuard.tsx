'use client';

import { useRouter } from 'next/navigation';
import { useEffect } from 'react';

interface Props {
  children: React.ReactNode;
  requireAuth?: boolean;
}

export function AuthGuard({ children, requireAuth = true }: Props) {
  const router = useRouter();

  useEffect(() => {
    const token = localStorage.getItem('token');

    if (requireAuth && !token) {
      router.replace('/login');
      return;
    }

    if (!requireAuth && token) {
      router.replace('/dashboard');
    }
  }, [requireAuth, router]);

  return <>{children}</>;
}
