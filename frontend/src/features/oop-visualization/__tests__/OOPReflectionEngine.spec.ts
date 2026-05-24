import { describe, it, expect, beforeEach } from 'vitest';
import { OOPReflectionEngine } from '../engine/OOPReflectionEngine';
import type { ClassDefinition } from '../types/oop-visualization.types';

describe('OOPReflectionEngine', () => {
  let engine: OOPReflectionEngine;

  beforeEach(() => {
    engine = new OOPReflectionEngine();
  });

  describe('registerClass', () => {
    it('should register a class and retrieve it by name', () => {
      const classDef: ClassDefinition = {
        className: 'Shape',
        members: [
          { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC' },
        ],
      };
      engine.registerClass(classDef);
      expect(engine.getClass('Shape')).toEqual(classDef);
    });

    it('should register multiple classes', () => {
      engine.registerClass({
        className: 'Animal',
        members: [{ name: 'speak', type: 'METHOD', accessModifier: 'PUBLIC' }],
      });
      engine.registerClass({
        className: 'Dog',
        parentClass: 'Animal',
        members: [{ name: 'speak', type: 'METHOD', accessModifier: 'PUBLIC', isOverridden: true }],
      });
      expect(engine.getAllClasses()).toHaveLength(2);
    });

    it('should reject inheritance depth exceeding MAX_INHERITANCE_DEPTH (5)', () => {
      engine.registerClass({ className: 'L0', members: [] });
      engine.registerClass({ className: 'L1', parentClass: 'L0', members: [] });
      engine.registerClass({ className: 'L2', parentClass: 'L1', members: [] });
      engine.registerClass({ className: 'L3', parentClass: 'L2', members: [] });
      engine.registerClass({ className: 'L4', parentClass: 'L3', members: [] });

      expect(() => {
        engine.registerClass({ className: 'L5', parentClass: 'L4', members: [] });
      }).not.toThrow();

      expect(() => {
        engine.registerClass({ className: 'L6', parentClass: 'L5', members: [] });
      }).toThrow(/KẾ THỪA QUÁ SÂU/);
    });
  });

  describe('instantiateObject', () => {
    it('should throw error for unregistered class', () => {
      expect(() => engine.instantiateObject('Unknown')).toThrow(
        /chưa được đăng ký/
      );
    });

    it('should create heap object with correct hex address', () => {
      engine.registerClass({
        className: 'Point',
        members: [{ name: 'x', type: 'FIELD', accessModifier: 'PUBLIC' }],
      });
      const obj = engine.instantiateObject('Point');
      expect(obj.address).toMatch(/^0x[0-9A-F]{6}$/);
      expect(obj.className).toBe('Point');
    });

    it('should increment addresses by 16 for each allocation', () => {
      engine.registerClass({
        className: 'A',
        members: [],
      });

      const obj1 = engine.instantiateObject('A');
      const obj2 = engine.instantiateObject('A');

      const addr1 = parseInt(obj1.address, 16);
      const addr2 = parseInt(obj2.address, 16);
      expect(addr2 - addr1).toBe(16);
    });

    it('should enforce MAX_HEAP_OBJECTS (10) limit', () => {
      engine.registerClass({ className: 'T', members: [] });
      for (let i = 0; i < 10; i++) {
        engine.instantiateObject('T');
      }
      expect(() => engine.instantiateObject('T')).toThrow(/HEAP OVERFLOW/);
    });

    it('should collect fields from inheritance chain', () => {
      engine.registerClass({
        className: 'Base',
        members: [
          { name: 'baseField', type: 'FIELD', accessModifier: 'PUBLIC' },
        ],
      });
      engine.registerClass({
        className: 'Child',
        parentClass: 'Base',
        members: [
          { name: 'childField', type: 'FIELD', accessModifier: 'PRIVATE' },
        ],
      });

      const obj = engine.instantiateObject('Child');
      expect(obj.fieldsData.has('baseField')).toBe(true);
      expect(obj.fieldsData.has('childField')).toBe(true);
    });
  });

  describe('VTable Dynamic Dispatch', () => {
    it('should resolve VTable to overridden Circle.draw instead of Shape.draw', () => {
      engine.registerClass({
        className: 'Shape',
        members: [
          { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC' },
        ],
      });
      engine.registerClass({
        className: 'Circle',
        parentClass: 'Shape',
        members: [
          { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC', isOverridden: true },
        ],
      });

      const circleObj = engine.instantiateObject('Circle');
      expect(circleObj.vTable.get('draw')).toBe('Circle');
    });

    it('should resolve inherited method to parent class when not overridden', () => {
      engine.registerClass({
        className: 'Shape',
        members: [
          { name: 'toString', type: 'METHOD', accessModifier: 'PUBLIC' },
          { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC' },
        ],
      });
      engine.registerClass({
        className: 'Circle',
        parentClass: 'Shape',
        members: [
          { name: 'draw', type: 'METHOD', accessModifier: 'PUBLIC', isOverridden: true },
        ],
      });

      const circleObj = engine.instantiateObject('Circle');
      expect(circleObj.vTable.get('draw')).toBe('Circle');
      expect(circleObj.vTable.get('toString')).toBe('Shape');
    });

    it('should dispatch method and return dispatch path', () => {
      engine.registerClass({
        className: 'Shape',
        members: [{ name: 'area', type: 'METHOD', accessModifier: 'PUBLIC' }],
      });
      engine.registerClass({
        className: 'Circle',
        parentClass: 'Shape',
        members: [
          { name: 'area', type: 'METHOD', accessModifier: 'PUBLIC', isOverridden: true },
        ],
      });

      const obj = engine.instantiateObject('Circle');
      const result = engine.dispatchMethod(obj, 'area');

      expect(result).not.toBeNull();
      expect(result!.resolvedClass).toBe('Circle');
      expect(result!.isOverridden).toBe(true);
      expect(result!.dispatchPath).toContain('Circle');
    });

    it('should return null when dispatching non-existent method', () => {
      engine.registerClass({ className: 'Empty', members: [] });
      const obj = engine.instantiateObject('Empty');
      expect(engine.dispatchMethod(obj, 'nonExistent')).toBeNull();
    });

    it('should handle 3-level inheritance VTable correctly', () => {
      engine.registerClass({
        className: 'Animal',
        members: [
          { name: 'speak', type: 'METHOD', accessModifier: 'PUBLIC' },
          { name: 'move', type: 'METHOD', accessModifier: 'PUBLIC' },
        ],
      });
      engine.registerClass({
        className: 'Dog',
        parentClass: 'Animal',
        members: [
          { name: 'speak', type: 'METHOD', accessModifier: 'PUBLIC', isOverridden: true },
        ],
      });
      engine.registerClass({
        className: 'Puppy',
        parentClass: 'Dog',
        members: [
          { name: 'speak', type: 'METHOD', accessModifier: 'PUBLIC', isOverridden: true },
        ],
      });

      const puppyObj = engine.instantiateObject('Puppy');
      expect(puppyObj.vTable.get('speak')).toBe('Puppy');
      expect(puppyObj.vTable.get('move')).toBe('Animal');
    });
  });

  describe('Encapsulation Access Control', () => {
    beforeEach(() => {
      engine.registerClass({
        className: 'BankAccount',
        members: [
          { name: 'balance', type: 'FIELD', accessModifier: 'PRIVATE' },
          { name: 'ownerName', type: 'FIELD', accessModifier: 'PROTECTED' },
          { name: 'getBalance', type: 'METHOD', accessModifier: 'PUBLIC' },
        ],
      });
      engine.registerClass({
        className: 'SavingsAccount',
        parentClass: 'BankAccount',
        members: [
          { name: 'interestRate', type: 'FIELD', accessModifier: 'PRIVATE' },
        ],
      });
    });

    it('should allow PUBLIC access from any class', () => {
      const result = engine.validateEncapsulationAccess(
        'BankAccount', 'getBalance', 'Hacker'
      );
      expect(result.hasAccess).toBe(true);
      expect(result.modifier).toBe('PUBLIC');
    });

    it('should reject PRIVATE access from external class', () => {
      const result = engine.validateEncapsulationAccess(
        'BankAccount', 'balance', 'Hacker'
      );
      expect(result.hasAccess).toBe(false);
      expect(result.errorReason).toContain('ENCAPSULATION_ERROR');
      expect(result.modifier).toBe('PRIVATE');
    });

    it('should allow PRIVATE access from same class', () => {
      const result = engine.validateEncapsulationAccess(
        'BankAccount', 'balance', 'BankAccount'
      );
      expect(result.hasAccess).toBe(true);
    });

    it('should allow PROTECTED access from subclass', () => {
      const result = engine.validateEncapsulationAccess(
        'BankAccount', 'ownerName', 'SavingsAccount'
      );
      expect(result.hasAccess).toBe(true);
    });

    it('should reject PROTECTED access from unrelated class', () => {
      const result = engine.validateEncapsulationAccess(
        'BankAccount', 'ownerName', 'Hacker'
      );
      expect(result.hasAccess).toBe(false);
      expect(result.errorReason).toContain('ENCAPSULATION_ERROR');
      expect(result.modifier).toBe('PROTECTED');
    });

    it('should reject access to non-existent class', () => {
      const result = engine.validateEncapsulationAccess(
        'NonExistent', 'field', 'Main'
      );
      expect(result.hasAccess).toBe(false);
    });

    it('should reject access to non-existent member', () => {
      const result = engine.validateEncapsulationAccess(
        'BankAccount', 'nonExistent', 'Main'
      );
      expect(result.hasAccess).toBe(false);
    });
  });

  describe('getInheritanceChain', () => {
    it('should return correct chain for 3-level hierarchy', () => {
      engine.registerClass({ className: 'A', members: [] });
      engine.registerClass({ className: 'B', parentClass: 'A', members: [] });
      engine.registerClass({ className: 'C', parentClass: 'B', members: [] });

      expect(engine.getInheritanceChain('C')).toEqual(['A', 'B', 'C']);
    });

    it('should return single class for base class', () => {
      engine.registerClass({ className: 'Root', members: [] });
      expect(engine.getInheritanceChain('Root')).toEqual(['Root']);
    });

    it('should return empty array for unknown class', () => {
      expect(engine.getInheritanceChain('Unknown')).toEqual([]);
    });
  });

  describe('removeHeapInstance', () => {
    it('should remove instance by address', () => {
      engine.registerClass({ className: 'X', members: [] });
      const obj = engine.instantiateObject('X');
      expect(engine.getHeapInstanceCount()).toBe(1);
      expect(engine.removeHeapInstance(obj.address)).toBe(true);
      expect(engine.getHeapInstanceCount()).toBe(0);
    });

    it('should return false for non-existent address', () => {
      expect(engine.removeHeapInstance('0xDEAD')).toBe(false);
    });
  });

  describe('clearRegistry', () => {
    it('should clear all classes and heap instances', () => {
      engine.registerClass({ className: 'Q', members: [] });
      engine.instantiateObject('Q');
      engine.clearRegistry();

      expect(engine.getAllClasses()).toHaveLength(0);
      expect(engine.getHeapInstances()).toHaveLength(0);
    });

    it('should reset address offset after clear', () => {
      engine.registerClass({ className: 'R', members: [] });
      engine.instantiateObject('R');
      engine.instantiateObject('R');
      engine.clearRegistry();

      engine.registerClass({ className: 'S', members: [] });
      const obj = engine.instantiateObject('S');
      const addr = parseInt(obj.address, 16);
      expect(addr).toBe(0x310000);
    });
  });
});
