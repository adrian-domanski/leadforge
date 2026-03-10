'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { Sidebar } from './sidebar';

export function AppLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter();

  useEffect(() => {
    const token = localStorage.getItem('token');

    if (!token) {
      router.push('/login');
    }
  }, []);

  return (
    <div className='flex'>
      <Sidebar />
      <main className='flex-1 p-8 max-w-5xl mx-auto'>{children}</main>
    </div>
  );
}
