import { Toaster } from 'sonner';
import './globals.css';
import { Providers } from './providers';
import { Inter } from 'next/font/google';
import { Metadata } from 'next';

const inter = Inter({
  subsets: ['latin'],
});

export const metadata: Metadata = {
  title: {
    default: 'LeadForge App',
    template: '%s | LeadForge App',
  },
  description: 'Generate high-performing LinkedIn posts with AI',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang='en'>
      <body className={inter.className}>
        <Providers>{children}</Providers>
        <Toaster />
      </body>
    </html>
  );
}
