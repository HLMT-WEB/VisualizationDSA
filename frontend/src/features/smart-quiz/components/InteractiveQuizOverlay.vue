<template>
  <div
    class="smart-quiz-overlay-panel"
    :class="{
      'is-active': store.overlayStatus === 'SLIDE_IN' || store.overlayStatus === 'SUBMITTED',
      'is-shake': shakeActive,
    }"
  >
    <!-- Quiz Header -->
    <div class="flex items-center justify-between mb-4">
      <div class="flex items-center gap-2">
        <div class="w-8 h-8 rounded-lg bg-cyan-600/20 border border-cyan-500/30 flex items-center justify-center">
          <span class="text-cyan-400 text-sm font-bold">?</span>
        </div>
        <h3 class="text-white font-semibold text-sm">Trắc Nghiệm Tương Tác</h3>
      </div>
      <span
        v-if="store.activeQuiz"
        class="text-[10px] px-2 py-0.5 rounded-full font-mono"
        :class="questionTypeBadgeClass"
      >
        {{ questionTypeLabel }}
      </span>
    </div>

    <!-- Prompt Text -->
    <p
      v-if="store.activeQuiz"
      class="text-slate-300 text-sm leading-relaxed mb-4"
    >
      {{ store.activeQuiz.promptText }}
    </p>

    <!-- Multiple Choice Options -->
    <div
      v-if="store.activeQuiz?.questionType === 'MULTIPLE_CHOICE' && store.activeQuiz.options"
      class="space-y-2 mb-4"
    >
      <button
        v-for="option in store.activeQuiz.options"
        :key="option.id"
        @click="store.toggleAnswerSelection(option.id)"
        class="w-full text-left px-3 py-2 rounded-lg text-xs font-medium transition-all duration-200 border"
        :class="optionClass(option.id)"
        :disabled="store.evaluationResult.hasSubmitted"
      >
        {{ option.label }}
      </button>
    </div>

    <!-- SVG / Monaco Click Hint -->
    <div
      v-if="store.activeQuiz?.questionType !== 'MULTIPLE_CHOICE' && !store.evaluationResult.hasSubmitted"
      class="mb-4 px-3 py-2 rounded-lg border border-cyan-500/20 bg-cyan-500/5"
    >
      <p class="text-cyan-400 text-[11px]">
        <span v-if="store.activeQuiz?.questionType === 'SVG_NODE_CLICK'">
          👆 Click chọn các phần tử trên Canvas SVG bên trái
        </span>
        <span v-else>
          👆 Click chọn dòng code trong Monaco Editor
        </span>
      </p>
      <p class="text-slate-500 text-[10px] mt-1">
        Đã chọn: {{ store.selectionCount }} / {{ store.maxSelections }}
      </p>
    </div>

    <!-- Selected Answers Display (SVG/Monaco) -->
    <div
      v-if="store.selectedAnswers.length > 0 && store.activeQuiz?.questionType !== 'MULTIPLE_CHOICE'"
      class="flex flex-wrap gap-1 mb-3"
    >
      <span
        v-for="ans in store.selectedAnswers"
        :key="ans"
        class="px-2 py-0.5 rounded text-[10px] font-mono bg-amber-500/10 text-amber-400 border border-amber-500/20"
      >
        {{ ans }}
      </span>
    </div>

    <!-- Action Buttons -->
    <div class="flex gap-2">
      <button
        v-if="!store.evaluationResult.hasSubmitted"
        @click="handleSubmit"
        :disabled="!store.canSubmit"
        class="flex-1 px-4 py-2 rounded-lg text-xs font-bold transition-all duration-200"
        :class="store.canSubmit
          ? 'bg-cyan-600 hover:bg-cyan-500 text-white shadow-lg shadow-cyan-600/20'
          : 'bg-slate-700 text-slate-500 cursor-not-allowed'"
      >
        SUBMIT
      </button>

      <button
        v-if="store.evaluationResult.hasSubmitted && !store.evaluationResult.isCorrect"
        @click="store.retryQuiz()"
        class="flex-1 px-4 py-2 rounded-lg text-xs font-bold bg-amber-600 hover:bg-amber-500 text-white transition-colors"
      >
        THỬ LẠI
      </button>

      <button
        v-if="store.evaluationResult.hasSubmitted"
        @click="store.closeQuiz()"
        class="flex-1 px-4 py-2 rounded-lg text-xs font-bold bg-slate-700 hover:bg-slate-600 text-slate-300 transition-colors"
      >
        TIẾP TỤC
      </button>
    </div>

    <!-- Evaluation Feedback -->
    <ExplanationHSLCard
      v-if="store.evaluationResult.hasSubmitted && store.activeQuiz"
      :is-correct="store.evaluationResult.isCorrect"
      :score-percentage="store.evaluationResult.scorePercentage"
      :explanation="store.activeQuiz.explanationMarkdown"
      :xp-awarded="store.xpAwarded"
    />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { computed } from 'vue';
