<template>
  <div class="solid-workspace bg-[#0e1726]/70 backdrop-blur-md border border-slate-800/80 rounded-2xl p-6 shadow-xl flex flex-col gap-5 relative">

    <!-- Header -->
    <div class="flex items-center justify-between border-b border-slate-800 pb-4">
      <div class="flex items-center gap-2">
        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" class="text-amber-400">
          <path d="M12 2L2 7l10 5 10-5-10-5z"/>
          <path d="M2 17l10 5 10-5"/>
          <path d="M2 12l10 5 10-5"/>
        </svg>
        <span class="text-xs font-bold uppercase tracking-wider text-slate-300">
          SOLID Principles Visualizer — Phase 2
        </span>
      </div>
      <div class="flex gap-1.5">
        <span class="text-[10px] font-bold uppercase tracking-wider bg-red-950/40 text-red-400 border border-red-800/40 px-2 py-1 rounded-lg">
          Thermal SRP
        </span>
        <span class="text-[10px] font-bold uppercase tracking-wider bg-amber-950/40 text-amber-400 border border-amber-800/40 px-2 py-1 rounded-lg">
          Laser LSP
        </span>
        <span class="text-[10px] font-bold uppercase tracking-wider bg-emerald-950/40 text-emerald-400 border border-emerald-800/40 px-2 py-1 rounded-lg">
          Neon DIP
        </span>
      </div>
    </div>

    <!-- Lesson Selector Tabs -->
    <div class="flex gap-2">
      <button
        v-for="lesson in lessons"
        :key="lesson.id"
        class="px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-all"
        :class="store.activeLesson === lesson.id
          ? 'bg-slate-700 text-white border border-slate-600'
          : 'bg-slate-900/40 text-slate-500 border border-slate-800/40 hover:text-slate-300 hover:border-slate-700'"
        @click="store.setLesson(lesson.id)"
      >
        {{ lesson.label }}
      </button>
    </div>

    <!-- Active Lesson Panel -->
    <div class="flex-1">
      <!-- SRP Lesson -->
      <SRPLessonPanel
        v-if="store.activeLesson === 'SRP'"
        :class-nodes="store.classNodes"
        :has-overheated="store.hasOverheatedNodes"
        :is-split="store.isSRPSplit"
        :diagnostic-result="store.lastDiagnosticResult"
        @split="onSRPSplit"
      />

      <!-- LSP Lesson -->
      <LSPLessonPanel
        v-else-if="store.activeLesson === 'LSP'"
        :lsp-phase="store.lspPhase"
        :diagnostic-result="store.lastDiagnosticResult"
        @run-violation="store.executeLSPSubstitution(true)"
        @run-valid="store.executeLSPSubstitution(false)"
      />

      <!-- DIP Lesson -->
      <DIPLessonPanel
        v-else-if="store.activeLesson === 'DIP'"
        :is-violating="store.dipState.isViolatingDIP"
        :has-interface="store.dipState.hasInterfaceInserted"
        :diagnostic-result="store.lastDiagnosticResult"
        @insert-interface="store.insertDIPInterface()"
        @reset-d-i-p="store.resetDIP()"
      />

      <!-- Placeholder for OCP/ISP -->
      <div
        v-else
        class="flex items-center justify-center h-40 bg-slate-900/30 rounded-xl border border-slate-800/40"
      >
        <span class="text-xs text-slate-600 font-bold uppercase tracking-wider">
          {{ store.activeLessonLabel }} — Coming Soon
        </span>
      </div>
    </div>

    <!-- Footer Controls -->
    <div class="flex items-center justify-between border-t border-slate-800 pt-4">
      <div class="flex items-center gap-2">
        <span class="w-1.5 h-1.5 rounded-full bg-cyan-500" />
        <span class="text-[10px] text-slate-500 font-medium">
          Bài học: {{ store.activeLessonLabel }}
        </span>
        <span class="text-[10px] text-slate-600">|</span>
        <span class="text-[10px] text-slate-500 font-medium">
          Nodes: {{ store.totalNodes }}
        </span>
      </div>
      <button
        class="px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider
               bg-slate-800/40 text-slate-400 border border-slate-700/40
               hover:bg-slate-700/60 hover:text-slate-200 transition-all"
        @click="store.resetAll()"
      >
        Reset All
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue';
import { useSOLIDVisualizerStore } from '../store/useSOLIDVisualizerStore';
import type { SOLIDPrinciple } from '../types/solid-visualization.types';
import SRPLessonPanel from './SRPLessonPanel.vue';
import LSPLessonPanel from './LSPLessonPanel.vue';
import DIPLessonPanel from './DIPLessonPanel.vue';

const store = useSOLIDVisualizerStore();

interface LessonTab {
  id: SOLIDPrinciple;
  label: string;
}

const lessons: LessonTab[] = [
  { id: 'SRP', label: 'SRP' },
  { id: 'OCP', label: 'OCP' },
  { id: 'LSP', label: 'LSP' },
  { id: 'ISP', label: 'ISP' },
  { id: 'DIP', label: 'DIP' },
];

onMounted(() => {
  store.initializeDemoData();
});

onUnmounted(() => {
  store.destroyStore();
});

function onSRPSplit(nodeId: string): void {
  store.triggerSRPSplit(nodeId);
}
</script>
