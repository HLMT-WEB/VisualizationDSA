<template>
  <div class="vcr-controller-panel">
    <!-- Rewind -->
    <button
      class="vcr-button"
      :disabled="store.isAtStart"
      :title="'Về đầu'"
      @click="store.rewind()"
    >
      <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
        <path d="M6 6h2v12H6zm3.5 6l8.5 6V6z" transform="scale(-1,1) translate(-24,0)" />
      </svg>
    </button>

    <!-- Step Back -->
    <button
      class="vcr-button"
      :disabled="store.isAtStart"
      :title="'Lùi 1 bước'"
      @click="onStepBack"
    >
      <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
        <path d="M6 6h2v12H6zm3.5 6l8.5 6V6z" transform="scale(-1,1) translate(-24,0)" />
      </svg>
    </button>

    <!-- Play / Pause -->
    <button
      class="vcr-button vcr-button-primary"
      :title="store.isPlaying ? 'Tạm dừng' : 'Phát'"
      @click="store.togglePlayPause()"
    >
      <svg v-if="!store.isPlaying" width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
        <path d="M8 5v14l11-7z" />
      </svg>
      <svg v-else width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
        <path d="M6 19h4V5H6v14zm8-14v14h4V5h-4z" />
      </svg>
    </button>

    <!-- Step Forward -->
    <button
      class="vcr-button"
      :disabled="store.isAtEnd"
      :title="'Tiến 1 bước'"
      @click="onStepForward"
    >
      <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
        <path d="M6 18l8.5-6L6 6v12zM16 6v12h2V6h-2z" />
      </svg>
    </button>

    <!-- Fast Forward -->
    <button
      class="vcr-button"
      :disabled="store.isAtEnd"
      :title="'Đến cuối'"
      @click="store.fastForward()"
    >
      <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
        <path d="M4 18l8.5-6L4 6v12zm9-12v12l8.5-6L13 6z" />
      </svg>
    </button>

    <!-- Step Counter Badge -->
    <span class="vcr-step-badge">{{ store.stepLabel }}</span>
  </div>
</template>

<script setup lang="ts">
import { useVCRTimelineStore } from '../store/useVCRTimelineStore';
import { STEP_DEBOUNCE_MS } from '../types/timeline-playback.types';

const store = useVCRTimelineStore();

let lastStepTime = 0;

function debounceStep(action: () => void): void {
  const now = Date.now();
  if (now - lastStepTime < STEP_DEBOUNCE_MS) return;
  lastStepTime = now;
  action();
}

function onStepBack(): void {
  debounceStep(() => store.stepBack());
}

function onStepForward(): void {
  debounceStep(() => store.stepForward());
}
</script>

<style scoped>
.vcr-controller-panel {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px 20px;
  background: rgba(15, 23, 42, 0.45);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 9999px;
  backdrop-filter: blur(16px);
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.4);
  width: fit-content;
  margin: 0 auto;
}

.vcr-button {
  background: transparent;
  border: none;
  color: #94A3B8;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
  cursor: pointer;
}

.vcr-button:hover:not(:disabled) {
  color: #06B6D4;
  background: rgba(6, 182, 212, 0.1);
  transform: scale(1.12);
  box-shadow: 0 0 12px rgba(6, 182, 212, 0.2);
}

.vcr-button:active:not(:disabled) {
  transform: scale(0.92);
}

.vcr-button:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.vcr-button-primary {
  width: 48px;
  height: 48px;
  background: rgba(6, 182, 212, 0.15);
  color: #06B6D4;
  border: 1px solid rgba(6, 182, 212, 0.3);
}

.vcr-button-primary:hover {
  background: rgba(6, 182, 212, 0.25);
  color: #22D3EE;
  box-shadow: 0 0 20px rgba(6, 182, 212, 0.4);
}

.vcr-step-badge {
  font-size: 12px;
  font-weight: 600;
  color: #94A3B8;
  font-family: 'JetBrains Mono', monospace;
  min-width: 60px;
  text-align: center;
}
</style>
