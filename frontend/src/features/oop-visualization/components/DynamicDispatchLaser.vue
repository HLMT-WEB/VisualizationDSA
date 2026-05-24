<template>
  <svg
    v-if="isActive"
    class="dynamic-dispatch-laser-overlay"
    xmlns="http://www.w3.org/2000/svg"
  >
    <!-- Seeking phase: source → VTable pivot -->
    <path
      v-if="showSeekingPath"
      :d="seekingPathD"
      class="laser-line"
      :class="seekingPhaseClass"
    />

    <!-- Dispatch phase: VTable pivot → target method -->
    <path
      v-if="showDispatchPath"
      :d="dispatchPathD"
      class="laser-line laser-dispatch"
      :class="dispatchPhaseClass"
    />

    <!-- Glow dot at VTable pivot -->
    <circle
      v-if="showSeekingPath"
      :cx="vTablePivot.x"
      :cy="vTablePivot.y"
      r="5"
      class="laser-pivot-dot"
    />

    <!-- Glow dot at target -->
    <circle
      v-if="showDispatchPath"
      :cx="target.x"
      :cy="target.y"
      r="6"
      class="laser-target-dot"
      :class="isOverridden ? 'dot-override' : 'dot-inherited'"
    />
  </svg>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { SVGLaserBatchRenderer } from '../engine/SVGLaserBatchRenderer';
import type { CoordinatePoint } from '../types/oop-visualization.types';

const props = defineProps<{
  isActive: boolean;
  source: CoordinatePoint;
  vTablePivot: CoordinatePoint;
  target: CoordinatePoint;
  phase: 'seeking' | 'resolved';
  isOverridden?: boolean;
}>();

const showSeekingPath = computed(() => props.isActive);
const showDispatchPath = computed(
  () => props.isActive && props.phase === 'resolved'
);

const seekingPathD = computed(() =>
  SVGLaserBatchRenderer.calculateLaserPath(props.source, props.vTablePivot)
);

const dispatchPathD = computed(() =>
  SVGLaserBatchRenderer.calculateLaserPath(props.vTablePivot, props.target)
);

const seekingPhaseClass = computed(() =>
  props.phase === 'seeking' ? 'laser-seeking' : 'laser-seeked'
);

const dispatchPhaseClass = computed(() =>
  props.isOverridden ? 'laser-override' : 'laser-inherited'
);
</script>

<style scoped>
.dynamic-dispatch-laser-overlay {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
  z-index: 50;
  overflow: visible;
}

.laser-line {
  fill: none;
  stroke-width: 3;
  stroke-linecap: round;
  stroke-dasharray: 8 12;
}

.laser-seeking {
  stroke: #10b981;
  filter: drop-shadow(0 0 6px rgba(16, 185, 129, 0.7));
  animation: laser-flow 1.2s infinite linear;
}

.laser-seeked {
  stroke: #10b981;
  filter: drop-shadow(0 0 4px rgba(16, 185, 129, 0.4));
  stroke-dasharray: none;
  opacity: 0.5;
}

.laser-override {
  stroke: #10b981;
  filter: drop-shadow(0 0 8px rgba(16, 185, 129, 0.8));
  animation: laser-flow 0.8s infinite linear;
}

.laser-inherited {
  stroke: #f59e0b;
  filter: drop-shadow(0 0 8px rgba(245, 158, 11, 0.8));
  animation: laser-flow 0.8s infinite linear;
}

.laser-pivot-dot {
  fill: #06b6d4;
  filter: drop-shadow(0 0 6px rgba(6, 182, 212, 0.8));
  animation: dot-pulse 1s infinite ease-in-out;
}

.laser-target-dot {
  filter: drop-shadow(0 0 8px currentColor);
  animation: dot-pulse 0.8s infinite ease-in-out;
}

.dot-override {
  fill: #10b981;
}

.dot-inherited {
  fill: #f59e0b;
}

@keyframes laser-flow {
  to {
    stroke-dashoffset: -30;
  }
}

@keyframes dot-pulse {
  0%,
  100% {
    r: 5;
    opacity: 1;
  }
  50% {
    r: 8;
    opacity: 0.6;
  }
}
</style>
