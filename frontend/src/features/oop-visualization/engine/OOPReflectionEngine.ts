// ============================================================
// OOP Reflection Engine — Phase 2 Meta-Object Reflection & VTable
// Class registration, inheritance chain, VTable dispatch, encapsulation
// ============================================================

import type {
  ClassDefinition,
  ClassMember,
  HeapObjectInstance,
  AccessModifier,
} from '../types/oop-visualization.types';
import {
  MAX_INHERITANCE_DEPTH,
  MAX_HEAP_OBJECTS,
  HEAP_BASE_ADDRESS,
  HEAP_ADDRESS_STEP,
} from '../types/oop-visualization.types';

export interface VTableDispatchResult {
  methodName: string;
  resolvedClass: string;
  dispatchPath: string[];
  isOverridden: boolean;
}

export interface EncapsulationCheckResult {
  hasAccess: boolean;
  errorReason?: string;
  modifier: AccessModifier;
}

export class OOPReflectionEngine {
  private classesRegistry: Map<string, ClassDefinition> = new Map();
  private heapInstances: HeapObjectInstance[] = [];
  private addressOffset = 0;

  registerClass(config: ClassDefinition): void {
    const depth = this.getInheritanceDepth(config);
    if (depth > MAX_INHERITANCE_DEPTH) {
      throw new Error(
        `KẾ THỪA QUÁ SÂU: Giới hạn chiều sâu kế thừa tối đa là ${MAX_INHERITANCE_DEPTH} cấp để đảm bảo tính trong sáng của sơ đồ thiết kế.`
      );
    }
    this.classesRegistry.set(config.className, config);
  }

  getClass(className: string): ClassDefinition | undefined {
    return this.classesRegistry.get(className);
  }

  getAllClasses(): ClassDefinition[] {
    return Array.from(this.classesRegistry.values());
  }

  getHeapInstances(): HeapObjectInstance[] {
    return [...this.heapInstances];
  }

  getHeapInstanceCount(): number {
    return this.heapInstances.length;
  }

  instantiateObject(className: string): HeapObjectInstance {
    const classDef = this.classesRegistry.get(className);
    if (!classDef) {
      throw new Error(`Lớp ${className} chưa được đăng ký trong hệ thống.`);
    }

    if (this.heapInstances.length >= MAX_HEAP_OBJECTS) {
      throw new Error(
        `HEAP OVERFLOW: Đã đạt giới hạn tối đa ${MAX_HEAP_OBJECTS} đối tượng trên Heap ảo.`
      );
    }

    const memAddress = `0x${(HEAP_BASE_ADDRESS + this.addressOffset)
      .toString(16)
      .toUpperCase()
      .padStart(6, '0')}`;
    this.addressOffset += HEAP_ADDRESS_STEP;

    const fields = new Map<string, unknown>();
    const vTable = new Map<string, string>();

    const inheritanceChain = this.buildInheritanceChain(classDef);

    for (const def of inheritanceChain) {
      for (const member of def.members) {
        if (member.type === 'FIELD') {
          if (!fields.has(member.name)) {
            fields.set(member.name, null);
          }
        } else if (member.type === 'METHOD') {
          vTable.set(member.name, def.className);
        }
      }
    }

    const instance: HeapObjectInstance = {
      address: memAddress,
      className,
      fieldsData: fields,
      vTable,
    };

    this.heapInstances.push(instance);
    return instance;
  }

  removeHeapInstance(address: string): boolean {
    const idx = this.heapInstances.findIndex((i) => i.address === address);
    if (idx === -1) return false;
    this.heapInstances.splice(idx, 1);
    return true;
  }

  dispatchMethod(
    instance: HeapObjectInstance,
    methodName: string
  ): VTableDispatchResult | null {
    const resolvedClass = instance.vTable.get(methodName);
    if (!resolvedClass) return null;

    const dispatchPath = this.buildDispatchPath(instance.className, resolvedClass);
    const classDef = this.classesRegistry.get(resolvedClass);
    const method = classDef?.members.find(
      (m) => m.name === methodName && m.type === 'METHOD'
    );

    return {
      methodName,
      resolvedClass,
      dispatchPath,
      isOverridden: method?.isOverridden ?? false,
    };
  }

