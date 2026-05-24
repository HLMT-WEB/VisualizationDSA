// ============================================================
// useSOLIDVisualizerStore — Pinia Setup Store
// Orchestrates SOLID principle lessons: SRP thermal cards,
// LSP laser fracture, DIP neon flowing path direction
// ============================================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { SOLIDEvaluatorEngine } from '../engine/SOLIDEvaluatorEngine';
import type {
  SOLIDPrinciple,
  SOLIDClassNode,
  LSPSubstitutionPhase,
  DIPState,
} from '../types/solid-visualization.types';
import {
  LSP_LASER_DELAY_MS,
  COOL_DOWN_CONFETTI_EVENT,
  GLASS_BREAK_SOUND_EVENT,
} from '../types/solid-visualization.types';

// ==========================================
// DEMO DATA: UserManager God Class (SRP)
// ==========================================
const DEMO_SRP_GOD_CLASS: SOLIDClassNode = {
  nodeId: 'user-manager-node',
  className: 'UserManager',
  members: [
    { name: 'dbConn', type: 'FIELD', accessedFields: [] },
    { name: 'hasher', type: 'FIELD', accessedFields: [] },
    { name: 'smtpServer', type: 'FIELD', accessedFields: [] },
    { name: 'saveUser', type: 'METHOD', accessedFields: ['dbConn'] },
    { name: 'findUser', type: 'METHOD', accessedFields: ['dbConn'] },
    { name: 'hashPassword', type: 'METHOD', accessedFields: ['hasher'] },
    { name: 'sendWelcomeEmail', type: 'METHOD', accessedFields: ['smtpServer'] },
  ],
  cohesionScore: 0,
  isViolatingSRP: false,
};

const DEMO_SRP_SPLIT_NODES: SOLIDClassNode[] = [
  {
    nodeId: 'user-repo-node',
    className: 'UserRepository',
    members: [
      { name: 'dbConn', type: 'FIELD', accessedFields: [] },
      { name: 'saveUser', type: 'METHOD', accessedFields: ['dbConn'] },
      { name: 'findUser', type: 'METHOD', accessedFields: ['dbConn'] },
    ],
    cohesionScore: 1,
    isViolatingSRP: false,
  },
  {
    nodeId: 'password-hasher-node',
    className: 'PasswordHasher',
    members: [
      { name: 'hasher', type: 'FIELD', accessedFields: [] },
      { name: 'hashPassword', type: 'METHOD', accessedFields: ['hasher'] },
    ],
    cohesionScore: 1,
    isViolatingSRP: false,
  },
  {
    nodeId: 'email-notifier-node',
    className: 'EmailNotifier',
    members: [
      { name: 'smtpServer', type: 'FIELD', accessedFields: [] },
      { name: 'sendWelcomeEmail', type: 'METHOD', accessedFields: ['smtpServer'] },
    ],
    cohesionScore: 1,
    isViolatingSRP: false,
  },
];

