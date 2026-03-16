'use client';

import { useState } from 'react';
import { AppLayout } from '@/components/layout/app-layout';
import { generatePost } from '@/api/generations';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from '@/components/ui/select';
import { GoalType } from '@/types/api';
import { toast } from 'sonner';
import { handleApiError } from '@/lib/handleApiError';
import { AuthGuard } from '@/components/auth/AuthGuard';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import ReactMarkdown from 'react-markdown';
import { useQueryClient } from '@tanstack/react-query';

export default function GeneratePage() {
  const [inputText, setInputText] = useState('');
  const [goalType, setGoalType] = useState<GoalType>('LeadGeneration');
  const [result, setResult] = useState('');
  const [loading, setLoading] = useState(false);
  const queryClient = useQueryClient();

  const handleGenerate = async () => {
    if (!inputText.trim()) {
      toast.error('Please describe your post idea');
      return;
    }

    try {
      setLoading(true);

      const data = await generatePost({
        inputText,
        goalType,
      });

      setResult(data.outputText);

      // UX improvement
      setInputText('');

      // Invalidate sidebar /me
      queryClient.invalidateQueries({ queryKey: ['me'] });

      toast.success('Post generated');
    } catch (err) {
      handleApiError(err);
    } finally {
      setLoading(false);
    }
  };

  const copyToClipboard = async () => {
    await navigator.clipboard.writeText(result);
    toast.success('Copied to clipboard');
  };

  return (
    <AuthGuard>
      <AppLayout>
        <div className='max-w-3xl space-y-8'>
          <h1 className='text-2xl font-bold'>Generate LinkedIn Post</h1>

          {/* Input Card */}
          <Card>
            <CardHeader>
              <CardTitle>Create new post</CardTitle>
            </CardHeader>

            <CardContent className='space-y-4'>
              <Textarea
                placeholder='Describe the idea for your LinkedIn post...'
                value={inputText}
                onChange={(e) => setInputText(e.target.value)}
                className='min-h-30'
              />

              <Select
                value={goalType}
                onValueChange={(value) => setGoalType(value as GoalType)}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>

                <SelectContent>
                  <SelectItem value='LeadGeneration'>
                    Lead Generation
                  </SelectItem>
                  <SelectItem value='Authority'>Authority</SelectItem>
                  <SelectItem value='Storytelling'>Storytelling</SelectItem>
                  <SelectItem value='Engagement'>Engagement</SelectItem>
                </SelectContent>
              </Select>

              <Button
                onClick={handleGenerate}
                disabled={loading || !inputText.trim()}
                className='w-full'
              >
                {loading ? 'Generating...' : 'Generate Post'}
              </Button>
            </CardContent>
          </Card>

          {/* Result */}
          {result && (
            <Card>
              <CardHeader>
                <CardTitle>Generated Post</CardTitle>
              </CardHeader>

              <CardContent className='space-y-4'>
                <div className='border rounded-lg p-4 bg-muted/30 prose prose-sm max-w-none'>
                  <ReactMarkdown>{result}</ReactMarkdown>
                </div>

                <div className='flex gap-3'>
                  <Button onClick={copyToClipboard}>Copy</Button>

                  <Button variant='outline' onClick={() => setResult('')}>
                    Clear
                  </Button>
                </div>
              </CardContent>
            </Card>
          )}
        </div>
      </AppLayout>
    </AuthGuard>
  );
}
