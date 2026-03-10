'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { register } from '@/api/auth';
import { AuthGuard } from '@/components/auth/AuthGuard';
import { handleApiError } from '@/lib/handleApiError';
import { Sparkles } from 'lucide-react';
import Link from 'next/link';

export default function RegisterPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      setLoading(true);

      const data = await register({
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

  return (
    <AuthGuard requireAuth={false}>
      <div
        className='relative flex min-h-screen items-center justify-center bg-cover bg-center'
        style={{ backgroundImage: "url('/auth-bg.jpg')" }}
      >
        <div className='absolute inset-0 bg-black/40' />

        <Card className='relative w-[400px] bg-white/90 backdrop-blur shadow-xl border'>
          <CardHeader className='space-y-3 text-center'>
            <div className='flex items-center justify-center gap-2 text-lg font-semibold'>
              <Sparkles className='h-5 w-5 text-primary' />
              LeadForge
            </div>

            <CardTitle className='text-xl'>Create an account</CardTitle>

            <p className='text-sm text-muted-foreground'>
              Start generating LinkedIn posts with AI
            </p>
          </CardHeader>

          <CardContent>
            <form onSubmit={handleRegister} className='space-y-4'>
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
                {loading ? 'Creating account...' : 'Register'}
              </Button>
            </form>

            <p className='text-sm text-center text-muted-foreground mt-4'>
              Already have an account?{' '}
              <Link href='/login' className='underline'>
                Login
              </Link>
            </p>
          </CardContent>
        </Card>
      </div>
    </AuthGuard>
  );
}
