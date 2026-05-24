<template>
  <div class="flex flex-col h-full gap-4">
    <!-- Header -->
    <div class="flex items-center justify-between px-2">
      <div class="flex items-center gap-3">
        <div class="w-8 h-8 rounded-lg bg-cyan-600/20 border border-cyan-500/30 flex items-center justify-center">
          <span class="text-cyan-400 text-sm">🗺️</span>
        </div>
        <div>
          <h2 class="text-base font-bold text-white">Learning Path Skill Tree</h2>
          <p class="text-[10px] text-slate-500">Bản đồ lộ trình học tập RPG cá nhân hóa</p>
        </div>
      </div>

      <div class="flex items-center gap-3">
        <!-- Average Score Badge -->
        <div class="flex items-center gap-1.5 px-3 py-1.5 rounded-lg bg-slate-800/50 border border-slate-700/50">
          <span class="text-[10px] text-slate-500">Điểm TB:</span>
          <span class="text-xs font-bold" :class="scoreColorClass">{{ store.averageScore }}%</span>
        </div>

        <!-- Completion Badge -->
        <div class="flex items-center gap-1.5 px-3 py-1.5 rounded-lg bg-slate-800/50 border border-slate-700/50">
          <span class="text-[10px] text-slate-500">Hoàn thành:</span>
          <span class="text-xs font-bold text-cyan-400">{{ store.completionPercentage }}%</span>
        </div>

        <!-- Demo Controls -->
        <button
          @click="handleDemoComplete"
          class="px-3 py-1.5 rounded-lg text-[10px] font-bold uppercase tracking-wider bg-emerald-500/20 text-emerald-300 border border-emerald-500/30 hover:bg-emerald-500/30 transition-colors"
        >
          +Demo Hoàn Thành Ải
        </button>

        <button
          @click="store.resetProgress()"
          class="px-3 py-1.5 rounded-lg text-[10px] font-bold uppercase tracking-wider bg-red-500/20 text-red-300 border border-red-500/30 hover:bg-red-500/30 transition-colors"
        >
          Reset
        </button>
      </div>
    </div>

    <!-- Main Content -->
    <div class="flex-1 flex gap-4 min-h-0">
      <!-- Map Area (Left) -->
      <div class="flex-1 min-w-0">
        <LearningPathMap @node-selected="handleNodeSelected" />
      </div>

      <!-- Sidebar (Right) -->
      <div class="w-72 flex flex-col gap-4">
        <!-- AI Evaluator Card -->
        <AIEvaluatorCard
          :recommendation="store.aiRecommendedNode"
          :is-review-mode="isReviewMode"
          @navigate-to="handleNavigateTo"
        />

        <!-- Node Details -->
        <div
          v-if="selectedNode"
          class="rounded-2xl p-4 border"
          :class="nodeDetailClasses"
        >
          <div class="flex items-center gap-2 mb-3">
            <span class="text-lg">{{ selectedNodeIcon }}</span>
            <h4 class="text-sm font-bold text-white">{{ selectedNode.title }}</h4>
          </div>

          <div class="space-y-2">
            <div class="flex items-center justify-between">
              <span class="text-[10px] text-slate-500">Trạng thái</span>
              <span class="text-[10px] font-bold" :class="statusColorClass">
                {{ statusLabel }}
              </span>
            </div>
            <div v-if="selectedNode.prerequisites.length > 0" class="flex items-center justify-between">
              <span class="text-[10px] text-slate-500">Tiên quyết</span>
              <span class="text-[10px] text-slate-400">
                {{ selectedNode.prerequisites.join(', ') }}
              </span>
            </div>
            <div v-if="selectedNodeScore" class="flex items-center justify-between">
              <span class="text-[10px] text-slate-500">Điểm thi</span>
              <span class="text-[10px] font-bold" :class="selectedNodeScore.scorePercentage >= 70 ? 'text-emerald-400' : 'text-amber-400'">
                {{ selectedNodeScore.scorePercentage }}%
              </span>
            </div>
          </div>
        </div>

        <!-- Nodes List Summary -->
        <div class="rounded-2xl p-4 bg-slate-900/50 backdrop-blur-sm border border-slate-700/30">
          <h4 class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-3">
            Danh sách ải môn
          </h4>
          <div class="space-y-2">
            <div
              v-for="node in store.resolvedNodes"
              :key="node.id"
              class="flex items-center justify-between px-2 py-1.5 rounded-lg hover:bg-slate-800/50 cursor-pointer transition-colors"
              @click="handleNodeSelected(node.id)"
            >
              <div class="flex items-center gap-2">
                <div
                  class="w-2.5 h-2.5 rounded-full"
                  :class="{
                    'bg-emerald-500': node.status === 'COMPLETED',
                    'bg-cyan-500': node.status === 'UNLOCKED' || node.status === 'IN_PROGRESS',
                    'bg-slate-600': node.status === 'LOCKED',
                  }"
                />
                <span class="text-[11px] text-slate-300">{{ node.title }}</span>
              </div>
              <span class="text-[9px] font-medium" :class="{
                'text-emerald-400': node.status === 'COMPLETED',
                'text-cyan-400': node.status === 'UNLOCKED',
                'text-amber-400': node.status === 'IN_PROGRESS',
                'text-slate-600': node.status === 'LOCKED',
              }">
                {{ node.status }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useLearningPathStore } from '../store/useLearningPathStore';
