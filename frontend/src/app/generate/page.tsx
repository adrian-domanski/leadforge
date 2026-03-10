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

export default function GeneratePage() {
  const [inputText, setInputText] = useState('');
  const [goalType, setGoalType] = useState<GoalType>('LeadGeneration');
  const [result, setResult] = useState('');
  const [loading, setLoading] = useState(false);

  const handleGenerate = async () => {
    setLoading(true);

    try {
      const data = await generatePost({
        inputText,
        goalType,
      });

      setResult(data.outputText);
      toast.success('Post generated');
    } catch (err) {
      handleApiError(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <AuthGuard>
      <AppLayout>
        <div className='max-w-3xl space-y-6'>
          <h1 className='text-2xl font-bold'>Generate LinkedIn Post</h1>

          <Textarea
            placeholder='Describe the idea for your LinkedIn post...'
            value={inputText}
            onChange={(e) => setInputText(e.target.value)}
          />

          <Select
            defaultValue='LeadGeneration'
            onValueChange={(value) => setGoalType(value as GoalType)}
          >
            <SelectTrigger>
              <SelectValue />
            </SelectTrigger>

            <SelectContent>
              <SelectItem value='LeadGeneration'>Lead Generation</SelectItem>
              <SelectItem value='Authority'>Authority</SelectItem>
              <SelectItem value='Storytelling'>Storytelling</SelectItem>
              <SelectItem value='Engagement'>Engagement</SelectItem>
            </SelectContent>
          </Select>

          <Button onClick={handleGenerate} disabled={loading}>
            {loading ? 'Generating...' : 'Generate Post'}
          </Button>

          {result && (
            <>
              <div className='border rounded-lg p-4 whitespace-pre-wrap'>
                {result}
              </div>
              <Button
                variant='outline'
                onClick={() => navigator.clipboard.writeText(result)}
              >
                Copy
              </Button>
            </>
          )}
        </div>
      </AppLayout>
    </AuthGuard>
  );
}
