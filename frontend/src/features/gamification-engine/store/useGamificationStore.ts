import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { StreakCalculator } from '../engine/StreakCalculator';
import { GamificationEngine } from '../engine/GamificationEngine';
import {
  CONFETTI_DURATION_MS,
  MAX_STREAK_FREEZES,
  LEADERBOARD_TOP_N,
} from '../types/gamification.types';
import type { LeaderboardEntry, UserProgressState } from '../types/gamification.types';

export const useGamificationStore = defineStore('gamification-engine', () => {
  // ==========================================
  // STATE
  // ==========================================
  const currentXP = ref(0);
  const activeStreak = ref(0);
  const lastActiveDate = ref('');
  const unlockedBadges = ref<string[]>([]);
  const showConfetti = ref(false);
  const leaderboardRank = ref(0);
  const streakFreezesCount = ref(MAX_STREAK_FREEZES);
  const leaderboardData = ref<LeaderboardEntry[]>([]);

  // ==========================================
  // COMPUTED
  // ==========================================
  const allBadges = computed(() => GamificationEngine.getBadgeTemplates());

  const lockedBadges = computed(() =>
    allBadges.value.filter(b => !unlockedBadges.value.includes(b.id)),
  );

  const nextBadgeXPThreshold = computed(() => {
    const locked = lockedBadges.value;
    if (locked.length === 0) return 0;
    const sorted = [...locked].sort((a, b) => a.xpThresholdRequired - b.xpThresholdRequired);
    return sorted[0].xpThresholdRequired;
  });

  const xpProgressPercent = computed(() => {
    if (nextBadgeXPThreshold.value === 0) return 100;
    return Math.min(100, Math.round((currentXP.value / nextBadgeXPThreshold.value) * 100));
  });

  const streakStatus = computed<'active' | 'inactive'>(() => {
    return activeStreak.value > 0 ? 'active' : 'inactive';
  });

  // ==========================================
  // ACTIONS
  // ==========================================

  function earnXPLocal(amount: number): void {
    if (!GamificationEngine.validateXPAmount(amount)) return;

    const todayStr = StreakCalculator.getAdjustedDate(new Date());
    currentXP.value += amount;

    const { nextStreak, shouldUpdate } = StreakCalculator.calculateUpdatedStreak(
      lastActiveDate.value,
      activeStreak.value,
      todayStr,
    );

    if (shouldUpdate) {
      activeStreak.value = nextStreak;
      lastActiveDate.value = todayStr;
    }

    checkAndUnlockBadges();
  }

  function checkAndUnlockBadges(): void {
    const userState: UserProgressState = {
      userId: 'current-user',
      totalXP: currentXP.value,
      activeStreak: activeStreak.value,
      lastActiveDate: lastActiveDate.value,
      unlockedBadges: unlockedBadges.value,
      streakFreezesCount: streakFreezesCount.value,
    };

    const newUnlocked = GamificationEngine.checkNewUnlockedBadges(userState);
    if (newUnlocked.length > 0) {
      unlockedBadges.value.push(...newUnlocked);
      triggerConfettiRain();
    }
  }

  function triggerConfettiRain(): void {
    showConfetti.value = true;
    setTimeout(() => {
      showConfetti.value = false;
    }, CONFETTI_DURATION_MS);
  }

  function useStreakFreeze(): boolean {
    if (streakFreezesCount.value > 0) {
      streakFreezesCount.value--;
      return true;
    }
    return false;
  }

  function setLeaderboardData(data: LeaderboardEntry[]): void {
    const sorted = [...data].sort((a, b) => a.rank - b.rank);
    leaderboardData.value = sorted.slice(0, LEADERBOARD_TOP_N);
  }

  function setStreakForTesting(streak: number): void {
    activeStreak.value = streak;
  }

  return {
    // State
    currentXP,
    activeStreak,
    lastActiveDate,
    unlockedBadges,
    showConfetti,
    leaderboardRank,
    streakFreezesCount,
    leaderboardData,
    // Computed
    allBadges,
    lockedBadges,
    nextBadgeXPThreshold,
    xpProgressPercent,
    streakStatus,
    // Actions
    earnXPLocal,
    checkAndUnlockBadges,
    triggerConfettiRain,
    useStreakFreeze,
    setLeaderboardData,
    setStreakForTesting,
  };
});
