import { marked } from 'marked';

// Configure marked for safe, clean rendering
marked.setOptions({
  breaks: true,
  gfm: true,
});

export function useMarkdown() {
  function render(raw: string): string {
    if (!raw) return '';
    return marked.parse(raw) as string;
  }

  return { render };
}
