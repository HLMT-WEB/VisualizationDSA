<template>
  <div class="call-stack-container">
    <div class="px-3 py-2 border-b border-slate-700/50">
      <div class="flex items-center justify-between">
        <h3 class="text-xs font-semibold text-cyan-400 uppercase tracking-wider">
          Call Stack
        </h3>
        <span class="text-xs text-slate-500">
          {{ stackFrames.length }} / {{ MAX_STACK_FRAMES }} frames
        </span>
      </div>
    </div>

    <div class="flex flex-col-reverse gap-2 p-3 overflow-y-auto max-h-[400px]">
      <div
        v-for="(frame, index) in stackFrames"
        :key="frame.frameId"
        class="stack-frame-card"
        :class="{
          'is-active': frame.isActive,
          'is-inactive': !frame.isActive,
        }"
        @click="$emit('selectFrame', index)"
      >
        <div class="flex items-center justify-between mb-1">
          <span class="text-sm font-mono font-semibold" :class="frame.isActive ? 'text-cyan-300' : 'text-slate-400'">
            {{ frame.functionName }}
          </span>
          <span class="text-[10px] px-1.5 py-0.5 rounded font-mono"
            :class="frame.isActive
              ? 'bg-cyan-500/20 text-cyan-400 border border-cyan-500/30'
              : 'bg-slate-700/50 text-slate-500'"
          >
            L{{ frame.lineNumber }}
          </span>
        </div>

        <div class="space-y-0.5">
          <div
            v-for="(variable, varName) in frame.localVariables"
            :key="varName"
            class="flex items-center justify-between text-xs font-mono px-1.5 py-0.5 rounded transition-all duration-200"
            :class="variable.heapAddress ? 'cursor-pointer hover:bg-amber-500/10' : ''"
            @mouseenter="variable.heapAddress && $emit('hoverVariable', variable.heapAddress)"
            @mouseleave="$emit('hoverVariable', undefined)"
          >
            <span class="text-slate-400">{{ varName }}</span>
            <span class="text-emerald-400">
              {{ variable.value === null ? 'null' : variable.value }}
              <span v-if="variable.heapAddress" class="text-amber-400/70 text-[9px] ml-1">
                → {{ variable.heapAddress }}
              </span>
            </span>
          </div>
        </div>

        <div v-if="frame.isActive" class="mt-1.5">
          <span class="inline-block text-[9px] px-1.5 py-0.5 rounded-full bg-cyan-500/20 text-cyan-400 border border-cyan-500/30 uppercase tracking-wider">
            Active
          </span>
        </div>
      </div>
    </div>

    <div v-if="stackFrames.length === 0" class="p-4 text-center text-slate-600 text-sm italic">
      Stack trống — Chưa có lệnh gọi hàm nào
    </div>
  </div>
</template>

<script setup lang="ts">
import type { StackFrame } from '../types/state-inspector.types';
import { MAX_STACK_FRAMES } from '../types/state-inspector.types';

defineProps<{
  stackFrames: StackFrame[];
}>();

defineEmits<{
  selectFrame: [index: number];
  hoverVariable: [heapAddress: string | undefined];
}>();
</script>

<style scoped>
.call-stack-container {
  background: rgba(10, 15, 30, 0.4);
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 16px;
  backdrop-filter: blur(12px);
  overflow: hidden;
}

.stack-frame-card {
  background: rgba(30, 41, 59, 0.5);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 12px;
  padding: 10px;
  transition: transform 0.3s ease, border-color 0.3s ease, box-shadow 0.3s ease;
  cursor: pointer;
}

.stack-frame-card.is-active {
  border-color: #06B6D4;
  box-shadow: 0 0 15px rgba(6, 182, 212, 0.4);
  transform: translateY(-2px) scale(1.02);
}

.stack-frame-card.is-inactive {
  opacity: 0.65;
  transform: scale(0.96);
}

.stack-frame-card:hover {
  border-color: rgba(6, 182, 212, 0.5);
}
</style>
