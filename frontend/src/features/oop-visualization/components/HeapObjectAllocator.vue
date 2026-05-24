<template>
  <div class="heap-allocator bg-[#070b13]/60 border border-slate-800 rounded-xl p-4">
    <!-- Header -->
    <div class="flex items-center justify-between mb-4">
      <div class="flex items-center gap-2">
        <svg class="w-4 h-4 text-amber-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/>
        </svg>
        <span class="text-[11px] font-bold uppercase tracking-wider text-slate-400">
          Heap Memory Allocator
        </span>
      </div>
      <span class="text-[10px] font-mono text-slate-500">
        {{ heapObjects.length }}/{{ maxObjects }} objects
      </span>
    </div>

    <!-- Empty State -->
    <div v-if="heapObjects.length === 0" class="text-center py-8">
      <div class="text-slate-500 text-xs">
        Chưa có object nào trên Heap. Nhấp "New Instance" để khởi tạo.
      </div>
    </div>

    <!-- Heap Instances -->
    <div v-else class="space-y-2">
      <div
        v-for="instance in heapObjects"
        :key="instance.address"
        :id="`heap-obj-${instance.address}`"
        class="heap-instance p-3 bg-slate-900/50 border border-slate-800 rounded-lg transition-all hover:border-slate-700"
        :class="{ 'active-instance': instance.address === activeAddress }"
      >
        <div class="flex items-center justify-between">
          <div class="flex items-center gap-2">
            <span class="text-[10px] font-mono text-amber-400 font-bold">
              {{ instance.address }}
            </span>
            <span
              class="text-xs font-bold"
              :class="getClassColor(instance.className)"
            >
              {{ instance.className }}
            </span>
          </div>
          <button
            @click="$emit('remove', instance.address)"
            class="text-[10px] text-rose-400 hover:text-rose-300 px-2 py-0.5 rounded hover:bg-rose-950/30 transition-all"
          >
            free()
          </button>
        </div>

        <!-- Fields -->
        <div class="mt-2 flex flex-wrap gap-1">
          <span
            v-for="fieldName in getFieldNames(instance)"
            :key="fieldName"
            class="text-[9px] px-1.5 py-0.5 rounded bg-slate-800 text-slate-400"
          >
            {{ fieldName }}
          </span>
        </div>

        <!-- VTable Summary -->
        <div class="mt-2 flex flex-wrap gap-1">
          <span
            v-for="[methodName, resolvedClass] in getVTableEntries(instance)"
            :key="methodName"
            class="text-[9px] px-1.5 py-0.5 rounded"
            :class="resolvedClass !== instance.className
              ? 'bg-amber-950/40 text-amber-400 border border-amber-800/30'
              : 'bg-emerald-950/40 text-emerald-400 border border-emerald-800/30'"
          >
            {{ methodName }}() → {{ resolvedClass }}
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { HeapObjectInstance } from '../types/oop-visualization.types';
import { MAX_HEAP_OBJECTS } from '../types/oop-visualization.types';

defineProps<{
  heapObjects: HeapObjectInstance[];
  activeAddress?: string;
}>();

defineEmits<{
  (e: 'remove', address: string): void;
}>();

const maxObjects = MAX_HEAP_OBJECTS;

function getClassColor(className: string): string {
  switch (className) {
    case 'Shape':
      return 'text-emerald-400';
    case 'Circle':
      return 'text-purple-400';
    case 'Rectangle':
      return 'text-cyan-400';
    default:
      return 'text-slate-300';
  }
}

function getFieldNames(instance: HeapObjectInstance): string[] {
  return Array.from(instance.fieldsData.keys());
}

function getVTableEntries(instance: HeapObjectInstance): Array<[string, string]> {
  return Array.from(instance.vTable.entries());
}
</script>

<style scoped>
.active-instance {
  border-color: rgba(245, 158, 11, 0.5) !important;
  box-shadow: 0 0 15px rgba(245, 158, 11, 0.15);
}
</style>
