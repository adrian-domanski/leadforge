'use client';

import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { AppLayout } from '@/components/layout/app-layout';
import { getGenerations, deleteGeneration } from '@/api/generations';
import { Button } from '@/components/ui/button';
import { AuthGuard } from '@/components/auth/AuthGuard';

export default function HistoryPage() {
  const queryClient = useQueryClient();

  const [page, setPage] = useState(1);
  const pageSize = 5;

  const { data, isLoading } = useQuery({
    queryKey: ['generations', page],
    queryFn: () => getGenerations(page, pageSize),
    placeholderData: (prev) => prev,
  });

  const deleteMutation = useMutation({
    mutationFn: deleteGeneration,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['generations'] });
    },
  });

  if (isLoading) return <div>Loading...</div>;

  if (!data?.items.length) {
    return (
      <AuthGuard>
        <AppLayout>
          <div className='text-muted-foreground'>
            No generations yet. Try generating your first post.
          </div>
        </AppLayout>
      </AuthGuard>
    );
  }

  const totalPages = Math.ceil(data.totalCount / pageSize);

  return (
    <AuthGuard>
      <AppLayout>
        <div className='max-w-4xl space-y-6'>
          <h1 className='text-2xl font-bold'>History</h1>

          {data.items.map((g) => (
            <div key={g.id} className='border rounded-lg p-4 space-y-3'>
              <span className='text-xs text-muted-foreground'>
                {new Date(g.createdAt).toLocaleDateString()}
              </span>

              <div className='flex justify-between items-center'>
                <span className='text-sm text-muted-foreground'>
                  {g.goalType}
                </span>

                <Button
                  variant='destructive'
                  size='sm'
                  onClick={() => deleteMutation.mutate(g.id)}
                >
                  Delete
                </Button>
              </div>

              <div className='whitespace-pre-wrap'>{g.outputText}</div>
            </div>
          ))}

          {/* Pagination */}
          <div className='flex items-center justify-between pt-4'>
            <Button
              variant='outline'
              disabled={page === 1}
              onClick={() => setPage((p) => p - 1)}
            >
              Previous
            </Button>

            <span className='text-sm text-muted-foreground'>
              Page {page} of {totalPages}
            </span>

            <Button
              variant='outline'
              disabled={page === totalPages}
              onClick={() => setPage((p) => p + 1)}
            >
              Next
            </Button>
          </div>
        </div>
      </AppLayout>
    </AuthGuard>
  );
}
