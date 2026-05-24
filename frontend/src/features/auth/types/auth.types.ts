export interface UserDto {
  id: string;
  email: string;
  username: string;
  totalXP: number;
  currentLevel: number;
  streakDays: number;
  createdAt: string;
  badges: BadgeDto[];
}

export interface BadgeDto {
  id: string;
  name: string;
  description: string;
  icon: string;
  color: string;
  earnedAt: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  user: UserDto;
}

export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}
