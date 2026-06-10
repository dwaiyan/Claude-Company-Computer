// Import this in main.ts to enable mock mode
// All API calls will return mock data without hitting any backend

import {
  mockAuthResponse, mockTechTrees, mockNodes,
  getMockResources, getMockQuestions,
  mockLearningProfile, mockCheckIns, mockAssessments,
  mockNotes, mockDiscussions,
  mockLanguages, mockRunCode,
} from './data';

// Set flag BEFORE any API modules are imported
(window as any).__MOCK_MODE__ = true;

// ── Pre-fill localStorage so the app starts logged in ──
localStorage.setItem('accessToken', mockAuthResponse.accessToken);
localStorage.setItem('refreshToken', mockAuthResponse.refreshToken);
localStorage.setItem('user', JSON.stringify(mockAuthResponse.user));
localStorage.setItem('theme-preference', 'light');

// ── Mock API exports ──
export { mockAuthResponse };
export { mockTechTrees, mockNodes, getMockResources, getMockQuestions };
export { mockLearningProfile, mockCheckIns, mockAssessments };
export { mockNotes, mockDiscussions };
export { mockLanguages, mockRunCode };

export function isMockMode(): boolean {
  return !!(window as any).__MOCK_MODE__;
}
