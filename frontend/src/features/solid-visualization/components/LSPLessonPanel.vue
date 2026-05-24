<template>
  <div class="lsp-panel flex flex-col gap-4">
    <!-- LSP Header -->
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-2">
        <span class="w-2 h-2 rounded-full" :class="phaseStatusDot" />
        <span class="text-xs font-bold uppercase tracking-wider text-slate-300">
          LSP — Liskov Substitution Principle
        </span>
      </div>
      <span
        class="text-[10px] font-bold uppercase tracking-wider px-2 py-1 rounded-lg"
        :class="phaseBadgeClass"
      >
        {{ phaseBadgeText }}
      </span>
    </div>

    <!-- Laser Fracture Overlay -->
    <LaserFractureOverlay
      :phase="lspPhase"
      :source-point="{ x: 80, y: 90 }"
      :target-point="{ x: 420, y: 90 }"
      source-label="makeBirdFly(bird)"
      target-label="Ostrich"
      :error-message="diagnosticResult ?? 'Đà điểu không thể bay!'"
    />

    <!-- Action Buttons -->
    <div class="flex gap-3">
      <button
        class="flex-1 py-2.5 rounded-xl text-xs font-bold uppercase tracking-wider
               bg-red-950/40 text-red-400 border border-red-800/40
               hover:bg-red-900/60 transition-all"
        :disabled="lspPhase === 'TRANSMITTING'"
        @click="$emit('runViolation')"
      >
        🦤 Thay thế Ostrich (Vi phạm)
      </button>
      <button
        class="flex-1 py-2.5 rounded-xl text-xs font-bold uppercase tracking-wider
               bg-emerald-950/40 text-emerald-400 border border-emerald-800/40
               hover:bg-emerald-900/60 transition-all"
        :disabled="lspPhase === 'TRANSMITTING'"
        @click="$emit('runValid')"
      >
        🕊️ Thay thế Eagle (Đạt)
      </button>
    </div>

    <!-- Diagnostic result -->
    <div
      v-if="diagnosticResult"
      class="text-xs font-bold px-4 py-2.5 rounded-xl backdrop-blur-md border"
      :class="lspPhase === 'SHATTERED'
        ? 'bg-red-950/40 text-red-400 border-red-800/40'
        : 'bg-emerald-950/40 text-emerald-400 border-emerald-800/40'"
    >
      {{ diagnosticResult }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { LSPSubstitutionPhase } from '../types/solid-visualization.types';
import LaserFractureOverlay from './LaserFractureOverlay.vue';

const props = defineProps<{
  lspPhase: LSPSubstitutionPhase;
  diagnosticResult: string | null;
}>();

defineEmits<{
  runViolation: [];
  runValid: [];
}>();

const phaseStatusDot = computed(() => {
  switch (props.lspPhase) {
    case 'TRANSMITTING': return 'bg-amber-500 animate-pulse';
    case 'SHATTERED': return 'bg-red-500 animate-pulse';
    case 'PASSED': return 'bg-emerald-500';
    default: return 'bg-slate-500';
  }
});

const phaseBadgeClass = computed(() => {
  switch (props.lspPhase) {
    case 'TRANSMITTING': return 'bg-amber-950/50 text-amber-400 border border-amber-800/40';
    case 'SHATTERED': return 'bg-red-950/50 text-red-400 border border-red-800/40';
    case 'PASSED': return 'bg-emerald-950/50 text-emerald-400 border border-emerald-800/40';
    default: return 'bg-slate-800/50 text-slate-400 border border-slate-700/40';
  }
});

const phaseBadgeText = computed(() => {
  switch (props.lspPhase) {
    case 'TRANSMITTING': return 'TRANSMITTING...';
    case 'SHATTERED': return 'SHATTERED!';
    case 'PASSED': return 'LSP PASSED';
    default: return 'IDLE';
  }
});
</script>
