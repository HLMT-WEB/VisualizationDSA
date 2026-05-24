<template>
  <div
    class="solid-class-card relative"
    :class="{ 'is-overheated': isOverheated, 'is-cool': isCool }"
  >
    <!-- Canvas behind card for thermal sparks -->
    <canvas
      v-if="isOverheated"
      ref="sparkCanvas"
      class="thermal-spark-canvas"
    />

    <!-- Card Content -->
    <div class="relative z-10">
      <!-- Class Name Header -->
      <div class="flex items-center justify-between mb-3">
        <div class="flex items-center gap-2">
          <span class="w-2 h-2 rounded-full" :class="statusDotClass" />
          <span class="text-sm font-bold text-white">{{ classNode.className }}</span>
        </div>
        <span
          class="text-[10px] font-bold uppercase tracking-wider px-2 py-0.5 rounded-lg"
          :class="cohesionBadgeClass"
        >
          LCOM4: {{ classNode.cohesionScore }}
        </span>
      </div>

      <!-- Members List -->
      <div class="space-y-1.5">
        <div
          v-for="member in classNode.members"
          :key="member.name"
          class="flex items-center gap-2 text-[11px] font-mono"
        >
          <span
            class="w-4 h-4 rounded flex items-center justify-center text-[9px] font-bold"
            :class="member.type === 'FIELD' ? 'bg-amber-950/50 text-amber-400 border border-amber-800/40' : 'bg-cyan-950/50 text-cyan-400 border border-cyan-800/40'"
          >
            {{ member.type === 'FIELD' ? 'F' : 'M' }}
          </span>
          <span class="text-slate-300">{{ member.name }}</span>
          <span v-if="member.accessedFields.length > 0" class="text-slate-600 text-[9px]">
            → {{ member.accessedFields.join(', ') }}
          </span>
        </div>
      </div>

      <!-- Split Button (only for overheated SRP cards) -->
      <button
        v-if="isOverheated && showSplitButton"
        class="mt-4 w-full py-2 rounded-lg text-xs font-bold uppercase tracking-wider
               bg-emerald-950/40 text-emerald-400 border border-emerald-800/40
               hover:bg-emerald-900/60 hover:border-emerald-600/60 transition-all"
        @click="$emit('split', classNode.nodeId)"
      >
        SPLIT CLASS
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onMounted, onUnmounted, computed } from 'vue';
import type { SOLIDClassNode } from '../types/solid-visualization.types';
import { ThermalSparkParticleEngine } from '../engine/ThermalSparkParticleEngine';

const props = defineProps<{
  classNode: SOLIDClassNode;
  isOverheated: boolean;
  isCool?: boolean;
  showSplitButton?: boolean;
}>();

defineEmits<{
  split: [nodeId: string];
}>();

const sparkCanvas = ref<HTMLCanvasElement | null>(null);
let particleEngine: ThermalSparkParticleEngine | null = null;

const statusDotClass = computed(() =>
  props.isOverheated
    ? 'bg-red-500 animate-pulse'
    : 'bg-emerald-500'
);

const cohesionBadgeClass = computed(() =>
  props.isOverheated
    ? 'bg-red-950/50 text-red-400 border border-red-800/40'
    : 'bg-emerald-950/50 text-emerald-400 border border-emerald-800/40'
);

watch(
  () => props.isOverheated,
  (hot) => {
    if (hot && sparkCanvas.value) {
      startParticles();
    } else {
      stopParticles();
    }
  }
);

onMounted(() => {
  if (props.isOverheated && sparkCanvas.value) {
    startParticles();
  }
});

onUnmounted(() => {
  stopParticles();
});

function startParticles(): void {
  if (!sparkCanvas.value) return;
  if (particleEngine) particleEngine.destroy();

  sparkCanvas.value.width = sparkCanvas.value.offsetWidth;
  sparkCanvas.value.height = sparkCanvas.value.offsetHeight;

  particleEngine = new ThermalSparkParticleEngine(sparkCanvas.value);
  particleEngine.start();
}

function stopParticles(): void {
  if (particleEngine) {
    particleEngine.destroy();
    particleEngine = null;
  }
}
</script>

<style scoped>
.solid-class-card {
  width: 260px;
  background: rgba(15, 23, 42, 0.5);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 20px;
  padding: 20px;
  backdrop-filter: blur(16px);
  position: relative;
  transition: border-color 0.4s ease, box-shadow 0.4s ease;
}

.solid-class-card.is-overheated {
  border-color: #EF4444 !important;
  box-shadow: 0 0 30px rgba(239, 68, 68, 0.6), inset 0 0 15px rgba(239, 68, 68, 0.2);
  animation: thermal-glow 1.5s infinite alternate ease-in-out;
}

.solid-class-card.is-cool {
  border-color: #10B981 !important;
  box-shadow: 0 0 20px rgba(16, 185, 129, 0.3), inset 0 0 10px rgba(16, 185, 129, 0.1);
}

@keyframes thermal-glow {
  0% { filter: drop-shadow(0 0 5px rgba(239, 68, 68, 0.4)); }
  100% { filter: drop-shadow(0 0 20px rgba(239, 68, 68, 0.8)); }
}

.thermal-spark-canvas {
  position: absolute;
  top: -10px;
  left: -10px;
  width: calc(100% + 20px);
  height: calc(100% + 20px);
  pointer-events: none;
  z-index: 1;
}
</style>
