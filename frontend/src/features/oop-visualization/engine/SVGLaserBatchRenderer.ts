// ============================================================
// SVG Laser Batch Renderer — Dynamic Dispatch Laser Pointer
// Cubic Bezier paths for VTable dispatch visualization
// rAF-throttled coordinate updates for 60 FPS smooth rendering
// ============================================================

import type { CoordinatePoint } from '../types/oop-visualization.types';

export class SVGLaserBatchRenderer {
  private animationFrameId: number | null = null;
  private pendingUpdates: Array<() => void> = [];

  static getDOMElementCenter(elementId: string): CoordinatePoint {
    if (typeof document === 'undefined') return { x: 0, y: 0 };

    const el = document.getElementById(elementId);
    if (!el) return { x: 0, y: 0 };

    const rect = el.getBoundingClientRect();
    return {
      x: rect.left + rect.width / 2,
      y: rect.top + rect.height / 2,
    };
  }

  static calculateLaserPath(
    start: CoordinatePoint,
    end: CoordinatePoint
  ): string {
    const controlX = (start.x + end.x) / 2;
    return `M ${start.x} ${start.y} C ${controlX} ${start.y}, ${controlX} ${end.y}, ${end.x} ${end.y}`;
  }

  static calculateDispatchLaserPath(
    source: CoordinatePoint,
    vTablePivot: CoordinatePoint,
    target: CoordinatePoint
  ): string {
    const seg1ControlX = (source.x + vTablePivot.x) / 2;
    const seg2ControlX = (vTablePivot.x + target.x) / 2;

    return [
      `M ${source.x} ${source.y}`,
      `C ${seg1ControlX} ${source.y}, ${seg1ControlX} ${vTablePivot.y}, ${vTablePivot.x} ${vTablePivot.y}`,
      `C ${seg2ControlX} ${vTablePivot.y}, ${seg2ControlX} ${target.y}, ${target.x} ${target.y}`,
    ].join(' ');
  }

  static calculateLinearPath(
    start: CoordinatePoint,
    end: CoordinatePoint
  ): string {
    return `M ${start.x} ${start.y} L ${end.x} ${end.y}`;
  }

  scheduleBatchRender(updateFn: () => void): void {
    this.pendingUpdates.push(updateFn);

    if (this.animationFrameId === null) {
      this.animationFrameId = requestAnimationFrame(() => {
        for (const fn of this.pendingUpdates) {
          fn();
        }
        this.pendingUpdates = [];
        this.animationFrameId = null;
      });
    }
  }

  destroy(): void {
    if (this.animationFrameId !== null) {
      cancelAnimationFrame(this.animationFrameId);
      this.animationFrameId = null;
    }
    this.pendingUpdates = [];
  }
}
