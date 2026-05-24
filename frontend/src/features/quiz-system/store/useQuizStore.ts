import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { useLectureStore } from '../../e-lecture/store/useLectureStore';
import { useAnimationStore } from '../../animation-engine/store/useAnimationStore';
import { QuizVerificationEngine } from '../engine/QuizVerificationEngine';
import { QuizStatsManager } from '../engine/QuizStatsManager';
import type { QuizQuestion, QuizCheckpoint, CanvasNodeDTO } from '../types/quiz.types';
import { quizApi } from '../../../services/quizApi';
import type { QuizDto, QuizHistoryEntry } from '../../../services/quizApi';
import { getStoredToken } from '../../../services/apiClient';

/**
 * useQuizStore — Pinia Store điều khiển trạng thái trắc nghiệm tương tác.
 * Quản lý checkpoint detection, câu hỏi MC/TF/Canvas, chấm điểm,
 * khóa bài giảng E-Lecture và đồng bộ thống kê localStorage.
 */
export const useQuizStore = defineStore('quizSystem', () => {
  const lectureStore = useLectureStore();
  const animStore = useAnimationStore();

  // ==========================================
  // STATE
  // ==========================================
  const activeQuestion = ref<QuizQuestion | null>(null);
  const selectedAnswerIndex = ref<number | null>(null);
  const isSubmitted = ref(false);
  const isCorrect = ref(false);
  const feedbackExplanation = ref('');
  const matchedNodeId = ref<string | null>(null);
  const isCanvasTargetMode = ref(false);

  const checkpoints = ref<QuizCheckpoint[]>([]);
  const completedCheckpointIndexes = ref<number[]>([]);

  const sessionCorrect = ref(0);
  const sessionTotal = ref(0);

  // ==========================================
  // GETTERS
  // ==========================================
  const isLectureLockedByQuiz = computed(() => activeQuestion.value !== null);

  const isQuizActive = computed(() => activeQuestion.value !== null);

  const sessionAccuracy = computed(() => {
    if (sessionTotal.value === 0) return 0;
    return Math.round((sessionCorrect.value / sessionTotal.value) * 100);
  });

  const allCheckpointsCompleted = computed(() => {
    if (checkpoints.value.length === 0) return false;
    return checkpoints.value.every((cp) =>
      completedCheckpointIndexes.value.includes(cp.frameIndex),
    );
  });

  // ==========================================
  // ACTIONS
  // ==========================================

  function loadCheckpoints(quizCheckpoints: QuizCheckpoint[]): void {
    checkpoints.value = quizCheckpoints;
    completedCheckpointIndexes.value = [];
    sessionCorrect.value = 0;
    sessionTotal.value = 0;
  }

  function checkFrameForQuiz(frameIndex: number): void {
    if (activeQuestion.value !== null) return;

    if (completedCheckpointIndexes.value.includes(frameIndex)) return;

    const checkpoint = checkpoints.value.find((cp) => cp.frameIndex === frameIndex);
    if (!checkpoint) return;

    triggerCheckpointQuestion(checkpoint.question, frameIndex);
  }

  function triggerCheckpointQuestion(question: QuizQuestion, frameIndex: number): void {
    activeQuestion.value = question;
    selectedAnswerIndex.value = null;
    isSubmitted.value = false;
    isCorrect.value = false;
    feedbackExplanation.value = '';
    matchedNodeId.value = null;
    isCanvasTargetMode.value = question.type === 'CANVAS_TARGET';

    lectureStore.lockLectureInteraction();

    if (!completedCheckpointIndexes.value.includes(frameIndex)) {
      completedCheckpointIndexes.value.push(frameIndex);
    }
  }

  function submitOptionAnswer(index: number): void {
    if (!activeQuestion.value || isSubmitted.value) return;

    selectedAnswerIndex.value = index;
    isSubmitted.value = true;

    const result = QuizVerificationEngine.verifyOptionAnswer(index, activeQuestion.value);

    isCorrect.value = result.isCorrect;
    feedbackExplanation.value = result.explanation;

    sessionTotal.value++;
    if (result.isCorrect) sessionCorrect.value++;

    QuizStatsManager.saveAttempt(result.isCorrect, activeQuestion.value.id);
  }

  function handleCanvasClickAnswer(clickX: number, clickY: number, nodes: CanvasNodeDTO[]): void {
    if (!activeQuestion.value || isSubmitted.value) return;
    if (activeQuestion.value.type !== 'CANVAS_TARGET') return;

    const result = QuizVerificationEngine.verifyCanvasClickAnswer(
      clickX,
      clickY,
      nodes,
      activeQuestion.value,
    );

    if (!result.matchedNodeId) return;

    isSubmitted.value = true;
    isCorrect.value = result.isCorrect;
    feedbackExplanation.value = result.explanation;
    matchedNodeId.value = result.matchedNodeId ?? null;
    isCanvasTargetMode.value = false;

    sessionTotal.value++;
    if (result.isCorrect) sessionCorrect.value++;

    QuizStatsManager.saveAttempt(result.isCorrect, activeQuestion.value.id);
  }

  function dismissQuestionAndContinue(): void {
    activeQuestion.value = null;
    selectedAnswerIndex.value = null;
    isSubmitted.value = false;
    isCorrect.value = false;
    feedbackExplanation.value = '';
    matchedNodeId.value = null;
    isCanvasTargetMode.value = false;

    lectureStore.unlockLectureInteraction();
  }

  function resetQuizStore(): void {
    activeQuestion.value = null;
    selectedAnswerIndex.value = null;
    isSubmitted.value = false;
    isCorrect.value = false;
    feedbackExplanation.value = '';
    matchedNodeId.value = null;
    isCanvasTargetMode.value = false;
    checkpoints.value = [];
    completedCheckpointIndexes.value = [];
    sessionCorrect.value = 0;
    sessionTotal.value = 0;
  }

  // ==========================================
  // SERVER-SYNC ACTIONS (B3 Integration)
  // ==========================================
  const serverQuizzes = ref<QuizDto[]>([]);
  const quizHistory = ref<QuizHistoryEntry[]>([]);
  const isLoadingQuizzes = ref(false);
  const quizSyncError = ref<string | null>(null);

  const isOnlineMode = computed(() => !!getStoredToken());

  async function fetchQuizzesFromServer(): Promise<void> {
    if (!isOnlineMode.value) return;

    try {
      isLoadingQuizzes.value = true;
      quizSyncError.value = null;
      serverQuizzes.value = await quizApi.getAll();
    } catch {
      quizSyncError.value = 'Không thể tải danh sách quiz';
    } finally {
      isLoadingQuizzes.value = false;
    }
  }

  async function fetchQuizzesByTopic(topic: string): Promise<QuizDto[]> {
    if (!isOnlineMode.value) return [];

    try {
      return await quizApi.getByTopic(topic);
    } catch {
      quizSyncError.value = 'Không thể tải quiz theo chủ đề';
      return [];
    }
  }

  async function submitAttemptToServer(quizId: string, answers: number[]): Promise<void> {
    if (!isOnlineMode.value) return;

    try {
      quizSyncError.value = null;
      await quizApi.submitAttempt({ quizId, answers });
    } catch {
      quizSyncError.value = 'Không thể gửi kết quả quiz';
    }
  }

  async function fetchQuizHistory(): Promise<void> {
    if (!isOnlineMode.value) return;

    try {
      quizSyncError.value = null;
      quizHistory.value = await quizApi.getHistory();
    } catch {
      quizSyncError.value = 'Không thể tải lịch sử quiz';
    }
  }

  return {
    activeQuestion,
    selectedAnswerIndex,
    isSubmitted,
    isCorrect,
    feedbackExplanation,
    matchedNodeId,
    isCanvasTargetMode,
    checkpoints,
    completedCheckpointIndexes,
    sessionCorrect,
    sessionTotal,
    serverQuizzes,
    quizHistory,
    isLoadingQuizzes,
    quizSyncError,

    isLectureLockedByQuiz,
    isQuizActive,
    sessionAccuracy,
    allCheckpointsCompleted,
    isOnlineMode,

    loadCheckpoints,
    checkFrameForQuiz,
    triggerCheckpointQuestion,
    submitOptionAnswer,
    handleCanvasClickAnswer,
    dismissQuestionAndContinue,
    resetQuizStore,
    // Server-sync
    fetchQuizzesFromServer,
    fetchQuizzesByTopic,
    submitAttemptToServer,
    fetchQuizHistory,
  };
});
