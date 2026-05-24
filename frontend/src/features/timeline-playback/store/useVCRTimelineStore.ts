import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { VCRPlaybackEngine } from '../engine/VCRPlaybackEngine';
import type { PlaybackFrame, PlaybackStatus, CanvasStateSnapshot } from '../types/timeline-playback.types';

/**
 * useVCRTimelineStore — Pinia setup store managing VCR timeline state,
 * playback engine lifecycle, Monaco line sync, and canvas state updates.
 */
export const useVCRTimelineStore = defineStore('vcrTimeline', () => {
  // ==========================================
  // STATE
  // ==========================================
  const currentStep = ref<number>(0);
  const totalSteps = ref<number>(0);
  const playbackSpeed = ref<number>(1.0);
  const status = ref<PlaybackStatus>('PAUSED');
  const currentDescription = ref<string>('Chờ nạp giải thuật');
  const currentLineNumber = ref<number>(0);
  const currentSnapshot = ref<CanvasStateSnapshot | null>(null);
  const isInitialized = ref<boolean>(false);

  let engine: VCRPlaybackEngine | null = null;

  // ==========================================
  // COMPUTED
  // ==========================================
  const progressPercent = computed<number>(() => {
    if (totalSteps.value <= 1) return 0;
    return (currentStep.value / (totalSteps.value - 1)) * 100;
  });

  const isPlaying = computed<boolean>(() => status.value === 'PLAYING');
  const isAtStart = computed<boolean>(() => currentStep.value === 0);
  const isAtEnd = computed<boolean>(() => {
    if (totalSteps.value === 0) return true;
    return currentStep.value >= totalSteps.value - 1;
  });

  const stepLabel = computed<string>(() => {
    if (totalSteps.value === 0) return '0 / 0';
    return `${currentStep.value + 1} / ${totalSteps.value}`;
  });

  // ==========================================
  // ACTIONS
  // ==========================================

  function onFrameChange(frame: PlaybackFrame): void {
    currentStep.value = frame.stepIndex;
    currentDescription.value = frame.description;
    currentLineNumber.value = frame.lineNumber;
    currentSnapshot.value = frame.canvasStateSnapshot;

    triggerMonacoLineSync(frame.lineNumber);
    triggerCanvasStateUpdate(frame.canvasStateSnapshot);

    if (engine && engine.getCurrentStep() >= engine.getFrameCount() - 1) {
      status.value = 'PAUSED';
    }
  }

  function initializeFrames(frames: PlaybackFrame[]): void {
    if (engine) {
      engine.destroy();
    }

    totalSteps.value = frames.length;
    currentStep.value = 0;
    status.value = 'PAUSED';
    playbackSpeed.value = 1.0;
    isInitialized.value = true;

    engine = new VCRPlaybackEngine(onFrameChange);
    engine.setFrames(frames);

    if (frames.length > 0) {
      engine.seekToStep(0);
    }
  }

  function play(): void {
    if (engine) {
      engine.play();
      status.value = engine.getStatus();
    }
  }

  function pause(): void {
    if (engine) {
      engine.pause();
      status.value = engine.getStatus();
    }
  }

  function togglePlayPause(): void {
    if (status.value === 'PLAYING') {
      pause();
    } else {
      play();
    }
  }

  function stepForward(): void {
    if (engine) {
      const frame = engine.stepForward();
      if (frame) {
        status.value = engine.getStatus();
      }
    }
  }

  function stepBack(): void {
    if (engine) {
      const frame = engine.stepBack();
      if (frame) {
        status.value = engine.getStatus();
      }
    }
  }

  function rewind(): void {
    if (engine) {
      engine.rewind();
      status.value = engine.getStatus();
    }
  }

  function fastForward(): void {
    if (engine) {
      engine.fastForward();
      status.value = engine.getStatus();
    }
  }

  function seekTo(stepIndex: number): void {
    if (engine) {
      engine.seekToStep(stepIndex);
    }
  }

  function changeSpeed(speed: number): void {
    playbackSpeed.value = speed;
    if (engine) {
      engine.setSpeed(speed);
    }
  }

  function clearTimeline(): void {
    if (engine) {
      engine.destroy();
      engine = null;
    }
    currentStep.value = 0;
    totalSteps.value = 0;
    playbackSpeed.value = 1.0;
    status.value = 'PAUSED';
    currentDescription.value = 'Giải phóng tài nguyên';
    currentLineNumber.value = 0;
    currentSnapshot.value = null;
    isInitialized.value = false;
  }

  // ==========================================
  // DEMO DATA — Bubble Sort 12 steps
  // ==========================================
  function loadDemoBubbleSort(): void {
    const demoFrames: PlaybackFrame[] = [
      {
        stepIndex: 0,
        canvasStateSnapshot: { array: [64, 34, 25, 12, 22, 11, 90], highlights: [{ index: 0, color: '#06B6D4' }] },
        lineNumber: 1,
        description: 'Khởi tạo mảng [64, 34, 25, 12, 22, 11, 90]',
      },
      {
        stepIndex: 1,
        canvasStateSnapshot: { array: [64, 34, 25, 12, 22, 11, 90], highlights: [{ index: 0, color: '#F59E0B' }, { index: 1, color: '#F59E0B' }] },
        lineNumber: 3,
        description: 'So sánh arr[0]=64 với arr[1]=34',
      },
      {
        stepIndex: 2,
        canvasStateSnapshot: { array: [34, 64, 25, 12, 22, 11, 90], highlights: [{ index: 0, color: '#10B981' }, { index: 1, color: '#10B981' }] },
        lineNumber: 4,
        description: 'Hoán vị 64 ↔ 34',
      },
      {
        stepIndex: 3,
        canvasStateSnapshot: { array: [34, 64, 25, 12, 22, 11, 90], highlights: [{ index: 1, color: '#F59E0B' }, { index: 2, color: '#F59E0B' }] },
        lineNumber: 3,
        description: 'So sánh arr[1]=64 với arr[2]=25',
      },
      {
        stepIndex: 4,
        canvasStateSnapshot: { array: [34, 25, 64, 12, 22, 11, 90], highlights: [{ index: 1, color: '#10B981' }, { index: 2, color: '#10B981' }] },
        lineNumber: 4,
        description: 'Hoán vị 64 ↔ 25',
      },
      {
        stepIndex: 5,
        canvasStateSnapshot: { array: [34, 25, 64, 12, 22, 11, 90], highlights: [{ index: 2, color: '#F59E0B' }, { index: 3, color: '#F59E0B' }] },
        lineNumber: 3,
        description: 'So sánh arr[2]=64 với arr[3]=12',
      },
      {
        stepIndex: 6,
        canvasStateSnapshot: { array: [34, 25, 12, 64, 22, 11, 90], highlights: [{ index: 2, color: '#10B981' }, { index: 3, color: '#10B981' }] },
        lineNumber: 4,
        description: 'Hoán vị 64 ↔ 12',
      },
      {
        stepIndex: 7,
        canvasStateSnapshot: { array: [34, 25, 12, 64, 22, 11, 90], highlights: [{ index: 3, color: '#F59E0B' }, { index: 4, color: '#F59E0B' }] },
        lineNumber: 3,
        description: 'So sánh arr[3]=64 với arr[4]=22',
      },
      {
        stepIndex: 8,
        canvasStateSnapshot: { array: [34, 25, 12, 22, 64, 11, 90], highlights: [{ index: 3, color: '#10B981' }, { index: 4, color: '#10B981' }] },
        lineNumber: 4,
        description: 'Hoán vị 64 ↔ 22',
      },
      {
        stepIndex: 9,
        canvasStateSnapshot: { array: [34, 25, 12, 22, 64, 11, 90], highlights: [{ index: 4, color: '#F59E0B' }, { index: 5, color: '#F59E0B' }] },
        lineNumber: 3,
        description: 'So sánh arr[4]=64 với arr[5]=11',
      },
      {
        stepIndex: 10,
        canvasStateSnapshot: { array: [34, 25, 12, 22, 11, 64, 90], highlights: [{ index: 4, color: '#10B981' }, { index: 5, color: '#10B981' }] },
        lineNumber: 4,
        description: 'Hoán vị 64 ↔ 11',
      },
      {
        stepIndex: 11,
        canvasStateSnapshot: { array: [34, 25, 12, 22, 11, 64, 90], highlights: [{ index: 5, color: '#F59E0B' }, { index: 6, color: '#F59E0B' }] },
        lineNumber: 3,
        description: 'So sánh arr[5]=64 với arr[6]=90 — không hoán vị, kết thúc vòng lặp 1',
      },
    ];

    initializeFrames(demoFrames);
  }

  // ==========================================
  // UTILS
  // ==========================================
  function triggerMonacoLineSync(lineNumber: number): void {
    if (typeof window !== 'undefined' && typeof CustomEvent !== 'undefined') {
      const syncEvent = new CustomEvent('MONACO_REVEAL_LINE_INSIGHT', {
        detail: { lineNumber },
      });
      window.dispatchEvent(syncEvent);
    }
  }

  function triggerCanvasStateUpdate(snapshot: CanvasStateSnapshot): void {
    if (typeof window !== 'undefined' && typeof CustomEvent !== 'undefined') {
      const syncEvent = new CustomEvent('CANVAS_REDRAW_STATE_SNAPSHOT', {
        detail: { snapshot },
      });
      window.dispatchEvent(syncEvent);
    }
  }

  return {
    // State
    currentStep,
    totalSteps,
    playbackSpeed,
    status,
    currentDescription,
    currentLineNumber,
    currentSnapshot,
    isInitialized,
    // Computed
    progressPercent,
    isPlaying,
    isAtStart,
    isAtEnd,
    stepLabel,
    // Actions
    initializeFrames,
    play,
    pause,
    togglePlayPause,
    stepForward,
    stepBack,
    rewind,
    fastForward,
    seekTo,
    changeSpeed,
    clearTimeline,
    loadDemoBubbleSort,
  };
});
