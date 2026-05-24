<template>
  <div class="sandbox bg-[#070b13]/60 border border-slate-800 rounded-xl p-4">
    <!-- Header -->
    <div class="flex items-center justify-between mb-4">
      <div class="flex items-center gap-2">
        <svg class="w-4 h-4 text-purple-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <polygon points="12 2 2 7 12 12 22 7 12 2"/>
          <polyline points="2 17 12 22 22 17"/>
          <polyline points="2 12 12 17 22 12"/>
        </svg>
        <span class="text-[11px] font-bold uppercase tracking-wider text-slate-400">
          Polymorphism Sandbox
        </span>
      </div>
    </div>

    <!-- Controls Row -->
    <div class="flex flex-wrap gap-3 mb-4">
      <!-- Class Selector -->
      <div class="flex items-center gap-2">
        <span class="text-[10px] text-slate-500 font-bold uppercase">Lớp:</span>
        <div class="flex gap-1">
          <button
            v-for="className in availableClasses"
            :key="className"
            class="px-3 py-1.5 text-[10px] font-bold rounded-lg transition-all border"
            :class="selectedClass === className
              ? 'bg-purple-950/50 border-purple-700/50 text-purple-400'
              : 'bg-slate-900/50 border-slate-800 text-slate-500 hover:text-slate-300 hover:border-slate-700'"
            @click="$emit('select-class', className)"
          >
            {{ className }}
          </button>
        </div>
      </div>

      <!-- Instantiate Button -->
      <button
        class="px-3 py-1.5 text-[10px] font-bold rounded-lg transition-all border"
        :class="canAllocate
          ? 'bg-emerald-950/50 border-emerald-700/40 text-emerald-400 hover:bg-emerald-900/50'
          : 'bg-slate-900/50 border-slate-800 text-slate-600 cursor-not-allowed'"
        :disabled="!canAllocate"
        @click="$emit('instantiate', selectedClass)"
      >
        + new {{ selectedClass }}()
      </button>

      <!-- Reset Button -->
      <button
        class="px-3 py-1.5 text-[10px] font-bold rounded-lg bg-rose-950/30 border border-rose-800/30 text-rose-400 hover:bg-rose-900/40 transition-all ml-auto"
        @click="$emit('reset')"
      >
        Reset All
      </button>
    </div>

    <!-- VTable Dispatch Map -->
    <div v-if="vTableEntries.length > 0" class="mb-4">
      <div class="text-[10px] font-bold uppercase text-slate-500 mb-2">
        VTable Dispatch Map — {{ selectedClass }}
      </div>
      <div class="space-y-1.5">
        <button
          v-for="entry in vTableEntries"
          :key="entry.methodName"
          class="w-full flex items-center justify-between p-2.5 rounded-lg border text-xs transition-all text-left"
          :class="isMethodActive(entry.methodName)
            ? 'bg-purple-900/30 border-purple-700/40'
            : 'bg-slate-900/50 border-slate-800 hover:border-slate-700'"
          @click="onDispatchMethod(entry.methodName)"
        >
          <div class="flex items-center gap-2">
            <span class="text-slate-400 font-mono">{{ entry.methodName }}()</span>
            <span class="text-[10px] text-slate-600">→</span>
            <span class="font-bold" :class="getClassTextColor(entry.resolvedClass)">
              {{ entry.resolvedClass }}.{{ entry.methodName }}()
            </span>
          </div>
          <span
            class="text-[9px] px-1.5 py-0.5 rounded"
            :class="entry.isOverridden
              ? 'bg-emerald-950/50 text-emerald-400 border border-emerald-800/30'
              : 'bg-slate-800 text-slate-500'"
          >
            {{ entry.isOverridden ? 'override' : 'inherited' }}
          </span>
        </button>
      </div>
    </div>

    <!-- Dispatch Status -->
    <div
      v-if="dispatchStatus !== 'IDLE'"
      class="dispatch-status p-3 rounded-lg border text-xs"
      :class="statusClass"
    >
      <div class="flex items-center gap-2">
        <span v-if="dispatchStatus === 'SEEKING_VTABLE'" class="status-dot seeking"></span>
        <span v-else-if="dispatchStatus === 'DISPATCHED'" class="status-dot dispatched"></span>
        <span v-else-if="dispatchStatus === 'ACCESS_VIOLATED'" class="status-dot violated"></span>
        <span class="font-bold">{{ statusLabel }}</span>
      </div>
      <div v-if="resolvedClass" class="mt-1 text-slate-400">
        Phương thức được định tuyến sang: <span class="font-bold" :class="getClassTextColor(resolvedClass)">{{ resolvedClass }}</span>
      </div>
    </div>

    <!-- Encapsulation Violation Alert -->
    <div
      v-if="violation"
      class="mt-3 p-3 bg-red-950/30 border border-red-700/40 rounded-lg"
    >
      <div class="flex items-center gap-2 text-red-400 text-xs">
        <svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <circle cx="12" cy="12" r="10"/>
          <line x1="12" x2="12" y1="8" y2="12"/>
          <line x1="12" x2="12.01" y1="16" y2="16"/>
        </svg>
        <span class="font-bold">VI PHẠM ĐÓNG GÓI!</span>
      </div>
      <p class="text-[11px] text-red-300 mt-1 font-mono">{{ violation.errorMessage }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { DispatchStatus, EncapsulationViolation } from '../types/oop-visualization.types';

const props = defineProps<{
  availableClasses: string[];
  selectedClass: string;
  canAllocate: boolean;
  vTableEntries: Array<{ methodName: string; resolvedClass: string; isOverridden: boolean }>;
  dispatchStatus: DispatchStatus;
  resolvedClass?: string;
  activeMethod?: string | null;
  violation?: EncapsulationViolation | null;
}>();

const emit = defineEmits<{
  (e: 'select-class', className: string): void;
  (e: 'instantiate', className: string): void;
  (e: 'dispatch', methodName: string): void;
  (e: 'reset'): void;
}>();

const statusClass = computed(() => {
  switch (props.dispatchStatus) {
    case 'SEEKING_VTABLE':
      return 'bg-cyan-950/30 border-cyan-700/40';
    case 'DISPATCHED':
      return 'bg-emerald-950/30 border-emerald-700/40';
    case 'ACCESS_VIOLATED':
      return 'bg-red-950/30 border-red-700/40';
    default:
      return 'bg-slate-900/50 border-slate-800';
  }
});

const statusLabel = computed(() => {
  switch (props.dispatchStatus) {
    case 'SEEKING_VTABLE':
      return 'Đang tra cứu VTable...';
    case 'DISPATCHED':
      return 'Dynamic Dispatch thành công!';
    case 'ACCESS_VIOLATED':
      return 'Vi phạm đóng gói!';
    default:
      return '';
  }
});

function isMethodActive(methodName: string): boolean {
  return props.activeMethod?.endsWith(`.${methodName}`) ?? false;
}

function getClassTextColor(className: string): string {
  switch (className) {
    case 'Shape':
      return 'text-emerald-400';
    case 'Circle':
      return 'text-purple-400';
    case 'Rectangle':
      return 'text-cyan-400';
    default:
      return 'text-slate-300';
  }
}

function onDispatchMethod(methodName: string): void {
  emit('dispatch', methodName);
}
</script>

<style scoped>
.status-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  display: inline-block;
}

.status-dot.seeking {
  background: #06b6d4;
  animation: dot-blink 0.6s infinite;
}

.status-dot.dispatched {
  background: #10b981;
  box-shadow: 0 0 6px rgba(16, 185, 129, 0.6);
}

.status-dot.violated {
  background: #ef4444;
  animation: dot-blink 0.3s infinite;
}

@keyframes dot-blink {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.3; }
}
</style>
