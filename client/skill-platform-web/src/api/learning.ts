import http from './http';
import { mockLearningProfile, mockCheckIns, mockAssessments } from '@/mocks/data';

export interface LearningProfile { userId: string; streakDays: number; totalCheckIns: number; weeklyMinutes: number; skillRadar: SkillRadarItem[]; }
export interface SkillRadarItem { nodeId: string; nodeTitle: string; selfScore: number; level: number; }
export interface CheckInDto { id: string; date: string; durationMinutes: number; note: string | null; }
export interface AssessmentDto { id: string; title: string; type: string; nodeId: string; questions: any; timeLimit: number; passScore: number; }

const MOCK = (window as any).__MOCK_MODE__;

function delay<T>(data: T): Promise<T> {
  return new Promise(r => setTimeout(() => r(data), 60 + Math.random() * 100));
}

export const learningApi = {
  getProfile(): Promise<LearningProfile> {
    if (MOCK) return delay(mockLearningProfile);
    return http.get('/api/learning/profile').then(r => r.data);
  },
  checkIn(durationMinutes: number, note?: string): Promise<CheckInDto> {
    if (MOCK) return delay({ id: 'ci-new', date: new Date().toISOString().slice(0, 10), durationMinutes, note: note || null });
    return http.post('/api/learning/check-in', { durationMinutes, note }).then(r => r.data);
  },
  getCheckIns(): Promise<CheckInDto[]> {
    if (MOCK) return delay(mockCheckIns);
    return http.get('/api/learning/check-ins').then(r => r.data);
  },
  getStreak(): Promise<{ streakDays: number }> {
    if (MOCK) return delay({ streakDays: mockLearningProfile.streakDays });
    return http.get('/api/learning/streak').then(r => r.data);
  },
  getSkillRadar(): Promise<SkillRadarItem[]> {
    if (MOCK) return delay(mockLearningProfile.skillRadar);
    return http.get('/api/learning/skill-radar').then(r => r.data);
  },
  updateSkillRadar(_nodeId: string, _selfScore: number, _level: number) {
    if (MOCK) return delay({ ok: true });
    return http.put('/api/learning/skill-radar', { nodeId: _nodeId, selfScore: _selfScore, level: _level }).then(r => r.data);
  },
  getAssessments(nodeId?: string): Promise<AssessmentDto[]> {
    if (MOCK) return delay(mockAssessments.filter(a => !nodeId || a.nodeId === nodeId));
    return http.get('/api/learning/assessments', { params: { node: nodeId } }).then(r => r.data);
  },
};
