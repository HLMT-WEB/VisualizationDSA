<template>
  <div class="canvas-preview-panel">
    <div class="canvas-header">
      <div class="canvas-header-dot"></div>
      <span class="canvas-header-title">Canvas Preview</span>
      <span class="canvas-header-badge" v-if="store.isInitialized">
        {{ store.currentSnapshot?.array?.length ?? 0 }} phần tử
      </span>
    </div>

    <div class="canvas-body">
      <svg
        v-if="store.currentSnapshot"
        :viewBox="`0 0 ${svgWidth} ${svgHeight}`"
        class="canvas-svg"
        preserveAspectRatio="xMidYMid meet"
      >
        <rect
          v-for="(bar, idx) in bars"
          :key="idx"
          :x="bar.x"
          :y="bar.y"
          :width="bar.width"
          :height="bar.height"
          :fill="bar.color"
          rx="3"
          class="canvas-bar"
        />
        <text
          v-for="(bar, idx) in bars"
          :key="'t-' + idx"
          :x="bar.x + bar.width / 2"
          :y="bar.y - 6"
          text-anchor="middle"
          fill="#CBD5E1"
          font-size="11"
          font-family="JetBrains Mono, monospace"
        >
          {{ store.currentSnapshot!.array[idx] }}
        </text>
      </svg>

      <div v-else class="canvas-empty">
        <span class="canvas-empty-text">Chưa nạp dữ liệu giải thuật</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useVCRTimelineStore } from '../store/useVCRTimelineStore';

const store = useVCRTimelineStore();

const svgWidth = 500;
const svgHeight = 250;
const barGap = 8;
const topPadding = 30;

interface BarData {
  x: number;
  y: number;
  width: number;
  height: number;
  color: string;
}

const bars = computed<BarData[]>(() => {
  const snapshot = store.currentSnapshot;
  if (!snapshot || !snapshot.array || snapshot.array.length === 0) return [];

  const arr = snapshot.array;
  const maxVal = Math.max(...arr, 1);
  const count = arr.length;
  const barWidth = (svgWidth - barGap * (count + 1)) / count;
  const maxBarHeight = svgHeight - topPadding - 10;

  return arr.map((val, idx) => {
    const height = (val / maxVal) * maxBarHeight;
    const x = barGap + idx * (barWidth + barGap);
    const y = svgHeight - height - 5;

    let color = '#334155';
    if (snapshot.highlights) {
      const hl = snapshot.highlights.find(h => h.index === idx);
      if (hl) color = hl.color;
    }

    return { x, y, width: barWidth, height, color };
  });
});
</script>

<style scoped>
.canvas-preview-panel {
  background: rgba(15, 23, 42, 0.5);
  border: 1px solid rgba(255, 255, 255, 0.06);
  border-radius: 12px;
  backdrop-filter: blur(12px);
  overflow: hidden;
  flex: 1;
}

.canvas-header {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
}

.canvas-header-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #06B6D4;
}

.canvas-header-title {
  font-size: 12px;
  font-weight: 600;
  color: #E2E8F0;
  flex: 1;
}

.canvas-header-badge {
  font-size: 10px;
  color: #06B6D4;
  background: rgba(6, 182, 212, 0.1);
  padding: 2px 8px;
  border-radius: 9999px;
  font-family: 'JetBrains Mono', monospace;
}

.canvas-body {
  padding: 16px;
  min-height: 200px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.canvas-svg {
  width: 100%;
  max-height: 220px;
}

.canvas-bar {
  transition: all 0.15s ease;
}

.canvas-empty {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 180px;
}

.canvas-empty-text {
  font-size: 13px;
  color: #475569;
}
</style>
