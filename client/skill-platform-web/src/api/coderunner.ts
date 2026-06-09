import http from './http';

export interface CodeRunRequest {
  code: string;
  timeoutMs?: number;
}

export interface CodeRunResult {
  success: boolean;
  output: string;
  errors: string;
  compilationSucceeded: boolean;
  executionTimeMs: number;
}

export interface LanguageInfo {
  id: string;
  name: string;
  template: string;
}

export const codeRunnerApi = {
  run(request: CodeRunRequest): Promise<CodeRunResult> {
    return http.post('/api/code/run', request).then(r => r.data);
  },
  getLanguages(): Promise<LanguageInfo[]> {
    return http.get('/api/code/languages').then(r => r.data);
  },
};
