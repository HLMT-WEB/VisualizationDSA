<template>
  <div
    class="path-node-circle"
    :class="nodeStateClass"
    :style="{ left: `${x}px`, top: `${y}px` }"
    @click="handleClick"
    :title="node.title"
  >
    <div class="flex flex-col items-center gap-1">
      <span class="text-lg font-bold">{{ nodeIcon }}</span>
      <span class="text-[9px] font-semibold tracking-wider uppercase whitespace-nowrap">
        {{ node.title }}
      </span>
    </div>

    <!-- Status indicator -->
    <div
      v-if="node.status === 'COMPLETED'"
      class="absolute -top-1 -right-1 w-5 h-5 rounded-full bg-emerald-500 flex items-center justify-center text-white text-[10px] font-bold shadow-lg shadow-emerald-500/50"
    >
      ✓
    </div>
    <div
      v-else-if="node.status === 'LOCKED'"
      class="absolute -top-1 -right-1 w-5 h-5 rounded-full bg-slate-600 flex items-center justify-center text-slate-400 text-[10px] font-bold"
    >
      🔒
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { PathNode } from '../types/learning-path.types';

const props = defineProps<{
  node: PathNode;
  x: number;
  y: number;
  isRecommended: boolean;
}>();

const emit = defineEmits<{
  (e: 'node-click', nodeId: string): void;
}>();

const nodeStateClass = computed(() => {
  const classes: string[] = [];

  switch (props.node.status) {
    case 'COMPLETED':
      classes.push('node-state-completed');
      break;
    case 'UNLOCKED':
    case 'IN_PROGRESS':
      classes.push('node-state-active');
      break;
    case 'LOCKED':
      classes.push('node-state-locked');
      break;
  }

  if (props.isRecommended) {
    classes.push('node-recommended');
  }

  return classes;
});

const nodeIcon = computed(() => {
  switch (props.node.status) {
    case 'COMPLETED':
      return '⭐';
    case 'UNLOCKED':
    case 'IN_PROGRESS':
      return '⚡';
    case 'LOCKED':
      return '🔒';
    default:
      return '📚';
  }
});

function handleClick() {
  if (props.node.status !== 'LOCKED') {
    emit('node-click', props.node.id);
  }
}
</script>

<style scoped>
.path-node-circle {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
  font-family: 'JetBrains Mono', monospace;
  font-weight: 700;
  cursor: pointer;
  position: absolute;
  z-index: 10;
  transition: transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);
  transform: translate(-50%, -50%);
}

.path-node-circle:hover {
  transform: translate(-50%, -50%) scale(1.15);
}

.node-state-completed {
  background: rgba(16, 185, 129, 0.15);
  border: 2px solid #10b981;
  box-shadow: 0 0 20px rgba(16, 185, 129, 0.5);
  color: #10b981;
}

.node-state-active {
  background: rgba(6, 182, 212, 0.2);
  border: 2px solid #06b6d4;
  box-shadow: 0 0 25px rgba(6, 182, 212, 0.6);
  color: #06b6d4;
  animation: node-active-breath 2s infinite ease-in-out;
}

@keyframes node-active-breath {
  0%,
  100% {
    box-shadow: 0 0 15px rgba(6, 182, 212, 0.4);
  }
  50% {
    box-shadow: 0 0 30px rgba(6, 182, 212, 0.8);
  }
}

.node-state-locked {
  background: rgba(30, 41, 59, 0.5);
  border: 2px dashed rgba(148, 163, 184, 0.2);
  color: #475569;
  cursor: not-allowed;
}

.node-state-locked:hover {
  transform: translate(-50%, -50%) scale(1);
}

.node-recommended {
  box-shadow: 0 0 30px rgba(245, 158, 11, 0.6) !important;
  border-color: #f59e0b !important;
  animation: recommended-glow 1.5s infinite ease-in-out;
}

@keyframes recommended-glow {
  0%,
  100% {
    box-shadow: 0 0 20px rgba(245, 158, 11, 0.4);
  }
  50% {
    box-shadow: 0 0 40px rgba(245, 158, 11, 0.8);
  }
}
</style>
