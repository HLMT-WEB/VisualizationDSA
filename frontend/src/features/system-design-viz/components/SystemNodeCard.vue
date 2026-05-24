<script setup lang="ts">
import type { SystemNode } from '../types/system-design-viz.types';
import { NODE_TYPE_LABELS } from '../types/system-design-viz.types';

const props = defineProps<{
  node: SystemNode;
}>();

const emit = defineEmits<{
  (e: 'toggleStatus', nodeId: string): void;
}>();

function statusColor(status: string): string {
  switch (status) {
    case 'HEALTHY':
      return '#10B981';
    case 'OVERLOADED':
      return '#F59E0B';
    case 'FAILED':
      return '#EF4444';
    default:
      return '#64748B';
  }
}
</script>

<template>
  <div
    class="system-node-card"
    :class="{
      'is-failed': node.status === 'FAILED',
      'is-overloaded': node.status === 'OVERLOADED',
    }"
    :style="{ left: node.posX + 'px', top: node.posY + 'px' }"
  >
    <div class="node-header">
      <span class="node-type-badge">{{ NODE_TYPE_LABELS[node.nodeType] }}</span>
      <span
        class="node-status-dot"
        :style="{ backgroundColor: statusColor(node.status) }"
      ></span>
    </div>
    <div class="node-label">{{ node.label }}</div>
    <div class="node-stats">
      <span class="request-count">{{ node.requestCount }} req</span>
      <span
        class="status-text"
        :style="{ color: statusColor(node.status) }"
      >
        {{ node.status }}
      </span>
    </div>
    <button
      v-if="node.nodeType === 'WEB_SERVER'"
      class="toggle-btn"
      :class="{ 'is-danger': node.status !== 'FAILED' }"
      @click.stop="emit('toggleStatus', node.nodeId)"
    >
      {{ node.status === 'FAILED' ? 'Khôi phục' : 'Sập nguồn' }}
    </button>
  </div>
</template>

<style scoped>
.system-node-card {
  background: rgba(15, 23, 42, 0.6);
  border: 1.5px solid rgba(255, 255, 255, 0.08);
  border-radius: 16px;
  padding: 16px;
  backdrop-filter: blur(16px);
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
  min-width: 140px;
  position: relative;
  transition: transform 0.25s cubic-bezier(0.4, 0, 0.2, 1),
    border-color 0.25s ease;
}

.system-node-card.is-failed {
  border-color: #ef4444;
  box-shadow: 0 0 20px rgba(239, 68, 68, 0.5),
    inset 0 0 10px rgba(239, 68, 68, 0.2);
  filter: brightness(0.8);
}

.system-node-card.is-overloaded {
  border-color: #f59e0b;
  box-shadow: 0 0 16px rgba(245, 158, 11, 0.4);
}

.node-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
}

.node-type-badge {
  font-size: 10px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  color: #94a3b8;
  background: rgba(100, 116, 139, 0.2);
  padding: 2px 6px;
  border-radius: 4px;
}

.node-status-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  box-shadow: 0 0 6px currentColor;
}

.node-label {
  font-size: 16px;
  font-weight: 600;
  color: #e2e8f0;
  margin-bottom: 8px;
}

.node-stats {
  display: flex;
  justify-content: space-between;
  font-size: 11px;
  color: #94a3b8;
  margin-bottom: 8px;
}

.status-text {
  font-weight: 600;
  text-transform: uppercase;
  font-size: 10px;
}

.toggle-btn {
  width: 100%;
  padding: 6px 0;
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 8px;
  background: rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  font-size: 11px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.toggle-btn.is-danger {
  color: #ef4444;
  border-color: rgba(239, 68, 68, 0.3);
}

.toggle-btn:hover {
  background: rgba(255, 255, 255, 0.1);
}
</style>