  validateEncapsulationAccess(
    targetClassName: string,
    memberName: string,
    callerClassName: string
  ): EncapsulationCheckResult {
    const targetClass = this.classesRegistry.get(targetClassName);
    if (!targetClass) {
      return {
        hasAccess: false,
        errorReason: `Lớp ${targetClassName} không tồn tại.`,
        modifier: 'PRIVATE',
      };
    }

    const member = this.findMemberInChain(targetClassName, memberName);
    if (!member) {
      return {
        hasAccess: false,
        errorReason: `Thành viên '${memberName}' không tồn tại trong lớp ${targetClassName}.`,
        modifier: 'PRIVATE',
      };
    }

    if (member.accessModifier === 'PUBLIC') {
      return { hasAccess: true, modifier: 'PUBLIC' };
    }

    if (member.accessModifier === 'PRIVATE') {
      if (targetClassName === callerClassName) {
        return { hasAccess: true, modifier: 'PRIVATE' };
      }
      return {
        hasAccess: false,
        errorReason: `ENCAPSULATION_ERROR: Thuộc tính '${memberName}' có modifier là PRIVATE. Không thể truy cập từ lớp ngoài '${callerClassName}'.`,
        modifier: 'PRIVATE',
      };
    }

    if (member.accessModifier === 'PROTECTED') {
      if (targetClassName === callerClassName) {
        return { hasAccess: true, modifier: 'PROTECTED' };
      }

      if (this.isSubclassOf(callerClassName, targetClassName)) {
        return { hasAccess: true, modifier: 'PROTECTED' };
      }

      return {
        hasAccess: false,
        errorReason: `ENCAPSULATION_ERROR: Thuộc tính '${memberName}' có modifier là PROTECTED. Chỉ được phép truy cập từ lớp con kế thừa.`,
        modifier: 'PROTECTED',
      };
    }

    return {
      hasAccess: false,
      errorReason: 'Quyền truy cập không hợp lệ.',
      modifier: 'PRIVATE',
    };
  }

  getInheritanceChain(className: string): string[] {
    const classDef = this.classesRegistry.get(className);
    if (!classDef) return [];
    return this.buildInheritanceChain(classDef).map((c) => c.className);
  }

  clearRegistry(): void {
    this.classesRegistry.clear();
    this.heapInstances = [];
    this.addressOffset = 0;
  }

  private buildInheritanceChain(classDef: ClassDefinition): ClassDefinition[] {
    const chain: ClassDefinition[] = [];
    let current: ClassDefinition | undefined = classDef;
    let depth = 0;

    while (current && depth < MAX_INHERITANCE_DEPTH + 1) {
      chain.unshift(current);
      current = current.parentClass
        ? this.classesRegistry.get(current.parentClass)
        : undefined;
      depth++;
    }

    return chain;
  }

  private buildDispatchPath(
    instanceClass: string,
    resolvedClass: string
  ): string[] {
    const path: string[] = [];
    let current = this.classesRegistry.get(instanceClass);

    while (current) {
      path.unshift(current.className);
      if (current.className === resolvedClass) break;
      current = current.parentClass
        ? this.classesRegistry.get(current.parentClass)
        : undefined;
    }

    return path;
  }

  private isSubclassOf(
    childClassName: string,
    parentClassName: string
  ): boolean {
    let current = this.classesRegistry.get(childClassName);
    while (current) {
      if (current.parentClass === parentClassName) return true;
      current = current.parentClass
        ? this.classesRegistry.get(current.parentClass)
        : undefined;
    }
    return false;
  }

  private findMemberInChain(
    className: string,
    memberName: string
  ): ClassMember | undefined {
    const classDef = this.classesRegistry.get(className);
    if (!classDef) return undefined;

    const member = classDef.members.find((m) => m.name === memberName);
    if (member) return member;

    if (classDef.parentClass) {
      return this.findMemberInChain(classDef.parentClass, memberName);
    }

    return undefined;
  }

  private getInheritanceDepth(config: ClassDefinition): number {
    let depth = 0;
    let parentName = config.parentClass;
    while (parentName) {
      depth++;
      const parent = this.classesRegistry.get(parentName);
      parentName = parent?.parentClass;
    }
    return depth;
  }
}
