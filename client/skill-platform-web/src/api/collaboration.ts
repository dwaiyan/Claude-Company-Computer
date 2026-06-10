import http from './http';
import { mockNotes, mockDiscussions } from '@/mocks/data';

export interface NoteDto { id: string; title: string; content: string; nodeId: string; userId: string; status: string; viewCount: number; likeCount: number; createdAt: string; }
export interface DiscussionDto { id: string; nodeId: string; userId: string; content: string; parentId: string | null; replies: DiscussionDto[]; createdAt: string; }

const MOCK = (window as any).__MOCK_MODE__;

function delay<T>(data: T): Promise<T> {
  return new Promise(r => setTimeout(() => r(data), 60 + Math.random() * 100));
}

export const collabApi = {
  getNotes(_nodeId?: string): Promise<NoteDto[]> {
    if (MOCK) return delay(mockNotes);
    return http.get('/api/notes', { params: { node: _nodeId } }).then(r => r.data);
  },
  getNote(id: string): Promise<NoteDto> {
    if (MOCK) return delay(mockNotes.find(n => n.id === id)!);
    return http.get(`/api/notes/${id}`).then(r => r.data);
  },
  createNote(data: { title: string; content: string; nodeId: string }): Promise<NoteDto> {
    if (MOCK) return delay({ ...data, id: 'note-new', createdAt: new Date().toISOString(), viewCount: 0, likeCount: 0, userId: 'u-001', status: 'published' });
    return http.post('/api/notes', data).then(r => r.data);
  },
  getDiscussions(_nodeId?: string): Promise<DiscussionDto[]> {
    if (MOCK) return delay(mockDiscussions);
    return http.get('/api/discussions', { params: { node: _nodeId } }).then(r => r.data);
  },
  addDiscussion(data: { content: string; nodeId: string; parentId?: string }) {
    if (MOCK) return delay({ ...data, id: 'disc-new', createdAt: new Date().toISOString(), replies: [], userId: 'u-001' });
    return http.post('/api/discussions', data).then(r => r.data);
  },
};
