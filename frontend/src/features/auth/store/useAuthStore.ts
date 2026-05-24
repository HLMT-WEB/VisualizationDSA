import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { api, setStoredTokens, clearStoredTokens, getStoredToken } from '../../../services/apiClient';
import type { UserDto, AuthResponse, RegisterRequest, LoginRequest } from '../types/auth.types';

export const useAuthStore = defineStore('auth', () => {
  const currentUser = ref<UserDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  const isAuthenticated = computed(() => currentUser.value !== null);
  const userId = computed(() => currentUser.value?.id ?? null);

  async function register(request: RegisterRequest): Promise<boolean> {
    isLoading.value = true;
    error.value = null;
    try {
      const response = await api.post<AuthResponse>('/auth/register', request);
      setStoredTokens(response.token, response.refreshToken);
      currentUser.value = response.user;
      return true;
    } catch (err: unknown) {
      const apiErr = err as { detail?: string; title?: string };
      error.value = apiErr.detail || apiErr.title || 'Đăng ký thất bại';
      return false;
    } finally {
      isLoading.value = false;
    }
  }

  async function login(request: LoginRequest): Promise<boolean> {
    isLoading.value = true;
    error.value = null;
    try {
      const response = await api.post<AuthResponse>('/auth/login', request);
      setStoredTokens(response.token, response.refreshToken);
      currentUser.value = response.user;
      return true;
    } catch (err: unknown) {
      const apiErr = err as { detail?: string; title?: string };
      error.value = apiErr.detail || apiErr.title || 'Đăng nhập thất bại';
      return false;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchCurrentUser(): Promise<void> {
    if (!getStoredToken()) return;
    isLoading.value = true;
    try {
      const user = await api.get<UserDto>('/auth/me');
      currentUser.value = user;
    } catch {
      clearStoredTokens();
      currentUser.value = null;
    } finally {
      isLoading.value = false;
    }
  }

  function logout(): void {
    clearStoredTokens();
    currentUser.value = null;
    error.value = null;
  }

  function clearError(): void {
    error.value = null;
  }

  return {
    currentUser,
    isLoading,
    error,
    isAuthenticated,
    userId,
    register,
    login,
    fetchCurrentUser,
    logout,
    clearError,
  };
});
