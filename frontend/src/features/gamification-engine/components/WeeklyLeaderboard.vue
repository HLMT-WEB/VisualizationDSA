<template>
  <div class="rounded-2xl bg-slate-900/45 border border-white/5 backdrop-blur-xl p-6">
    <h3 class="text-sm font-semibold text-slate-300 uppercase tracking-wider mb-4">
      Bảng Vinh Danh Top 10 Tuần
    </h3>
    <div class="space-y-2">
      <div
        v-for="entry in entries"
        :key="entry.userId"
        class="flex items-center gap-3 px-3 py-2 rounded-lg transition-colors"
        :class="podiumClass(entry.rank)"
      >
        <span
          class="w-7 h-7 rounded-full flex items-center justify-center text-xs font-bold"
          :class="rankBadgeClass(entry.rank)"
        >
          {{ entry.rank }}
        </span>
        <span class="flex-1 text-sm truncate" :class="nameClass(entry.rank)">
          {{ entry.fullName }}
        </span>
        <span class="text-xs font-mono" :class="xpClass(entry.rank)">
          {{ entry.weeklyXP.toLocaleString() }} XP
        </span>
      </div>
      <div
        v-if="entries.length === 0"
        class="text-center py-8 text-slate-600 text-xs"
      >
        Chưa có dữ liệu xếp hạng tuần này
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { LeaderboardEntry } from '../types/gamification.types';

defineProps<{
  entries: LeaderboardEntry[];
}>();

function podiumClass(rank: number): string {
  if (rank === 1) return 'leaderboard-podium-first';
  if (rank === 2) return 'leaderboard-podium-second';
  if (rank === 3) return 'leaderboard-podium-third';
  return 'bg-slate-800/30';
}

function rankBadgeClass(rank: number): string {
  if (rank === 1) return 'bg-amber-500/20 text-amber-400 border border-amber-500/30';
  if (rank === 2) return 'bg-slate-400/20 text-slate-300 border border-slate-400/30';
  if (rank === 3) return 'bg-orange-700/20 text-orange-400 border border-orange-700/30';
  return 'bg-slate-700/50 text-slate-500';
}

function nameClass(rank: number): string {
  if (rank <= 3) return 'text-white font-medium';
  return 'text-slate-400';
}

function xpClass(rank: number): string {
  if (rank === 1) return 'text-amber-400';
  if (rank === 2) return 'text-slate-300';
  if (rank === 3) return 'text-orange-400';
  return 'text-slate-500';
}
</script>

<style scoped>
.leaderboard-podium-first {
  border: 1px solid rgba(245, 158, 11, 0.3);
  box-shadow: 0 0 20px rgba(245, 158, 11, 0.1);
  background: linear-gradient(180deg, rgba(245, 158, 11, 0.05) 0%, transparent 100%);
}

.leaderboard-podium-second {
  border: 1px solid rgba(148, 163, 184, 0.2);
  box-shadow: 0 0 15px rgba(148, 163, 184, 0.08);
}

.leaderboard-podium-third {
  border: 1px solid rgba(180, 83, 9, 0.2);
  box-shadow: 0 0 10px rgba(180, 83, 9, 0.06);
}
</style>
