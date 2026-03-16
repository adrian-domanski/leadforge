'use client';

import { useQuery } from '@tanstack/react-query';
import { api } from '@/api/client';
import { AppLayout } from '@/components/layout/app-layout';
import { GenerationListItem } from '@/types/api';
import { Skeleton } from '@/components/ui/skeleton';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { AuthGuard } from '@/components/auth/AuthGuard';
import Link from 'next/link';
import { Sparkles } from 'lucide-react';
import { Markdown } from '@/components/ui/markdown';

export default function Dashboard() {
  const { data, isLoading } = useQuery({
    queryKey: ['dashboard'],
    queryFn: () => api.get('/dashboard').then((res) => res.data),
  });

  if (isLoading) {
    return (
      <AuthGuard>
        <AppLayout>
          <div className='space-y-6'>
            <Skeleton className='h-8 w-40' />
            <Skeleton className='h-6 w-32' />

            <div className='space-y-4'>
              <Skeleton className='h-20 w-full rounded-lg' />
              <Skeleton className='h-20 w-full rounded-lg' />
              <Skeleton className='h-20 w-full rounded-lg' />
            </div>
          </div>
        </AppLayout>
      </AuthGuard>
    );
  }

  return (
    <AuthGuard>
      <AppLayout>
        <div className='space-y-8 max-w-3xl'>
          {/* Header */}
          <div className='flex items-center justify-between'>
            <h1 className='text-2xl font-bold'>Dashboard</h1>

            {data.credits > 0 ? (
              <Link href='/generate'>
                <Button className='flex items-center gap-2'>
                  <Sparkles className='h-4 w-4' />
                  Generate post
                </Button>
              </Link>
            ) : (
              <Button
                disabled
                className='flex items-center gap-2'
                title='You need credits to generate posts'
              >
                <Sparkles className='h-4 w-4' />
                No credits left
              </Button>
            )}
          </div>

          {/* Credits */}
          <Card>
            <CardHeader>
              <CardTitle>Your Credits</CardTitle>
            </CardHeader>

            <CardContent>
              <p className='text-3xl font-semibold'>{data.credits}</p>

              <p className='text-sm text-muted-foreground'>
                Each generation uses 1 credit
              </p>
            </CardContent>
          </Card>

          {/* Recent generations */}
          <div>
            <div className='flex items-center justify-between mb-4'>
              <h2 className='font-semibold'>Recent generations</h2>

              <Link href='/history'>
                <Button variant='outline' size='sm'>
                  View all
                </Button>
              </Link>
            </div>

            {data.recentGenerations.length === 0 && (
              <p className='text-muted-foreground text-sm'>
                No generations yet. Try creating your first post.
              </p>
            )}

            <div className='space-y-3'>
              {data.recentGenerations.map((g: GenerationListItem) => (
                <Card key={g.id}>
                  <CardContent className='p-4 space-y-3'>
                    <div className='flex items-center justify-between text-xs text-muted-foreground'>
                      <span>{g.goalType}</span>

                      <span>{new Date(g.createdAt).toLocaleDateString()}</span>
                    </div>

                    <Markdown content={g.outputText} />

                    <Button
                      variant='outline'
                      size='sm'
                      onClick={() =>
                        navigator.clipboard.writeText(g.outputText)
                      }
                    >
                      Copy
                    </Button>
                  </CardContent>
                </Card>
              ))}
            </div>
          </div>
        </div>
      </AppLayout>
    </AuthGuard>
  );
}
