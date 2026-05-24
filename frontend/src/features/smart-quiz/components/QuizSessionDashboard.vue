<template>
  <div class="rounded-2xl border border-slate-700/50 bg-slate-800/30 backdrop-blur-sm p-4">
    <h3 class="text-white font-semibold text-sm mb-3">📊 Thống Kê Phiên Quiz</h3>

    <!-- Stats Grid -->
    <div class="grid grid-cols-3 gap-3 mb-4">
      <div class="rounded-xl bg-slate-900/50 border border-slate-700/30 p-3 text-center">
        <p class="text-2xl font-bold text-cyan-400">{{ store.sessionStats.totalQuestions }}</p>
        <p class="text-[10px] text-slate-500 mt-1">Câu hỏi</p>
      </div>
      <div class="rounded-xl bg-slate-900/50 border border-emerald-500/20 p-3 text-center">
        <p class="text-2xl font-bold text-emerald-400">{{ store.sessionStats.correctFirstTry }}</p>
        <p class="text-[10px] text-slate-500 mt-1">Đúng lần đầu</p>
      </div>
      <div class="rounded-xl bg-slate-900/50 border border-amber-500/20 p-3 text-center">
        <p class="text-2xl font-bold text-amber-400">{{ store.sessionStats.totalXPEarned }}</p>
        <p class="text-[10px] text-slate-500 mt-1">XP kiếm được</p>
      </div>
    </div>

    <!-- Accuracy Bar -->
    <div class="mb-4">
      <div class="flex items-center justify-between mb-1">
        <span class="text-[10px] text-slate-500">Độ chính xác</span>
        <span class="text-[10px] font-mono text-emerald-400">{{ accuracyPercent }}%</span>
      </div>
      <div class="h-2 rounded-full bg-slate-900/80 overflow-hidden">
        <div
          class="h-full rounded-full bg-gradient-to-r from-emerald-500 to-cyan-500 transition-all duration-500"
          :style="{ width: `${accuracyPercent}%` }"
        />
      </div>
    </div>

    <!-- Demo Trigger Buttons -->
    <div class="space-y-2">
      <p class="text-[10px] text-slate-500 font-medium mb-1">Kích hoạt Demo Quiz:</p>
      <button
        v-for="(quiz, index) in demoQuizLabels"
        :key="index"
        @click="store.triggerDemoQuiz(index)"
        :disabled="store.isQuizVisible"
        class="w-full px-3 py-2 rounded-lg text-xs font-medium transition-all duration-200 text-left"
        :class="store.isQuizVisible
          ? 'bg-slate-800 text-slate-600 cursor-not-allowed'
          : 'bg-slate-800/50 text-slate-300 hover:bg-cyan-500/10 hover:text-cyan-400 border border-slate-700/50 hover:border-cyan-500/30'"
      >
        <span class="text-cyan-400/60 mr-2">{{ quiz.badge }}</span>
        {{ quiz.label }}
      </button>
    </div>

    <!-- Reset Button -->
    <button
      @click="store.resetSession()"
      class="w-full mt-3 px-3 py-2 rounded-lg text-xs font-medium bg-slate-800/50 text-slate-400 hover:text-red-400 hover:bg-red-500/10 border border-slate-700/50 hover:border-red-500/30 transition-all duration-200"
    >
      🔄 Reset Session
    </button>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useSmartQuizStore } from '../store/useSmartQuizStore';

const store = useSmartQuizStore();

const accuracyPercent = computed(() => {
  if (store.sessionStats.totalQuestions === 0) return 0;
  return Math.round(
    (store.sessionStats.correctFirstTry / store.sessionStats.totalQuestions) * 100
  );
});

const demoQuizLabels = [
  { badge: 'SVG', label: 'Heap Sort — Chọn phần tử hoán đổi' },
  { badge: 'MON', label: 'QuickSort — Chọn dòng code tiếp theo' },
  { badge: 'MC', label: 'Bubble Sort — Trắc nghiệm so sánh' },
];
</script>
