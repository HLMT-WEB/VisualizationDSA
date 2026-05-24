// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import {
  getStoredToken,
  getStoredRefreshToken,
  setStoredTokens,
  clearStoredTokens,
  apiRequest,
  api,
} from '../apiClient';

describe('apiClient', () => {
  beforeEach(() => {
    localStorage.clear();
    vi.restoreAllMocks();
  });

  afterEach(() => {
    vi.unstubAllGlobals();
  });

  describe('token storage', () => {
    it('should store and retrieve access token', () => {
      setStoredTokens('test-token', 'test-refresh');
      expect(getStoredToken()).toBe('test-token');
      expect(getStoredRefreshToken()).toBe('test-refresh');
    });

    it('should return null when no token stored', () => {
      expect(getStoredToken()).toBeNull();
      expect(getStoredRefreshToken()).toBeNull();
    });

    it('should clear tokens', () => {
      setStoredTokens('token', 'refresh');
      clearStoredTokens();
      expect(getStoredToken()).toBeNull();
      expect(getStoredRefreshToken()).toBeNull();
    });
  });

  describe('apiRequest', () => {
    it('should make GET request with correct URL', async () => {
      const mockResponse = { data: 'test' };
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve(mockResponse),
      }));

      const result = await apiRequest('/test-endpoint');
      expect(result).toEqual(mockResponse);
      expect(fetch).toHaveBeenCalledWith(
        expect.stringContaining('/test-endpoint'),
        expect.objectContaining({ headers: expect.any(Object) }),
      );
    });

    it('should attach Authorization header when token exists', async () => {
      setStoredTokens('my-jwt-token', 'my-refresh');
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({}),
      }));

      await apiRequest('/protected');
      expect(fetch).toHaveBeenCalledWith(
        expect.any(String),
        expect.objectContaining({
          headers: expect.objectContaining({
            'Authorization': 'Bearer my-jwt-token',
          }),
        }),
      );
    });

    it('should not attach Authorization header when no token', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({}),
      }));

      await apiRequest('/public');
      const callArgs = (fetch as ReturnType<typeof vi.fn>).mock.calls[0];
      expect(callArgs[1].headers['Authorization']).toBeUndefined();
    });

    it('should throw ApiError on non-ok response', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false,
        status: 404,
        statusText: 'Not Found',
        json: () => Promise.resolve({ status: 404, title: 'Not Found', detail: 'Resource not found' }),
      }));

      await expect(apiRequest('/missing')).rejects.toEqual({
        status: 404,
        title: 'Not Found',
        detail: 'Resource not found',
      });
    });

    it('should return undefined for 204 No Content', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 204,
      }));

      const result = await apiRequest('/delete-something');
      expect(result).toBeUndefined();
    });

    it('should attempt token refresh on 401', async () => {
      setStoredTokens('expired-token', 'valid-refresh');

      let callCount = 0;
      vi.stubGlobal('fetch', vi.fn().mockImplementation((url: string) => {
        if (url.includes('/auth/refresh')) {
          return Promise.resolve({
            ok: true,
            status: 200,
            json: () => Promise.resolve({ token: 'new-token', refreshToken: 'new-refresh' }),
          });
        }
        callCount++;
        if (callCount === 1) {
          return Promise.resolve({
            ok: false,
            status: 401,
            statusText: 'Unauthorized',
            json: () => Promise.resolve({ status: 401, title: 'Unauthorized', detail: 'Token expired' }),
          });
        }
        return Promise.resolve({
          ok: true,
          status: 200,
          json: () => Promise.resolve({ success: true }),
        });
      }));

      const result = await apiRequest('/protected-resource');
      expect(result).toEqual({ success: true });
      expect(getStoredToken()).toBe('new-token');
    });
  });

  describe('api convenience methods', () => {
    beforeEach(() => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: true,
        status: 200,
        json: () => Promise.resolve({ ok: true }),
      }));
    });

    it('api.get should use GET method', async () => {
      await api.get('/items');
      const callArgs = (fetch as ReturnType<typeof vi.fn>).mock.calls[0];
      expect(callArgs[1].method).toBeUndefined();
    });

    it('api.post should use POST method with body', async () => {
      await api.post('/items', { name: 'test' });
      const callArgs = (fetch as ReturnType<typeof vi.fn>).mock.calls[0];
      expect(callArgs[1].method).toBe('POST');
      expect(callArgs[1].body).toBe(JSON.stringify({ name: 'test' }));
    });

    it('api.put should use PUT method', async () => {
      await api.put('/items/1', { name: 'updated' });
      const callArgs = (fetch as ReturnType<typeof vi.fn>).mock.calls[0];
      expect(callArgs[1].method).toBe('PUT');
    });

    it('api.delete should use DELETE method', async () => {
      await api.delete('/items/1');
      const callArgs = (fetch as ReturnType<typeof vi.fn>).mock.calls[0];
      expect(callArgs[1].method).toBe('DELETE');
    });
  });
});
