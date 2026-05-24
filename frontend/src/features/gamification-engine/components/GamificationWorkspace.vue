<template>
  <div class="h-full flex flex-col gap-4 p-4 overflow-auto">
    <!-- Header -->
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <div class="w-8 h-8 rounded-lg bg-orange-600 flex items-center justify-center">
          <span class="text-white font-bold text-sm">🏆</span>
        </div>
        <div>
          <h2 class="text-base font-bold text-white">Gamification Engine</h2>
          <p class="text-[10px] text-slate-400">Streak • Badges • Leaderboard</p>
        </div>
      </div>

      <!-- XP Display -->
      <div class="flex items-center gap-4">
        <div class="text-right">
          <div class="text-lg font-bold text-cyan-400">{{ store.currentXP.toLocaleString() }} XP</div>
          <div class="text-[10px] text-slate-500">Tổng điểm kinh nghiệm</div>
        </div>

        <!-- Streak Freeze Button -->
        <button
          @click="handleStreakFreeze"
          class="px-3 py-1.5 rounded-lg text-xs font-medium transition-colors"
          :class="
            store.streakFreezesCount > 0
              ? 'bg-cyan-600/20 text-cyan-400 border border-cyan-500/30 hover:bg-cyan-600/30'
              : 'bg-slate-800/50 text-slate-600 border border-slate-700 cursor-not-allowed'
          "
          :disabled="store.streakFreezesCount === 0"
        >
          ❄️ Freeze ({{ store.streakFreezesCount }})
        </button>

        <!-- Earn XP Demo Button -->
        <button
          @click="handleEarnXP"
          class="px-3 py-1.5 rounded-lg text-xs font-medium bg-emerald-600/20 text-emerald-400 border border-emerald-500/30 hover:bg-emerald-600/30 transition-colors"
        >
          +50 XP Demo
        </button>
      </div>
    </div>

    <!-- XP Progress Bar -->
    <div class="rounded-xl bg-slate-900/45 border border-white/5 backdrop-blur-xl p-4">
      <div class="flex items-center justify-between mb-2">
        <span class="text-xs text-slate-400">Tiến độ huy hiệu tiếp theo</span>
        <span class="text-xs text-cyan-400">{{ store.xpProgressPercent }}%</span>
      </div>
      <div class="h-2 rounded-full bg-slate-800 overflow-hidden">
        <div
          class="h-full rounded-full bg-gradient-to-r from-cyan-500 to-emerald-500 transition-all duration-500"
          :style="{ width: `${store.xpProgressPercent}%` }"
        />
      </div>
      <div class="flex items-center justify-between mt-1">
        <span class="text-[10px] text-slate-600">{{ store.currentXP }} XP</span>
        <span class="text-[10px] text-slate-600">{{ store.nextBadgeXPThreshold }} XP</span>
      </div>
    </div>

    <!-- Main Grid -->
    <div class="flex-1 grid grid-cols-2 gap-4 min-h-0">
      <!-- Left Column: Streak + Badges -->
      <div class="flex flex-col gap-4 overflow-auto">
        <StreakFire :streak-count="store.activeStreak" />
        <BadgesCabinet
          :all-badges="store.allBadges"
          :unlocked-badges="store.unlockedBadges"
        />
      </div>

      <!-- Right Column: Leaderboard -->
      <div class="overflow-auto">
        <WeeklyLeaderboard :entries="store.leaderboardData" />
      </div>
    </div>

    <!-- Confetti Overlay -->
    <CanvasConfettiOverlay :visible="store.showConfetti" />
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useGamificationStore } from '../store/useGamificationStore';
import StreakFire from './StreakFire.vue';
import BadgesCabinet from './BadgesCabinet.vue';
import WeeklyLeaderboard from './WeeklyLeaderboard.vue';
import CanvasConfettiOverlay from './CanvasConfettiOverlay.vue';

const store = useGamificationStore();

function handleEarnXP(): void {
  store.earnXPLocal(50);
}

function handleStreakFreeze(): void {
  store.useStreakFreeze();
}

onMounted(() => {
  store.setLeaderboardData([
    { userId: 'user-009', fullName: 'Nguyễn Hoàng Nam', weeklyXP: 1450, rank: 1 },
    { userId: 'user-012', fullName: 'Trần Tuấn Kiệt', weeklyXP: 1250, rank: 2 },
    { userId: 'user-005', fullName: 'Lê Hà Phương', weeklyXP: 1100, rank: 3 },
    { userId: 'user-003', fullName: 'Phạm Minh Đức', weeklyXP: 950, rank: 4 },
    { userId: 'user-007', fullName: 'Võ Thanh Tùng', weeklyXP: 870, rank: 5 },
    { userId: 'user-001', fullName: 'Đặng Thị Mai', weeklyXP: 780, rank: 6 },
    { userId: 'user-011', fullName: 'Huỳnh Văn Hải', weeklyXP: 650, rank: 7 },
    { userId: 'user-008', fullName: 'Bùi Quang Huy', weeklyXP: 520, rank: 8 },
    { userId: 'user-004', fullName: 'Lý Ngọc Trâm', weeklyXP: 410, rank: 9 },
    { userId: 'user-010', fullName: 'Cao Đình Khoa', weeklyXP: 300, rank: 10 },
  ]);
});
</script>
