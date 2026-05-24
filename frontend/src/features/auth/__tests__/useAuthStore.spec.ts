// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { setActivePinia, createPinia } from 'pinia';
import { useAuthStore } from '../store/useAuthStore';

vi.stubGlobal('fetch', vi.fn());

describe('useAuthStore', () => {
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

  describe('initial state', () => {
    it('should start unauthenticated', () => {
      const store = useAuthStore();
      expect(store.isAuthenticated).toBe(false);
      expect(store.currentUser).toBeNull();
      expect(store.userId).toBeNull();
      expect(store.isLoading).toBe(false);
      expect(store.error).toBeNull();
    });
  });

  describe('register', () => {
    it('should set user and tokens on successful registration', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({
          token: 'jwt-token',
          refreshToken: 'refresh-token',
          user: {
            id: 'user-1',
            email: 'test@example.com',
            username: 'testuser',
            totalXP: 0,
            currentLevel: 1,
            streakDays: 0,
            createdAt: '2024-01-01',
            badges: [],
          },
        }),
      }));

      const store = useAuthStore();
      const result = await store.register({
        email: 'test@example.com',
        username: 'testuser',
        password: 'Password123',
      });

      expect(result).toBe(true);
      expect(store.isAuthenticated).toBe(true);
      expect(store.currentUser?.username).toBe('testuser');
      expect(localStorage.getItem('vdsa_access_token')).toBe('jwt-token');
    });

    it('should set error on failed registration', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false,
        status: 409,
        statusText: 'Conflict',
        json: () => Promise.resolve({
          status: 409,
          title: 'Conflict',
          detail: 'Email đã tồn tại',
        }),
      }));

      const store = useAuthStore();
      const result = await store.register({
        email: 'dup@example.com',
        username: 'dup',
        password: 'Password123',
      });

      expect(result).toBe(false);
      expect(store.error).toBe('Email đã tồn tại');
      expect(store.isAuthenticated).toBe(false);
    });
  });

  describe('login', () => {
    it('should authenticate on successful login', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({
          token: 'login-token',
          refreshToken: 'login-refresh',
          user: {
            id: 'user-2',
            email: 'login@test.com',
            username: 'loginuser',
            totalXP: 500,
            currentLevel: 3,
            streakDays: 5,
            createdAt: '2024-01-01',
            badges: [],
          },
        }),
      }));

      const store = useAuthStore();
      const result = await store.login({
        email: 'login@test.com',
        password: 'Password123',
      });

      expect(result).toBe(true);
      expect(store.currentUser?.totalXP).toBe(500);
    });

    it('should set error on wrong credentials', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false,
        status: 401,
        statusText: 'Unauthorized',
        json: () => Promise.resolve({
          status: 401,
          title: 'Unauthorized',
          detail: 'Sai mật khẩu',
        }),
      }));

      const store = useAuthStore();
      const result = await store.login({
        email: 'test@test.com',
        password: 'wrong',
      });

      expect(result).toBe(false);
      expect(store.error).toBe('Sai mật khẩu');
    });
  });

  describe('logout', () => {
    it('should clear user and tokens', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({
          token: 't', refreshToken: 'r',
          user: { id: '1', email: 'a@b.c', username: 'u', totalXP: 0, currentLevel: 1, streakDays: 0, createdAt: '', badges: [] },
        }),
      }));

      const store = useAuthStore();
      await store.login({ email: 'a@b.c', password: 'x' });
      expect(store.isAuthenticated).toBe(true);

      store.logout();
      expect(store.isAuthenticated).toBe(false);
      expect(store.currentUser).toBeNull();
      expect(localStorage.getItem('vdsa_access_token')).toBeNull();
    });
  });

  describe('fetchCurrentUser', () => {
    it('should fetch user when token exists', async () => {
      localStorage.setItem('vdsa_access_token', 'existing-token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({
          id: 'user-3',
          email: 'fetch@test.com',
          username: 'fetchuser',
          totalXP: 200,
          currentLevel: 2,
          streakDays: 3,
          createdAt: '2024-01-01',
          badges: [],
        }),
      }));

      const store = useAuthStore();
      await store.fetchCurrentUser();
      expect(store.currentUser?.username).toBe('fetchuser');
    });

    it('should skip fetch when no token', async () => {
      const store = useAuthStore();
      await store.fetchCurrentUser();
      expect(store.currentUser).toBeNull();
      expect(fetch).not.toHaveBeenCalled();
    });

    it('should clear tokens on fetch error', async () => {
      localStorage.setItem('vdsa_access_token', 'bad-token');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false,
        status: 401,
        statusText: 'Unauthorized',
        json: () => Promise.resolve({ status: 401, title: 'Unauthorized', detail: '' }),
      }));

      const store = useAuthStore();
      await store.fetchCurrentUser();
      expect(store.currentUser).toBeNull();
      expect(localStorage.getItem('vdsa_access_token')).toBeNull();
    });
  });

  describe('clearError', () => {
    it('should clear error state', () => {
      const store = useAuthStore();
      store.error = 'some error';
      store.clearError();
      expect(store.error).toBeNull();
    });
  });
});