import LearningPathMap from './LearningPathMap.vue';
import AIEvaluatorCard from './AIEvaluatorCard.vue';
import type { PathNode } from '../types/learning-path.types';

const store = useLearningPathStore();
const selectedNodeId = ref('quicksort');

const selectedNode = computed<PathNode | undefined>(() => {
  return store.resolvedNodes.find((n) => n.id === selectedNodeId.value);
});

const selectedNodeScore = computed(() => {
  return store.userScoresHistory.find((s) => s.algorithmId === selectedNodeId.value);
});

const isReviewMode = computed(() => {
  return store.userScoresHistory.some((s) => s.scorePercentage < 70);
});

const scoreColorClass = computed(() => {
  if (store.averageScore >= 80) return 'text-emerald-400';
  if (store.averageScore >= 70) return 'text-cyan-400';
  return 'text-amber-400';
});

const selectedNodeIcon = computed(() => {
  if (!selectedNode.value) return '📚';
  switch (selectedNode.value.status) {
    case 'COMPLETED': return '⭐';
    case 'UNLOCKED':
    case 'IN_PROGRESS': return '⚡';
    case 'LOCKED': return '🔒';
    default: return '📚';
  }
});

const statusLabel = computed(() => {
  if (!selectedNode.value) return '';
  switch (selectedNode.value.status) {
    case 'COMPLETED': return 'Đã hoàn thành';
    case 'UNLOCKED': return 'Đã mở khóa';
    case 'IN_PROGRESS': return 'Đang học';
    case 'LOCKED': return 'Bị khóa';
    default: return '';
  }
});

const statusColorClass = computed(() => {
  if (!selectedNode.value) return '';
  switch (selectedNode.value.status) {
    case 'COMPLETED': return 'text-emerald-400';
    case 'UNLOCKED': return 'text-cyan-400';
    case 'IN_PROGRESS': return 'text-amber-400';
    case 'LOCKED': return 'text-slate-600';
    default: return '';
  }
});

const nodeDetailClasses = computed(() => {
  if (!selectedNode.value) return 'bg-slate-900/50 border-slate-700/30';
  switch (selectedNode.value.status) {
    case 'COMPLETED':
      return 'bg-emerald-900/10 border-emerald-500/30';
    case 'UNLOCKED':
    case 'IN_PROGRESS':
      return 'bg-cyan-900/10 border-cyan-500/30';
    case 'LOCKED':
      return 'bg-slate-900/50 border-slate-700/30';
    default:
      return 'bg-slate-900/50 border-slate-700/30';
  }
});

function handleNodeSelected(nodeId: string) {
  selectedNodeId.value = nodeId;
  store.setActiveNode(nodeId);
}

function handleNavigateTo(nodeId: string) {
  selectedNodeId.value = nodeId;
  store.setActiveNode(nodeId);
}

async function handleDemoComplete() {
  const nextNode = store.resolvedNodes.find(
    (n) => n.status === 'UNLOCKED' || n.status === 'IN_PROGRESS'
  );
  if (nextNode) {
    const demoScore = 75 + Math.floor(Math.random() * 25);
    await store.completeNodeMilestone(nextNode.id, demoScore, 90 + Math.floor(Math.random() * 120));
    selectedNodeId.value = nextNode.id;
  }
}
</script>
