<template>
  <div class="flowchart-panel">
    <div class="panel-header">
      <span class="panel-icon">📊</span>
      <span class="panel-title">Flowchart</span>
      <span class="panel-badge" v-if="activeNodeId">{{ activeNodeId }}</span>
    </div>
    <div class="flowchart-container">
      <div class="flowchart-nodes">
        <div
          v-for="node in flowchartNodes"
          :key="node.id"
          class="flowchart-node"
          :class="{
            'node-active': node.id === activeNodeId,
            'node-start': node.type === 'start',
            'node-end': node.type === 'end',
            'node-decision': node.type === 'decision',
            'node-process': node.type === 'process',
          }"
        >
          <div class="node-label">{{ node.label }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useMultiViewStore } from '../store/useMultiViewStore';

const store = useMultiViewStore();

const activeNodeId = computed(() => store.currentStep?.activeFlowchartNodeId ?? '');

interface FlowchartNode {
  id: string;
  label: string;
  type: 'start' | 'end' | 'process' | 'decision';
}

const flowchartNodes: FlowchartNode[] = [
  { id: 'start', label: 'Bắt đầu', type: 'start' },
  { id: 'outer-loop', label: 'Vòng lặp ngoài i', type: 'process' },
  { id: 'compare', label: 'So sánh a[j] > a[j+1]', type: 'decision' },
  { id: 'swap', label: 'Hoán đổi', type: 'process' },
  { id: 'highlight', label: 'Đánh dấu đã sắp', type: 'process' },
  { id: 'end', label: 'Kết thúc', type: 'end' },
];
</script>

<style scoped>
.flowchart-panel {
  display: flex;
  flex-direction: column;
  height: 100%;
  background: rgba(15, 23, 42, 0.6);
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 12px;
  overflow: hidden;
}

.panel-header {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  background: rgba(15, 23, 42, 0.8);
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.panel-icon {
  font-size: 14px;
}

.panel-title {
  font-size: 12px;
  font-weight: 600;
  color: #e2e8f0;
}

.panel-badge {
  margin-left: auto;
  padding: 2px 8px;
  background: rgba(6, 182, 212, 0.15);
  border: 1px solid rgba(6, 182, 212, 0.3);
  border-radius: 6px;
  font-size: 10px;
  color: #06B6D4;
}

.flowchart-container {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 16px;
}

.flowchart-nodes {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
}

.flowchart-node {
  padding: 8px 20px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 8px;
  background: rgba(255, 255, 255, 0.03);
  color: #94a3b8;
  font-size: 11px;
  text-align: center;
  min-width: 140px;
  transition: all 0.2s ease;
  position: relative;
}

.flowchart-node::after {
  content: '↓';
  position: absolute;
  bottom: -14px;
  left: 50%;
  transform: translateX(-50%);
  color: rgba(255, 255, 255, 0.1);
  font-size: 10px;
}

.flowchart-node:last-child::after {
  display: none;
}

.node-start {
  border-radius: 20px;
  border-color: rgba(16, 185, 129, 0.3);
}

.node-end {
  border-radius: 20px;
  border-color: rgba(239, 68, 68, 0.3);
}

.node-decision {
  border-color: rgba(245, 158, 11, 0.3);
  transform: rotate(0deg);
}

.node-active {
  background: rgba(6, 182, 212, 0.15) !important;
  border-color: #06B6D4 !important;
  color: #06B6D4 !important;
  box-shadow: 0 0 15px rgba(6, 182, 212, 0.4), 0 0 30px rgba(6, 182, 212, 0.2);
  animation: node-pulse 1.5s ease-in-out infinite;
}

.node-active::after {
  color: #06B6D4;
}

@keyframes node-pulse {
  0%, 100% { box-shadow: 0 0 15px rgba(6, 182, 212, 0.4), 0 0 30px rgba(6, 182, 212, 0.2); }
  50% { box-shadow: 0 0 20px rgba(6, 182, 212, 0.6), 0 0 40px rgba(6, 182, 212, 0.3); }
}

.node-label {
  font-weight: 500;
}
</style>
