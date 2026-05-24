<template>
  <div class="oop-workspace bg-[#0e1726]/70 backdrop-blur-md border border-slate-800/80 rounded-2xl p-6 shadow-xl flex flex-col gap-5 relative">

    <!-- Header -->
    <div class="flex items-center justify-between border-b border-slate-800 pb-4">
      <div class="flex items-center gap-2">
        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" class="text-purple-400">
          <polygon points="12 2 2 7 12 12 22 7 12 2"/>
          <polyline points="2 17 12 22 22 17"/>
          <polyline points="2 12 12 17 22 12"/>
        </svg>
        <span class="text-xs font-bold uppercase tracking-wider text-slate-300">
          OOP Concepts Visualizer — Phase 2
        </span>
      </div>
      <div class="flex gap-1.5">
        <span class="text-[10px] font-bold uppercase tracking-wider bg-purple-950/40 text-purple-400 border border-purple-800/40 px-2 py-1 rounded-lg">
          VTable Dynamic Dispatch
        </span>
        <span class="text-[10px] font-bold uppercase tracking-wider bg-cyan-950/40 text-cyan-400 border border-cyan-800/40 px-2 py-1 rounded-lg">
          Encapsulation
        </span>
      </div>
    </div>

    <!-- UML Class Cards Row -->
    <div class="flex flex-col gap-3">
      <div class="text-[11px] font-bold uppercase tracking-wider text-slate-400 flex items-center gap-2">
        <span class="w-1.5 h-1.5 rounded-full bg-purple-400"></span>
        Sơ đồ lớp kế thừa (Class Hierarchy)
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <UMLClassCard
          v-for="classDef in store.registeredClasses"
          :key="classDef.className"
          :class-def="classDef"
          :header-color="getClassHeaderColor(classDef.className)"
          :is-active="store.selectedClassName === classDef.className"
          :is-wiggling="isCardWiggling(classDef.className)"
          :violated-field="getViolatedField(classDef.className)"
          :selected-method="store.selectedMethodCall"
          @method-click="onMethodClick"
          @field-click="onFieldClick"
        />
      </div>
    </div>

    <!-- Sandbox + Heap Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <!-- Polymorphism Sandbox -->
      <PolymorphismSandbox
        :available-classes="store.availableClassNames"
        :selected-class="store.selectedClassName"
        :can-allocate="store.canAllocate"
        :v-table-entries="store.vTableForSelectedClass"
        :dispatch-status="store.activeExecutionPointer.dispatchStatus"
        :resolved-class="store.activeExecutionPointer.resolvedClass"
        :active-method="store.selectedMethodCall"
        :violation="store.lastEncapsulationViolation"
        @select-class="store.selectClass"
        @instantiate="onInstantiate"
        @dispatch="onDispatchFromSandbox"
        @reset="onReset"
      />

      <!-- Heap Object Allocator -->
      <HeapObjectAllocator
        :heap-objects="store.heapObjects"
        :active-address="store.activeExecutionPointer.activeObjectAddress"
        @remove="store.removeHeapObject"
      />
    </div>

    <!-- Dynamic Dispatch Laser (SVG Overlay) -->
    <DynamicDispatchLaser
      :is-active="store.isDispatching || store.activeExecutionPointer.dispatchStatus === 'DISPATCHED'"
      :source="laserSource"
      :v-table-pivot="laserPivot"
      :target="laserTarget"
      :phase="store.activeExecutionPointer.dispatchStatus === 'DISPATCHED' ? 'resolved' : 'seeking'"
      :is-overridden="isCurrentDispatchOverridden"
    />
  </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, computed, ref } from 'vue';
import { useOOPVisualizerStore } from '../store/useOOPVisualizerStore';
import type { CoordinatePoint } from '../types/oop-visualization.types';
import UMLClassCard from './UMLClassCard.vue';
import DynamicDispatchLaser from './DynamicDispatchLaser.vue';
import HeapObjectAllocator from './HeapObjectAllocator.vue';
import PolymorphismSandbox from './PolymorphismSandbox.vue';

const store = useOOPVisualizerStore();

const laserSource = ref<CoordinatePoint>({ x: 100, y: 80 });
const laserPivot = ref<CoordinatePoint>({ x: 300, y: 200 });
const laserTarget = ref<CoordinatePoint>({ x: 500, y: 120 });

const CLASS_COLORS: Record<string, string> = {
  Shape: '#10b981',
  Circle: '#a855f7',
  Rectangle: '#06b6d4',
};

onMounted(() => {
  store.initializeDemoClasses();
  store.instantiateNewObject('Circle');
});

onUnmounted(() => {
  store.destroyStore();
});

function getClassHeaderColor(className: string): string {
  return CLASS_COLORS[className] ?? '#94a3b8';
}

function isCardWiggling(className: string): boolean {
  return (
    store.lastEncapsulationViolation?.targetClass === className &&
    store.activeExecutionPointer.dispatchStatus === 'ACCESS_VIOLATED'
  );
}

function getViolatedField(className: string): string | null {
  if (store.lastEncapsulationViolation?.targetClass === className) {
    return store.lastEncapsulationViolation.memberName;
  }
  return null;
}

const isCurrentDispatchOverridden = computed(() => {
  const ptr = store.activeExecutionPointer;
  if (!ptr.resolvedClass || !ptr.activeMethod) return false;
  const entry = store.vTableForSelectedClass.find(
    (e) => e.methodName === ptr.activeMethod
  );
  return entry?.isOverridden ?? false;
});

function onMethodClick(className: string, methodName: string): void {
  store.selectClass(className);
  const instance = store.heapObjects.find((o) => o.className === className);
  if (instance) {
    store.triggerPolymorphicCall(instance.address, methodName);
  }
}

function onFieldClick(className: string, fieldName: string): void {
  store.tryAccessProperty(className, fieldName, 'ExternalClass');
}

function onInstantiate(className: string): void {
  store.instantiateNewObject(className);
}

function onDispatchFromSandbox(methodName: string): void {
  const className = store.selectedClassName;
  const instance = store.heapObjects.find((o) => o.className === className);
  if (instance) {
    store.triggerPolymorphicCall(instance.address, methodName);
  }
}

function onReset(): void {
  store.resetAll();
  store.initializeDemoClasses();
  store.instantiateNewObject('Circle');
}
</script>

<style scoped>
.oop-workspace {
  min-height: 500px;
  position: relative;
}
</style>
