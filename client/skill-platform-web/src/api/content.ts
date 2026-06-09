import http from './http';

export interface TechTreeDto {
  id: string;
  title: string;
  description: string;
  icon: string | null;
  children: TechTreeDto[];
}

export interface TechNodeDto {
  id: string;
  title: string;
  description: string;
  level: number;
  parentId: string | null;
  children: TechNodeDto[];
  resourceCount: number;
  questionCount: number;
}

export interface ResourceDto {
  id: string;
  title: string;
  url: string;
  type: string;
  difficulty: number;
}

export interface InterviewQuestionDto {
  id: string;
  question: string;
  answerTip: string;
  difficulty: number;
  category: string;
}

export const contentApi = {
  getTechTrees(): Promise<TechTreeDto[]> {
    return http.get('/api/tech-trees').then((r) => r.data);
  },
  getNodes(treeId: string): Promise<TechNodeDto[]> {
    return http.get('/api/tech-trees/nodes', { params: { tree: treeId } }).then((r) => r.data);
  },
  getNode(id: string): Promise<TechNodeDto> {
    return http.get(`/api/tech-trees/nodes/${id}`).then((r) => r.data);
  },
  getResources(nodeId: string): Promise<ResourceDto[]> {
    return http.get(`/api/tech-trees/nodes/${nodeId}/resources`).then((r) => r.data);
  },
  getQuestions(nodeId: string, difficulty?: number): Promise<InterviewQuestionDto[]> {
    return http.get(`/api/tech-trees/nodes/${nodeId}/questions`, { params: { difficulty } }).then((r) => r.data);
  },
};
