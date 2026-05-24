// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { setActivePinia, createPinia } from 'pinia';
import { useGamificationStore } from '../../features/gamification-engine/store/useGamificationStore';

describe('Gamification Server Sync (B3)', () => {
  beforeEach(() => {
    setActivePinia(createPinia());
    localStorage.clear();
    vi.restoreAllMocks();
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      status: 200,
      json: () => Promise.resolve({}),
    }));
  });

  afterEach(() => {
    vi.unstubAllGlobals();
  });

  describe('isOnlineMode', () => {
    it('should be false when no token stored', () => {
      const store = useGamificationStore();
      expect(store.isOnlineMode).toBe(false);
    });

    it('should be true when token stored', () => {
      localStorage.setItem('vdsa_access_token', 'test-token');
      const store = useGamificationStore();
      expect(store.isOnlineMode).toBe(true);
    });
  });

  describe('earnXPWithSync', () => {
    it('should update local XP without server call when offline', async () => {
      const store = useGamificationStore();
      await store.earnXPWithSync(50, 'quiz');
      expect(store.currentXP).toBe(50);
      expect(fetch).not.toHaveBeenCalled();
    });

    it('should call server when online and update XP from response', async () => {
      localStorage.setItem('vdsa_access_token', 'token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({ message: 'OK', totalXP: 150 }),
      }));

      const store = useGamificationStore();
      await store.earnXPWithSync(50, 'practice');
      expect(store.currentXP).toBe(150);
    });

    it('should set syncError on server failure', async () => {
      localStorage.setItem('vdsa_access_token', 'token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false,
        status: 500,
        statusText: 'Internal Server Error',
        json: () => Promise.resolve({ status: 500, title: 'Error', detail: 'Server error' }),
      }));

      const store = useGamificationStore();
      await store.earnXPWithSync(50, 'error');
      expect(store.syncError).toBe('Không thể đồng bộ XP với server');
      expect(store.currentXP).toBe(50);
    });
  });

  describe('syncProgressFromServer', () => {
    it('should skip when offline', async () => {
      const store = useGamificationStore();
      await store.syncProgressFromServer();
      expect(fetch).not.toHaveBeenCalled();
    });

    it('should update state from server response', async () => {
      localStorage.setItem('vdsa_access_token', 'token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({
          totalXP: 300,
          streakDays: 5,
          badges: [{ badgeId: 'sorting-champion', earnedAt: '2024-01-01' }],
          completedModules: [],
        }),
      }));

      const store = useGamificationStore();
      await store.syncProgressFromServer();
      expect(store.currentXP).toBe(300);
      expect(store.activeStreak).toBe(5);
      expect(store.unlockedBadges).toContain('sorting-champion');
    });
  });

  describe('fetchLeaderboardFromServer', () => {
    it('should populate leaderboardData from API', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve([
          { rank: 1, username: 'alice', totalXP: 1000, currentLevel: 4, badgeCount: 3 },
          { rank: 2, username: 'bob', totalXP: 800, currentLevel: 3, badgeCount: 2 },
        ]),
      }));

      const store = useGamificationStore();
      await store.fetchLeaderboardFromServer();
      expect(store.leaderboardData).toHaveLength(2);
      expect(store.leaderboardData[0].displayName).toBe('alice');
      expect(store.leaderboardData[0].xp).toBe(1000);
      expect(store.leaderboardData[1].rank).toBe(2);
    });

    it('should set syncError on failure', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false,
        status: 500,
        statusText: 'Error',
        json: () => Promise.resolve({ status: 500, title: 'Error', detail: '' }),
      }));

      const store = useGamificationStore();
      await store.fetchLeaderboardFromServer();
      expect(store.syncError).toBe('Không thể tải bảng xếp hạng');
    });
  });
});
