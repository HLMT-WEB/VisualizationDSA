import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { SVGLaserBatchRenderer } from '../engine/SVGLaserBatchRenderer';

let rafId = 0;
const rafCallbacks = new Map<number, FrameRequestCallback>();

vi.stubGlobal('requestAnimationFrame', (cb: FrameRequestCallback) => {
  rafId++;
  rafCallbacks.set(rafId, cb);
  setTimeout(() => {
    const fn = rafCallbacks.get(rafId);
    if (fn) {
      fn(performance.now());
      rafCallbacks.delete(rafId);
    }
  }, 16);
  return rafId;
});

vi.stubGlobal('cancelAnimationFrame', (id: number) => {
  rafCallbacks.delete(id);
});

describe('SVGLaserBatchRenderer', () => {
  describe('calculateLaserPath (static)', () => {
    it('should return valid SVG cubic bezier path string', () => {
      const path = SVGLaserBatchRenderer.calculateLaserPath(
        { x: 10, y: 20 },
        { x: 100, y: 80 }
      );
      expect(path).toContain('M 10 20');
      expect(path).toContain('C ');
      expect(path).toContain('100 80');
    });

    it('should calculate control point at midpoint X', () => {
      const path = SVGLaserBatchRenderer.calculateLaserPath(
        { x: 0, y: 0 },
        { x: 200, y: 100 }
      );
      expect(path).toContain('C 100 0');
    });
  });

  describe('calculateDispatchLaserPath (static)', () => {
    it('should return path with source → pivot → target segments', () => {
      const path = SVGLaserBatchRenderer.calculateDispatchLaserPath(
        { x: 10, y: 20 },
        { x: 150, y: 100 },
        { x: 300, y: 50 }
      );
      expect(path).toContain('M 10 20');
      expect(path).toContain('150 100');
      expect(path).toContain('300 50');
    });
  });

  describe('calculateLinearPath (static)', () => {
    it('should return a straight line path', () => {
      const path = SVGLaserBatchRenderer.calculateLinearPath(
        { x: 5, y: 10 },
        { x: 50, y: 60 }
      );
      expect(path).toBe('M 5 10 L 50 60');
    });
  });

  describe('getDOMElementCenter (static)', () => {
    it('should return {0,0} when element not found (SSR-safe)', () => {
      const center = SVGLaserBatchRenderer.getDOMElementCenter('non-existent-id');
      expect(center).toEqual({ x: 0, y: 0 });
    });
  });

  describe('scheduleBatchRender', () => {
    let renderer: SVGLaserBatchRenderer;

    beforeEach(() => {
      renderer = new SVGLaserBatchRenderer();
      vi.useFakeTimers();
    });

    afterEach(() => {
      renderer.destroy();
      vi.useRealTimers();
    });

    it('should execute batched updates on next animation frame', () => {
      const fn1 = vi.fn();
      const fn2 = vi.fn();

      renderer.scheduleBatchRender(fn1);
      renderer.scheduleBatchRender(fn2);

      expect(fn1).not.toHaveBeenCalled();
      expect(fn2).not.toHaveBeenCalled();

      vi.advanceTimersByTime(20);

      expect(fn1).toHaveBeenCalledOnce();
      expect(fn2).toHaveBeenCalledOnce();
    });
  });

  describe('destroy', () => {
    it('should clean up pending animation frames', () => {
      const renderer = new SVGLaserBatchRenderer();
      const fn = vi.fn();
      renderer.scheduleBatchRender(fn);
      renderer.destroy();

      expect(fn).not.toHaveBeenCalled();
    });
  });
});
