'use client';

import Link from 'next/link';
import { useRouter, usePathname } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';
import { getMe } from '@/api/auth';
import { LayoutDashboard, Sparkles, History, LogOut } from 'lucide-react';

export function Sidebar() {
  const router = useRouter();
  const pathname = usePathname();

  const { data } = useQuery({
    queryKey: ['me'],
    queryFn: getMe,
  });

  const logout = () => {
    localStorage.removeItem('token');
    router.replace('/login');
  };

  const linkClass = (path: string) =>
    `flex items-center gap-2 px-3 py-2 rounded-lg transition ${
      pathname === path ? 'bg-muted font-medium' : 'hover:bg-muted'
    }`;

  return (
    <aside className='w-64 border-r h-screen sticky top-0 p-4 flex flex-col border-gray-200 bg-gray-50'>
      <div>
        {/* App info */}
        <div className='mb-6'>
          <h2 className='text-lg font-semibold'>LeadForge</h2>
          <p className='text-xs text-muted-foreground'>
            AI LinkedIn Post Generator
          </p>
        </div>

        {/* User info */}
        <div className='mb-6 space-y-1'>
          <p className='text-sm font-medium truncate'>
            {data?.email ?? 'Loading...'}
          </p>

          <p className='text-xs text-muted-foreground'>
            Credits: {data?.credits ?? '-'}
          </p>
        </div>

        {/* Navigation */}
        <nav className='space-y-2'>
          <Link className={linkClass('/dashboard')} href='/dashboard'>
            <LayoutDashboard className='h-4 w-4' />
            Dashboard
          </Link>

          <Link className={linkClass('/generate')} href='/generate'>
            <Sparkles className='h-4 w-4' />
            Generate
          </Link>

          <Link className={linkClass('/history')} href='/history'>
            <History className='h-4 w-4' />
            History
          </Link>
        </nav>
      </div>

      <button
        onClick={logout}
        className='mt-auto flex items-center gap-2 text-sm cursor-pointer text-muted-foreground hover:text-foreground transition'
      >
        <LogOut className='h-4 w-4' />
        Logout
      </button>
    </aside>
  );
}
