import http from './http';

export interface LearningProfile {
  userId: string;
  streakDays: number;
  totalCheckIns: number;
  weeklyMinutes: number;
  skillRadar: SkillRadarItem[];
}

export interface SkillRadarItem {
  nodeId: string;
  nodeTitle: string;
  selfScore: number;
  level: number;
}

export interface CheckInDto {
  id: string;
  date: string;
  durationMinutes: number;
  note: string | null;
}

export interface AssessmentDto {
  id: string;
  title: string;
  type: string;
  nodeId: string;
  questions: any;
  timeLimit: number;
  passScore: number;
}

export const learningApi = {
  getProfile(): Promise<LearningProfile> {
    return http.get('/api/learning/profile').then((r) => r.data);
  },
  checkIn(durationMinutes: number, note?: string): Promise<CheckInDto> {
    return http.post('/api/learning/check-in', { durationMinutes, note }).then((r) => r.data);
  },
  getCheckIns(): Promise<CheckInDto[]> {
    return http.get('/api/learning/check-ins').then((r) => r.data);
  },
  getStreak(): Promise<{ streakDays: number }> {
    return http.get('/api/learning/streak').then((r) => r.data);
  },
  getSkillRadar(): Promise<SkillRadarItem[]> {
    return http.get('/api/learning/skill-radar').then((r) => r.data);
  },
  updateSkillRadar(nodeId: string, selfScore: number, level: number) {
    return http.put('/api/learning/skill-radar', { nodeId, selfScore, level }).then((r) => r.data);
  },
  getAssessments(nodeId: string): Promise<AssessmentDto[]> {
    return http.get('/api/learning/assessments', { params: { node: nodeId } }).then((r) => r.data);
  },
};
