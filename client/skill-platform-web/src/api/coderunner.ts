import http from './http';
import { mockLanguages, mockRunCode } from '@/mocks/data';

export interface CodeRunRequest { code: string; timeoutMs?: number; }
export interface CodeRunResult { success: boolean; output: string; errors: string; compilationSucceeded: boolean; executionTimeMs: number; }
export interface LanguageInfo { id: string; name: string; template: string; }

const MOCK = (window as any).__MOCK_MODE__;

function delay<T>(data: T): Promise<T> {
  return new Promise(r => setTimeout(() => r(data), 150 + Math.random() * 300));
}

export const codeRunnerApi = {
  run(request: CodeRunRequest): Promise<CodeRunResult> {
    if (MOCK) return delay(mockRunCode(request.code));
    return http.post('/api/code/run', request).then(r => r.data);
  },
  getLanguages(): Promise<LanguageInfo[]> {
    if (MOCK) return delay(mockLanguages);
    return http.get('/api/code/languages').then(r => r.data);
  },
};
