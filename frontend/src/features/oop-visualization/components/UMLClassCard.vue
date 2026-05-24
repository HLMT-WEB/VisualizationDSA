<template>
  <div
    class="uml-class-card"
    :class="{
      'encapsulation-breach-wiggle': isWiggling,
      'card-active': isActive,
    }"
  >
    <!-- Class Header -->
    <div class="card-header" :style="{ borderColor: headerColor }">
      <div class="flex items-center justify-between">
        <span class="text-sm font-bold" :style="{ color: headerColor }">
          {{ classDef.className }}
        </span>
        <span v-if="classDef.parentClass" class="text-[10px] font-mono text-slate-500">
          extends {{ classDef.parentClass }}
        </span>
        <span v-else class="text-[10px] font-mono text-slate-500">Base Class</span>
      </div>
    </div>

    <!-- Fields Section -->
    <div class="card-body">
      <div class="section-label">Fields</div>
      <div v-for="field in fields" :key="field.name" class="member-row">
        <button
          class="member-btn"
          :class="{ 'member-violated': isFieldViolated(field.name) }"
          @click="onFieldClick(field)"
        >
          <AccessModifierPadlock :modifier="field.accessModifier" :size="'sm'" />
          <span class="text-slate-300 text-xs">{{ field.name }}: {{ field.returnType || 'any' }}</span>
          <span
            v-if="field.accessModifier === 'PRIVATE'"
            class="ml-auto text-[10px] text-red-400"
          >🔒</span>
          <span
            v-else-if="field.accessModifier === 'PROTECTED'"
            class="ml-auto text-[10px] text-yellow-400"
          >🔓</span>
        </button>
      </div>

      <!-- Divider -->
      <div class="section-divider"></div>

      <!-- Methods Section -->
      <div class="section-label">Methods</div>
      <div v-for="method in methods" :key="method.name" class="member-row">
        <button
          class="member-btn"
          :class="{
            'member-selected': isMethodSelected(method.name),
          }"
          @click="onMethodClick(method)"
        >
          <AccessModifierPadlock :modifier="method.accessModifier" :size="'sm'" />
          <span class="text-slate-300 text-xs">{{ method.name }}(): {{ method.returnType || 'void' }}</span>
          <span
            v-if="method.isOverridden"
            class="ml-auto text-[10px] font-bold"
            :style="{ color: headerColor }"
          >@Override</span>
          <span
            v-else
            class="ml-auto text-[10px] text-slate-500"
          >virtual</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { ClassDefinition, ClassMember } from '../types/oop-visualization.types';
import AccessModifierPadlock from './AccessModifierPadlock.vue';

const props = defineProps<{
  classDef: ClassDefinition;
  headerColor: string;
  isActive?: boolean;
  isWiggling?: boolean;
  violatedField?: string | null;
  selectedMethod?: string | null;
}>();

const emit = defineEmits<{
  (e: 'method-click', className: string, methodName: string): void;
  (e: 'field-click', className: string, fieldName: string): void;
}>();

const fields = computed(() =>
  props.classDef.members.filter((m) => m.type === 'FIELD')
);

const methods = computed(() =>
  props.classDef.members.filter((m) => m.type === 'METHOD')
);

function isFieldViolated(fieldName: string): boolean {
  return props.violatedField === fieldName;
}

function isMethodSelected(methodName: string): boolean {
  return props.selectedMethod === `${props.classDef.className}.${methodName}`;
}

function onMethodClick(method: ClassMember): void {
  emit('method-click', props.classDef.className, method.name);
}

function onFieldClick(field: ClassMember): void {
  emit('field-click', props.classDef.className, field.name);
}
</script>

<style scoped>
.uml-class-card {
  width: 100%;
  background: rgba(15, 23, 42, 0.45);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 16px;
  backdrop-filter: blur(12px);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
  font-family: 'Inter', sans-serif;
  color: #e2e8f0;
  transition: border-color 0.3s ease, box-shadow 0.3s ease, transform 0.3s ease;
  overflow: hidden;
}

.uml-class-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 14px 40px rgba(0, 0, 0, 0.6);
}

.card-active {
  border-color: rgba(6, 182, 212, 0.4);
  box-shadow: 0 0 20px rgba(6, 182, 212, 0.15);
}

.encapsulation-breach-wiggle {
  animation: wiggle-vibrate 0.4s cubic-bezier(0.36, 0.07, 0.19, 0.97) both;
  animation-iteration-count: 5;
  border-color: #ef4444 !important;
  box-shadow: 0 0 25px rgba(239, 68, 68, 0.5) !important;
}

@keyframes wiggle-vibrate {
  10%,
  90% {
    transform: translate3d(-1px, 0, 0);
  }
  20%,
  80% {
    transform: translate3d(2px, 0, 0);
  }
  30%,
  50%,
  70% {
    transform: translate3d(-4px, 0, 0);
  }
  40%,
  60% {
    transform: translate3d(4px, 0, 0);
  }
}

.card-header {
  border-bottom: 1px solid;
  padding: 10px 16px;
  background: rgba(7, 11, 19, 0.6);
}

.card-body {
  padding: 12px 16px;
}

.section-label {
  font-size: 10px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  color: #64748b;
  margin-bottom: 6px;
}

.section-divider {
  height: 1px;
  background: rgba(30, 41, 59, 0.8);
  margin: 10px 0;
}

.member-row {
  margin-bottom: 2px;
}

.member-btn {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 5px 8px;
  border-radius: 6px;
  transition: all 0.2s ease;
  text-align: left;
  background: transparent;
  border: 1px solid transparent;
  cursor: pointer;
}

.member-btn:hover {
  background: rgba(30, 41, 59, 0.5);
}

.member-selected {
  background: rgba(139, 92, 246, 0.15) !important;
  border-color: rgba(139, 92, 246, 0.3) !important;
}

.member-violated {
  background: rgba(239, 68, 68, 0.15) !important;
  border-color: rgba(239, 68, 68, 0.4) !important;
  animation: pulse 1s infinite;
}

@keyframes pulse {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.7; }
}
</style>
