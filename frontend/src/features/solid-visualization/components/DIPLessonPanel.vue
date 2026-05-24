<template>
  <div class="dip-panel flex flex-col gap-4">
    <!-- DIP Header -->
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-2">
        <span class="w-2 h-2 rounded-full" :class="isViolating ? 'bg-red-500 animate-pulse' : 'bg-emerald-500'" />
        <span class="text-xs font-bold uppercase tracking-wider text-slate-300">
          DIP — Dependency Inversion Principle
        </span>
      </div>
      <span
        class="text-[10px] font-bold uppercase tracking-wider px-2 py-1 rounded-lg"
        :class="isViolating
          ? 'bg-red-950/50 text-red-400 border border-red-800/40'
          : 'bg-emerald-950/50 text-emerald-400 border border-emerald-800/40'"
      >
        {{ isViolating ? 'DIRECT COUPLING' : 'INVERTED' }}
      </span>
    </div>

    <!-- Neon Flowing Path -->
    <NeonFlowingPath
      :is-violating="isViolating"
      :has-interface="hasInterface"
    />

    <!-- Action Buttons -->
    <div class="flex gap-3">
      <button
        class="flex-1 py-2.5 rounded-xl text-xs font-bold uppercase tracking-wider
               bg-emerald-950/40 text-emerald-400 border border-emerald-800/40
               hover:bg-emerald-900/60 transition-all"
        :disabled="hasInterface"
        @click="$emit('insertInterface')"
      >
        Chèn Interface trừu tượng
      </button>
      <button
        class="flex-1 py-2.5 rounded-xl text-xs font-bold uppercase tracking-wider
               bg-slate-800/40 text-slate-400 border border-slate-700/40
               hover:bg-slate-700/60 transition-all"
        :disabled="!hasInterface"
        @click="$emit('resetDIP')"
      >
        Reset DIP
      </button>
    </div>

    <!-- Diagnostic result -->
    <div
      v-if="diagnosticResult"
      class="text-xs font-bold px-4 py-2.5 rounded-xl backdrop-blur-md border"
      :class="isViolating
        ? 'bg-red-950/40 text-red-400 border-red-800/40'
        : 'bg-emerald-950/40 text-emerald-400 border-emerald-800/40'"
    >
      {{ diagnosticResult }}
    </div>
  </div>
</template>

<script setup lang="ts">
import NeonFlowingPath from './NeonFlowingPath.vue';

defineProps<{
  isViolating: boolean;
  hasInterface: boolean;
  diagnosticResult: string | null;
}>();

defineEmits<{
  insertInterface: [];
  resetDIP: [];
}>();
</script>