import { useSmartQuizStore } from '../store/useSmartQuizStore';
import ExplanationHSLCard from './ExplanationHSLCard.vue';

const store = useSmartQuizStore();

const shakeActive = ref(false);

const questionTypeLabel = computed(() => {
  switch (store.activeQuiz?.questionType) {
    case 'SVG_NODE_CLICK': return 'SVG Click';
    case 'MONACO_LINE_CLICK': return 'Monaco';
    case 'MULTIPLE_CHOICE': return 'Trắc nghiệm';
    default: return '';
  }
});

const questionTypeBadgeClass = computed(() => {
  switch (store.activeQuiz?.questionType) {
    case 'SVG_NODE_CLICK':
      return 'bg-cyan-500/10 text-cyan-400 border border-cyan-500/20';
    case 'MONACO_LINE_CLICK':
      return 'bg-amber-500/10 text-amber-400 border border-amber-500/20';
    case 'MULTIPLE_CHOICE':
      return 'bg-emerald-500/10 text-emerald-400 border border-emerald-500/20';
    default:
      return 'bg-slate-500/10 text-slate-400';
  }
});

function optionClass(optionId: string): string {
  const isSelected = store.selectedAnswers.includes(optionId);
  if (store.evaluationResult.hasSubmitted) {
    const isCorrect = store.activeQuiz?.correctAnswers.includes(optionId);
    if (isCorrect) return 'border-emerald-500/40 bg-emerald-500/10 text-emerald-400';
    if (isSelected && !isCorrect) return 'border-red-500/40 bg-red-500/10 text-red-400';
    return 'border-slate-700 bg-slate-800/50 text-slate-500';
  }
  if (isSelected) return 'border-amber-500/40 bg-amber-500/10 text-amber-300';
  return 'border-slate-700 bg-slate-800/50 text-slate-400 hover:border-cyan-500/30 hover:bg-cyan-500/5';
}

function handleSubmit(): void {
  const isCorrect = store.submitAnswers();
  if (!isCorrect) {
    shakeActive.value = true;
    setTimeout(() => {
      shakeActive.value = false;
    }, 400);
  }
}
</script>

<style scoped>
.smart-quiz-overlay-panel {
  position: absolute;
  top: 20px;
  right: -380px;
  width: 340px;
  background: rgba(10, 15, 30, 0.65);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 20px;
  padding: 24px;
  backdrop-filter: blur(16px);
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.6);
  z-index: 100;
  transition: right 0.5s cubic-bezier(0.16, 1, 0.3, 1);
}

.smart-quiz-overlay-panel.is-active {
  right: 20px;
}

.smart-quiz-overlay-panel.is-shake {
  animation: quiz-shake 0.4s ease-in-out;
}

@keyframes quiz-shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-6px); }
  75% { transform: translateX(6px); }
}
</style>
