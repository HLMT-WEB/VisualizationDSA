// ============================================================
// oop-visualization module — Public API
// Phase 2: OOP Concepts Visualizer & VTable Dynamic Dispatch
// ============================================================

export { default as OOPConceptsVisualizerWorkspace } from './components/OOPConceptsVisualizerWorkspace.vue';
export { default as UMLClassCard } from './components/UMLClassCard.vue';
export { default as AccessModifierPadlock } from './components/AccessModifierPadlock.vue';
export { default as DynamicDispatchLaser } from './components/DynamicDispatchLaser.vue';
export { default as HeapObjectAllocator } from './components/HeapObjectAllocator.vue';
export { default as PolymorphismSandbox } from './components/PolymorphismSandbox.vue';

export { OOPReflectionEngine } from './engine/OOPReflectionEngine';
export type { VTableDispatchResult, EncapsulationCheckResult } from './engine/OOPReflectionEngine';
export { SVGLaserBatchRenderer } from './engine/SVGLaserBatchRenderer';

export { useOOPVisualizerStore } from './store/useOOPVisualizerStore';

export type {
  AccessModifier,
  MemberType,
  DispatchStatus,
  ClassMember,
  ClassDefinition,
  HeapObjectInstance,
  ExecutionPointer,
  CoordinatePoint,
  LaserSegment,
  EncapsulationViolation,
} from './types/oop-visualization.types';

export {
  MAX_HEAP_OBJECTS,
  MAX_INHERITANCE_DEPTH,
  DISPATCH_LASER_DELAY_MS,
  VIOLATION_SHAKE_DURATION_MS,
  HEAP_BASE_ADDRESS,
  HEAP_ADDRESS_STEP,
} from './types/oop-visualization.types';
