<script setup lang="ts">
import type { SystemNode, NetworkLink } from '../types/system-design-viz.types';

const props = defineProps<{
  link: NetworkLink;
  sourceNode: SystemNode;
  targetNode: SystemNode;
  isFailed: boolean;
}>();

function sourceX(): number {
  return props.sourceNode.posX + 70;
}
function sourceY(): number {
  return props.sourceNode.posY + 40;
}
function targetX(): number {
  return props.targetNode.posX + 70;
}
function targetY(): number {
  return props.targetNode.posY + 40;
}
</script>

<template>
  <line
    :x1="sourceX()"
    :y1="sourceY()"
    :x2="targetX()"
    :y2="targetY()"
    :stroke="isFailed ? '#EF4444' : '#06B6D4'"
    stroke-width="2"
    :stroke-dasharray="isFailed ? '6,4' : 'none'"
    :opacity="isFailed ? 0.3 : 0.6"
  />
</template>
