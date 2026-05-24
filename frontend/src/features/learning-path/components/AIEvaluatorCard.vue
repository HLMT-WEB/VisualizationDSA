<template>
  <div
    v-if="recommendation.recommendedNodeId || recommendation.recommendationReason"
    class="ai-evaluator-card"
  >
    <div class="flex items-center gap-2 mb-3">
      <span class="text-amber-400 text-lg">🧠</span>
      <h3 class="text-sm font-bold text-amber-300 uppercase tracking-wider">
        AI Path Advisor
      </h3>
    </div>

    <p class="text-sm text-slate-300 leading-relaxed mb-4">
      {{ recommendation.recommendationReason }}
    </p>

    <div v-if="recommendation.recommendedNodeId" class="flex items-center gap-3">
      <button
        @click="$emit('navigate-to', recommendation.recommendedNodeId)"
        class="px-4 py-2 rounded-lg text-xs font-bold uppercase tracking-wider transition-all duration-300"
        :class="isReviewMode
          ? 'bg-amber-500/20 text-amber-300 border border-amber-500/50 hover:bg-amber-500/30'
          : 'bg-cyan-500/20 text-cyan-300 border border-cyan-500/50 hover:bg-cyan-500/30'"
      >
        {{ isReviewMode ? 'Ôn Tập Ngay' : 'Bắt Đầu Học' }}
      </button>

      <div class="flex items-center gap-1">
        <div class="w-2 h-2 rounded-full" :class="isReviewMode ? 'bg-amber-400' : 'bg-cyan-400'" />
        <span class="text-[10px] text-slate-500">
          {{ isReviewMode ? 'Cần ôn tập' : 'Sẵn sàng' }}
        </span>
      </div>
    </div>

    <!-- Completion banner -->
    <div
      v-if="!recommendation.recommendedNodeId && recommendation.recommendationReason"
      class="flex items-center gap-2 px-3 py-2 rounded-lg bg-emerald-500/10 border border-emerald-500/30"
    >
      <span class="text-emerald-400">🏆</span>
      <span class="text-xs text-emerald-300 font-medium">Hoàn thành toàn bộ lộ trình!</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { AIRecommendation } from '../types/learning-path.types';

const props = defineProps<{
  recommendation: AIRecommendation;
  isReviewMode: boolean;
}>();

defineEmits<{
  (e: 'navigate-to', nodeId: string): void;
}>();
</script>

<style scoped>
.ai-evaluator-card {
  background: rgba(15, 23, 42, 0.8);
  backdrop-filter: blur(12px);
  border: 1px solid rgba(245, 158, 11, 0.3);
  border-radius: 16px;
  padding: 16px;
  box-shadow: 0 0 30px rgba(245, 158, 11, 0.1);
}
</style>
