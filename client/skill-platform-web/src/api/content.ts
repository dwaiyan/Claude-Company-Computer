import http from './http';
import { mockTechTrees, mockNodes, getMockResources, getMockQuestions } from '@/mocks/data';

export interface TechTreeDto { id: string; title: string; description: string; icon: string | null; children: TechTreeDto[]; }
export interface TechNodeDto { id: string; title: string; description: string; level: number; parentId: string | null; children: TechNodeDto[]; resourceCount: number; questionCount: number; }
export interface ResourceDto { id: string; title: string; url: string; type: string; difficulty: number; }
export interface InterviewQuestionDto { id: string; question: string; answerTip: string; difficulty: number; category: string; }

const MOCK = (window as any).__MOCK_MODE__;

function delay<T>(data: T): Promise<T> {
  return new Promise(r => setTimeout(() => r(data), 60 + Math.random() * 100));
}

export const contentApi = {
  getTechTrees(): Promise<TechTreeDto[]> {
    if (MOCK) return delay(mockTechTrees);
    return http.get('/api/tech-trees').then(r => r.data);
  },
  getNodes(_treeId: string): Promise<TechNodeDto[]> {
    if (MOCK) return delay(mockNodes);
    return http.get('/api/tech-trees/nodes', { params: { tree: _treeId } }).then(r => r.data);
  },
  getNode(id: string): Promise<TechNodeDto> {
    if (MOCK) return delay(mockNodes.find(n => n.id === id)!);
    return http.get(`/api/tech-trees/nodes/${id}`).then(r => r.data);
  },
  getResources(nodeId: string): Promise<ResourceDto[]> {
    if (MOCK) return delay(getMockResources(nodeId));
    return http.get(`/api/tech-trees/nodes/${nodeId}/resources`).then(r => r.data);
  },
  getQuestions(nodeId: string, _difficulty?: number): Promise<InterviewQuestionDto[]> {
    if (MOCK) return delay(getMockQuestions(nodeId));
    return http.get(`/api/tech-trees/nodes/${nodeId}/questions`, { params: { difficulty: _difficulty } }).then(r => r.data);
  },
};
