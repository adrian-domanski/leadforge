'use client';

import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';

type MarkdownProps = {
  content: string;
  className?: string;
};

export function Markdown({ content, className }: MarkdownProps) {
  return (
    <div
      className={`prose text-base prose-sm max-w-none dark:prose-invert ${className ?? ''}`}
    >
      <ReactMarkdown
        remarkPlugins={[remarkGfm]}
        components={{
          a: (props) => (
            <a {...props} target='_blank' rel='noopener noreferrer' />
          ),
        }}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
}
