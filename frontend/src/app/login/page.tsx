'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { login } from '@/api/auth';
import { AuthGuard } from '@/components/auth/AuthGuard';
import { handleApiError } from '@/lib/handleApiError';
import { Sparkles } from 'lucide-react';
import Link from 'next/link';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      setLoading(true);

      const data = await login({
        email,
        password,
      });

      localStorage.setItem('token', data.accessToken);
      window.location.href = '/dashboard';
    } catch (e) {
      handleApiError(e);
    } finally {
      setLoading(false);
    }
  };

  const handleDemoLogin = async () => {
    try {
      setLoading(true);

      const data = await login({
        email: 'example@example.com',
        password: 'example123',
      });

      localStorage.setItem('token', data.accessToken);
      window.location.href = '/dashboard';
    } catch (e) {
      handleApiError(e);
    } finally {
      setLoading(false);
    }
  };

  return (
    <AuthGuard requireAuth={false}>
      <div className='relative flex min-h-screen items-center justify-center bg-cover bg-center'>
        {/* Background overlay */}
        <div className='absolute inset-0 bg-black/40' />

        {/* Card */}
        <Card className='relative w-[400px] bg-white/90 backdrop-blur shadow-xl border'>
          <CardHeader className='space-y-3 text-center'>
            <div className='flex items-center justify-center gap-2 text-lg font-semibold'>
              <Sparkles className='h-5 w-5 text-primary' />
              LeadForge
            </div>

            <CardTitle className='text-xl'>Sign in to your account</CardTitle>

            <p className='text-sm text-muted-foreground'>
              Generate high-performing LinkedIn posts with AI
            </p>
          </CardHeader>

          <CardContent>
            {/* Login form */}
            <form onSubmit={handleLogin} className='space-y-4'>
              <Input
                placeholder='Email'
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />

              <Input
                type='password'
                placeholder='Password'
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />

              <Button type='submit' className='w-full' disabled={loading}>
                {loading ? 'Signing in...' : 'Sign in'}
              </Button>
            </form>

            {/* Demo login */}
            <div className='mt-4'>
              <Button
                type='button'
                variant='outline'
                className='w-full'
                onClick={handleDemoLogin}
                disabled={loading}
              >
                Explore Demo
              </Button>

              <p className='text-xs text-center text-muted-foreground mt-2'>
                Recruiter? Instantly explore the app using the demo account.
              </p>
            </div>

            {/* Register */}
            <p className='text-sm text-center text-muted-foreground mt-6'>
              Don’t have an account?{' '}
              <Link href='/register' className='underline'>
                Register
              </Link>
            </p>
          </CardContent>
        </Card>
      </div>
    </AuthGuard>
  );
}
