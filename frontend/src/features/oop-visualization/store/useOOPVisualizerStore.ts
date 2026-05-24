// ============================================================
// useOOPVisualizerStore — Pinia Setup Store
// Orchestrates class registry, Heap allocation, VTable dispatch,
// encapsulation violation detection, and laser animation state
// ============================================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { OOPReflectionEngine } from '../engine/OOPReflectionEngine';
import type {
  ClassDefinition,
  HeapObjectInstance,
  ExecutionPointer,
  EncapsulationViolation,
} from '../types/oop-visualization.types';
import {
  MAX_HEAP_OBJECTS,
  DISPATCH_LASER_DELAY_MS,
  VIOLATION_SHAKE_DURATION_MS,
} from '../types/oop-visualization.types';

const DEMO_CLASSES: ClassDefinition[] = [
  {
    className: 'Shape',
    members: [
      { name: 'x', type: 'FIELD', accessModifier: 'PUBLIC', returnType: 'number' },
      { name: 'y', type: 'FIELD', accessModifier: 'PUBLIC', returnType: 'number' },
      { name: 'color', type: 'FIELD', accessModifier: 'PROTECTED', returnType: 'string' },
      { name: 'area', type: 'METHOD', accessModifier: 'PUBLIC', returnType: 'number' },
      { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC', returnType: 'void' },
    ],
  },
  {
    className: 'Circle',
    parentClass: 'Shape',
    members: [
      { name: 'radius', type: 'FIELD', accessModifier: 'PRIVATE', returnType: 'number' },
      { name: 'area', type: 'METHOD', accessModifier: 'PUBLIC', returnType: 'number', isOverridden: true },
      { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC', returnType: 'void', isOverridden: true },
    ],
  },
  {
    className: 'Rectangle',
    parentClass: 'Shape',
    members: [
      { name: 'width', type: 'FIELD', accessModifier: 'PRIVATE', returnType: 'number' },
      { name: 'height', type: 'FIELD', accessModifier: 'PRIVATE', returnType: 'number' },
      { name: 'area', type: 'METHOD', accessModifier: 'PUBLIC', returnType: 'number', isOverridden: true },
      { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC', returnType: 'void', isOverridden: true },
    ],
  },
];

export const useOOPVisualizerStore = defineStore('oopVisualizer', () => {
  // ==========================================
  // ENGINE INSTANCE
  // ==========================================
  const engine = new OOPReflectionEngine();

  // ==========================================
  // STATE
  // ==========================================
  const registeredClasses = ref<ClassDefinition[]>([]);
  const heapObjects = ref<HeapObjectInstance[]>([]);

  const activeExecutionPointer = ref<ExecutionPointer>({
    callerClass: 'Main',
    activeObjectAddress: '',
    activeMethod: '',
    dispatchStatus: 'IDLE',
    resolvedClass: undefined,
  });

  const lastEncapsulationViolation = ref<EncapsulationViolation | null>(null);
  const selectedClassName = ref<string>('Circle');
  const selectedMethodCall = ref<string | null>(null);
  const dispatchTimerId = ref<ReturnType<typeof setTimeout> | null>(null);
  const violationTimerId = ref<ReturnType<typeof setTimeout> | null>(null);

  // ==========================================
  // COMPUTED
  // ==========================================
  const heapObjectCount = computed(() => heapObjects.value.length);

  const canAllocate = computed(
    () => heapObjects.value.length < MAX_HEAP_OBJECTS
  );

  const isDispatching = computed(
    () => activeExecutionPointer.value.dispatchStatus === 'SEEKING_VTABLE'
  );

  const isViolated = computed(
    () => activeExecutionPointer.value.dispatchStatus === 'ACCESS_VIOLATED'
  );

  const availableClassNames = computed(() =>
    registeredClasses.value.map((c) => c.className)
  );

  const vTableForSelectedClass = computed(() => {
    const className = selectedClassName.value;
    const instance = heapObjects.value.find((o) => o.className === className);
    if (!instance) return [];

    const entries: Array<{
      methodName: string;
      resolvedClass: string;
      isOverridden: boolean;
    }> = [];

    for (const [methodName, resolvedClass] of instance.vTable) {
      const classDef = engine.getClass(resolvedClass);
      const method = classDef?.members.find(
        (m) => m.name === methodName && m.type === 'METHOD'
      );
      entries.push({
        methodName,
        resolvedClass,
        isOverridden: method?.isOverridden ?? false,
      });
    }

    return entries;
  });

  // ==========================================
  // ACTIONS
  // ==========================================
  function initializeDemoClasses(): void {
    engine.clearRegistry();
    registeredClasses.value = [];
    heapObjects.value = [];

    for (const classDef of DEMO_CLASSES) {
      engine.registerClass(classDef);
      registeredClasses.value.push(classDef);
    }
  }

  function registerClass(config: ClassDefinition): void {
    try {
      engine.registerClass(config);
      registeredClasses.value.push(config);
    } catch (error) {
      console.error((error as Error).message);
    }
  }

  function instantiateNewObject(className: string): string {
    try {
      const instance = engine.instantiateObject(className);
      heapObjects.value = [...engine.getHeapInstances()];
      return instance.address;
    } catch (error) {
      console.error((error as Error).message);
      return '';
    }
  }

  function removeHeapObject(address: string): void {
    engine.removeHeapInstance(address);
    heapObjects.value = [...engine.getHeapInstances()];
  }

  function triggerPolymorphicCall(
    objectAddress: string,
    methodName: string,
    callerClass: string = 'Main'
  ): void {
    const obj = heapObjects.value.find((o) => o.address === objectAddress);
    if (!obj) return;

    clearTimers();

    selectedMethodCall.value = `${obj.className}.${methodName}`;

    activeExecutionPointer.value = {
      callerClass,
      activeObjectAddress: objectAddress,
      activeMethod: methodName,
      dispatchStatus: 'SEEKING_VTABLE',
      resolvedClass: undefined,
    };

    dispatchTimerId.value = setTimeout(() => {
      const result = engine.dispatchMethod(obj, methodName);
      if (result) {
        activeExecutionPointer.value = {
          ...activeExecutionPointer.value,
          dispatchStatus: 'DISPATCHED',
          resolvedClass: result.resolvedClass,
        };
      }
      dispatchTimerId.value = null;
    }, DISPATCH_LASER_DELAY_MS);
  }

  function tryAccessProperty(
    targetClass: string,
    propertyName: string,
    callerClass: string = 'ExternalClass'
  ): boolean {
    const result = engine.validateEncapsulationAccess(
      targetClass,
      propertyName,
      callerClass
    );

    if (!result.hasAccess) {
      clearTimers();

      lastEncapsulationViolation.value = {
        targetClass,
        memberName: propertyName,
        callerClass,
        errorMessage:
          result.errorReason ?? 'Vi phạm quyền đóng gói.',
        timestamp: Date.now(),
      };

      activeExecutionPointer.value = {
        ...activeExecutionPointer.value,
        dispatchStatus: 'ACCESS_VIOLATED',
      };

      violationTimerId.value = setTimeout(() => {
        lastEncapsulationViolation.value = null;
        activeExecutionPointer.value = {
          ...activeExecutionPointer.value,
          dispatchStatus: 'IDLE',
        };
        violationTimerId.value = null;
      }, VIOLATION_SHAKE_DURATION_MS);

      return false;
    }

    lastEncapsulationViolation.value = null;
    return true;
  }

  function selectClass(className: string): void {
    selectedClassName.value = className;
  }

  function resetAll(): void {
    clearTimers();
    engine.clearRegistry();
    registeredClasses.value = [];
    heapObjects.value = [];
    activeExecutionPointer.value = {
      callerClass: 'Main',
      activeObjectAddress: '',
      activeMethod: '',
      dispatchStatus: 'IDLE',
      resolvedClass: undefined,
    };
    lastEncapsulationViolation.value = null;
    selectedClassName.value = 'Circle';
    selectedMethodCall.value = null;
  }

  function resetDispatchState(): void {
    clearTimers();
    activeExecutionPointer.value = {
      callerClass: 'Main',
      activeObjectAddress: '',
      activeMethod: '',
      dispatchStatus: 'IDLE',
      resolvedClass: undefined,
    };
    selectedMethodCall.value = null;
  }

  function destroyStore(): void {
    clearTimers();
    engine.clearRegistry();
  }

  function clearTimers(): void {
    if (dispatchTimerId.value !== null) {
      clearTimeout(dispatchTimerId.value);
      dispatchTimerId.value = null;
    }
    if (violationTimerId.value !== null) {
      clearTimeout(violationTimerId.value);
      violationTimerId.value = null;
    }
  }

  function getEngine(): OOPReflectionEngine {
    return engine;
  }

  return {
    // State
    registeredClasses,
    heapObjects,
    activeExecutionPointer,
    lastEncapsulationViolation,
    selectedClassName,
    selectedMethodCall,
    // Computed
    heapObjectCount,
    canAllocate,
    isDispatching,
    isViolated,
    availableClassNames,
    vTableForSelectedClass,
    // Actions
    initializeDemoClasses,
    registerClass,
    instantiateNewObject,
    removeHeapObject,
    triggerPolymorphicCall,
    tryAccessProperty,
    selectClass,
    resetAll,
    resetDispatchState,
    destroyStore,
    getEngine,
  };
});
