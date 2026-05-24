<template>
  <div class="w-full h-full flex gap-4">
    <!-- Left: SVG Quiz Canvas Area -->
    <div class="flex-1 relative rounded-2xl border border-slate-700/50 bg-slate-900/30 backdrop-blur-sm overflow-hidden">
      <!-- Title Bar -->
      <div class="absolute top-0 left-0 right-0 z-10 px-4 py-2 bg-slate-900/80 backdrop-blur-sm border-b border-slate-700/30 flex items-center justify-between">
        <div class="flex items-center gap-2">
          <span class="text-cyan-400 text-sm">🧠</span>
          <h2 class="text-white text-sm font-semibold">Smart Interactive Quiz</h2>
        </div>
        <div class="flex items-center gap-2">
          <span
            v-if="store.isVCRLocked"
            class="px-2 py-0.5 rounded-full text-[10px] font-bold bg-red-500/10 text-red-400 border border-red-500/20"
          >
            TIMELINE LOCKED
          </span>
          <span
            v-else
            class="px-2 py-0.5 rounded-full text-[10px] font-bold bg-emerald-500/10 text-emerald-400 border border-emerald-500/20"
          >
            TIMELINE READY
          </span>
        </div>
      </div>

      <!-- Canvas -->
      <div class="pt-10 h-full">
        <SVGQuizCanvas />
      </div>

      <!-- Quiz Overlay (slides in from right) -->
      <InteractiveQuizOverlay />
    </div>

    <!-- Right: Session Dashboard -->
    <div class="w-80 flex-shrink-0">
      <QuizSessionDashboard />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useSmartQuizStore } from '../store/useSmartQuizStore';
import SVGQuizCanvas from './SVGQuizCanvas.vue';
import InteractiveQuizOverlay from './InteractiveQuizOverlay.vue';
import QuizSessionDashboard from './QuizSessionDashboard.vue';

const store = useSmartQuizStore();
</script>
