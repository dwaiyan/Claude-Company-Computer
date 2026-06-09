import http from './http';

export interface NoteDto {
  id: string; title: string; content: string; nodeId: string;
  userId: string; status: string; viewCount: number; likeCount: number;
  createdAt: string;
}

export interface DiscussionDto {
  id: string; nodeId: string; userId: string; content: string;
  parentId: string | null; replies: DiscussionDto[]; createdAt: string;
}

export const collabApi = {
  getNotes(nodeId: string): Promise<NoteDto[]> {
    return http.get('/api/notes', { params: { node: nodeId } }).then(r => r.data);
  },
  getNote(id: string): Promise<NoteDto> {
    return http.get(`/api/notes/${id}`).then(r => r.data);
  },
  createNote(data: { title: string; content: string; nodeId: string }): Promise<NoteDto> {
    return http.post('/api/notes', data).then(r => r.data);
  },
  getDiscussions(nodeId: string): Promise<DiscussionDto[]> {
    return http.get('/api/discussions', { params: { node: nodeId } }).then(r => r.data);
  },
  addDiscussion(data: { content: string; nodeId: string; parentId?: string }) {
    return http.post('/api/discussions', data).then(r => r.data);
  },
};
