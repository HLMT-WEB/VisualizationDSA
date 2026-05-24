// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { setActivePinia, createPinia } from 'pinia';
import { useQuizStore } from '../../features/quiz-system/store/useQuizStore';

describe('Quiz Server Sync (B3)', () => {
  beforeEach(() => {
    setActivePinia(createPinia());
    localStorage.clear();
    vi.restoreAllMocks();
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      status: 200,
      json: () => Promise.resolve([]),
    }));
  });

  afterEach(() => {
    vi.unstubAllGlobals();
  });

  describe('isOnlineMode', () => {
    it('should be false when not authenticated', () => {
      const store = useQuizStore();
      expect(store.isOnlineMode).toBe(false);
    });

    it('should be true when token exists', () => {
      localStorage.setItem('vdsa_access_token', 'token');
      const store = useQuizStore();
      expect(store.isOnlineMode).toBe(true);
    });
  });

  describe('fetchQuizzesFromServer', () => {
    it('should skip fetch when offline', async () => {
      const store = useQuizStore();
      await store.fetchQuizzesFromServer();
      expect(fetch).not.toHaveBeenCalled();
      expect(store.serverQuizzes).toEqual([]);
    });

    it('should populate serverQuizzes when online', async () => {
      localStorage.setItem('vdsa_access_token', 'token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve([
          { id: 'q1', title: 'Sorting Quiz', topic: 'sorting', questions: [], passingScore: 70, xpReward: 50 },
          { id: 'q2', title: 'Tree Quiz', topic: 'trees', questions: [], passingScore: 60, xpReward: 40 },
        ]),
      }));

      const store = useQuizStore();
      await store.fetchQuizzesFromServer();
      expect(store.serverQuizzes).toHaveLength(2);
      expect(store.serverQuizzes[0].title).toBe('Sorting Quiz');
    });

    it('should set error on server failure', async () => {
      localStorage.setItem('vdsa_access_token', 'token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false,
        status: 500,
        statusText: 'Error',
        json: () => Promise.resolve({ status: 500, title: 'Error', detail: '' }),
      }));

      const store = useQuizStore();
      await store.fetchQuizzesFromServer();
      expect(store.quizSyncError).toBe('Không thể tải danh sách quiz');
    });
  });

  describe('fetchQuizHistory', () => {
    it('should populate quizHistory when online', async () => {
      localStorage.setItem('vdsa_access_token', 'token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve([
          { quizId: 'q1', quizTitle: 'Sort', score: 8, maxScore: 10, passed: true, attemptedAt: '2024-01-01' },
        ]),
      }));

      const store = useQuizStore();
      await store.fetchQuizHistory();
      expect(store.quizHistory).toHaveLength(1);
      expect(store.quizHistory[0].passed).toBe(true);
    });
  });

  describe('server state', () => {
    it('should track loading state during fetch', async () => {
      localStorage.setItem('vdsa_access_token', 'token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve([]),
      }));

      const store = useQuizStore();
      const promise = store.fetchQuizzesFromServer();
      expect(store.isLoadingQuizzes).toBe(true);
      await promise;
      expect(store.isLoadingQuizzes).toBe(false);
    });
  });
});