export const useSOLIDVisualizerStore = defineStore('solidVisualizer', () => {
  // ==========================================
  // STATE
  // ==========================================
  const activeLesson = ref<SOLIDPrinciple>('SRP');
  const classNodes = ref<SOLIDClassNode[]>([]);
  const isSRPSplit = ref(false);

  const lspPhase = ref<LSPSubstitutionPhase>('IDLE');
  const isLspShattered = ref(false);

  const dipState = ref<DIPState>({
    isViolatingDIP: true,
    hasInterfaceInserted: false,
  });

  const lastDiagnosticResult = ref<string | null>(null);
  const lspTimerId = ref<ReturnType<typeof setTimeout> | null>(null);

  // ==========================================
  // COMPUTED
  // ==========================================
  const hasOverheatedNodes = computed(() =>
    classNodes.value.some((n) => n.isViolatingSRP)
  );

  const overheatedNodeIds = computed(() =>
    classNodes.value.filter((n) => n.isViolatingSRP).map((n) => n.nodeId)
  );

  const totalNodes = computed(() => classNodes.value.length);

  const srpViolationCount = computed(
    () => classNodes.value.filter((n) => n.isViolatingSRP).length
  );

  const isLSPTransmitting = computed(
    () => lspPhase.value === 'TRANSMITTING'
  );

  const isDIPCorrect = computed(
    () => !dipState.value.isViolatingDIP && dipState.value.hasInterfaceInserted
  );

  const activeLessonLabel = computed(() => {
    const labels: Record<SOLIDPrinciple, string> = {
      SRP: 'Single Responsibility',
      OCP: 'Open/Closed',
      LSP: 'Liskov Substitution',
      ISP: 'Interface Segregation',
      DIP: 'Dependency Inversion',
    };
    return labels[activeLesson.value];
  });

  // ==========================================
  // ACTIONS
  // ==========================================

  function setLesson(lesson: SOLIDPrinciple): void {
    activeLesson.value = lesson;
    resetState();
    initializeDemoData();
  }

  function initializeDemoData(): void {
    if (activeLesson.value === 'SRP') {
      initializeSRPDemo();
    }
  }

  function initializeSRPDemo(): void {
    const godClass = { ...DEMO_SRP_GOD_CLASS };
    const evaluation = SOLIDEvaluatorEngine.evaluateSRP(godClass);
    godClass.cohesionScore = evaluation.lcom4;
    godClass.isViolatingSRP = evaluation.isViolating;
    classNodes.value = [godClass];
    isSRPSplit.value = false;
  }

  function initializeClassNodes(nodes: SOLIDClassNode[]): void {
    classNodes.value = nodes.map((node) => {
      const res = SOLIDEvaluatorEngine.evaluateSRP(node);
      return {
        ...node,
        cohesionScore: res.lcom4,
        isViolatingSRP: res.isViolating,
      };
    });
  }

  function triggerSRPSplit(nodeId: string): void {
    const targetIndex = classNodes.value.findIndex((n) => n.nodeId === nodeId);
    if (targetIndex === -1) return;

    classNodes.value.splice(
      targetIndex,
      1,
      ...DEMO_SRP_SPLIT_NODES.map((node) => ({
        ...node,
        cohesionScore: 1,
        isViolatingSRP: false,
      }))
    );

    isSRPSplit.value = true;
    lastDiagnosticResult.value =
      'SRP ĐẠT: Tái cấu trúc thành công! Mỗi lớp giờ chỉ gánh 1 nhiệm vụ duy nhất.';

    triggerCoolDownConfetti();
  }

  function executeLSPSubstitution(throwsException: boolean): void {
    isLspShattered.value = false;
    lspPhase.value = 'TRANSMITTING';
    lastDiagnosticResult.value = null;

    const res = SOLIDEvaluatorEngine.evaluateLSP('fly', throwsException);

    if (res.isViolating) {
      lspTimerId.value = setTimeout(() => {
        isLspShattered.value = true;
        lspPhase.value = 'SHATTERED';
        lastDiagnosticResult.value = res.errorReason ?? 'Vi phạm LSP.';
        triggerGlassBreakSound();
      }, LSP_LASER_DELAY_MS);
    } else {
      lspPhase.value = 'PASSED';
      isLspShattered.value = false;
      lastDiagnosticResult.value =
        'LSP ĐẠT: Thay thế đối tượng con hoạt động hoàn hảo!';
    }
  }

  function insertDIPInterface(): void {
    dipState.value = {
      isViolatingDIP: false,
      hasInterfaceInserted: true,
    };
    lastDiagnosticResult.value =
      'DIP ĐẠT: Luồng phụ thuộc đã đảo chiều hội tụ về phía Interface trừu tượng!';
  }

  function resetDIP(): void {
    dipState.value = {
      isViolatingDIP: true,
      hasInterfaceInserted: false,
    };
    lastDiagnosticResult.value = null;
  }

  function resetState(): void {
    classNodes.value = [];
    isSRPSplit.value = false;
    isLspShattered.value = false;
    lspPhase.value = 'IDLE';
    dipState.value = {
      isViolatingDIP: true,
      hasInterfaceInserted: false,
    };
    lastDiagnosticResult.value = null;

    if (lspTimerId.value !== null) {
      clearTimeout(lspTimerId.value);
      lspTimerId.value = null;
    }
  }

  function resetAll(): void {
    resetState();
    activeLesson.value = 'SRP';
    initializeDemoData();
  }

  function destroyStore(): void {
    resetState();
  }

  function triggerCoolDownConfetti(): void {
    if (typeof window !== 'undefined') {
      window.dispatchEvent(new CustomEvent(COOL_DOWN_CONFETTI_EVENT));
    }
  }

  function triggerGlassBreakSound(): void {
    if (typeof window !== 'undefined') {
      window.dispatchEvent(new CustomEvent(GLASS_BREAK_SOUND_EVENT));
    }
  }

  return {
    // State
    activeLesson,
    classNodes,
    isSRPSplit,
    lspPhase,
    isLspShattered,
    dipState,
    lastDiagnosticResult,
    // Computed
    hasOverheatedNodes,
    overheatedNodeIds,
    totalNodes,
    srpViolationCount,
    isLSPTransmitting,
    isDIPCorrect,
    activeLessonLabel,
    // Actions
    setLesson,
    initializeDemoData,
    initializeSRPDemo,
    initializeClassNodes,
    triggerSRPSplit,
    executeLSPSubstitution,
    insertDIPInterface,
    resetDIP,
    resetState,
    resetAll,
    destroyStore,
  };
});
