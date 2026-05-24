---
name: testing-visualizationdsa-features
description: Test VisualizationDSA feature tabs end-to-end via browser GUI. Use when verifying new Phase 2+ feature implementations (Embed Widget, Design Patterns, Debug Mode, etc).
---

# Testing VisualizationDSA Feature Tabs

## Prerequisites

- Node.js installed in the environment
- Repository cloned at `/home/ubuntu/VisualizationDSA/`

## Setup

1. Install dependencies:
   ```bash
   cd /home/ubuntu/VisualizationDSA/frontend && npm install
   ```

2. Start dev server:
   ```bash
   cd /home/ubuntu/VisualizationDSA/frontend && npx vite --host 0.0.0.0 --port 5173
   ```
   Note: The port might be taken (5173, 5174, etc). Check the terminal output for the actual port assigned.

3. Maximize browser window before recording:
   ```bash
   sudo apt-get install -y wmctrl 2>/dev/null; wmctrl -r :ACTIVE: -b add,maximized_vert,maximized_horz
   ```

## How to Navigate to Feature Tabs

The app uses a horizontal tab bar in the header (`<nav>` inside `<header>`). Each feature has its own tab button. Click the tab name (e.g., "Embed", "Patterns", "Debug") to switch to that feature's workspace.

The tab buttons are rendered from the `tabs` array in `App.vue`. New features are added as the last entries in this array.

## Testing Strategy

### For configurator/settings-style features (like Embed Widget):
1. **Verify default state** — Check all UI controls render with correct initial values
2. **Test each control independently** — Change one setting at a time, verify:
   - The UI control itself updates (active state, selected value)
   - The live preview/output updates reactively
   - Any generated code/config updates to reflect the change
   - Header badges (if present) update
3. **Test reset/clear functionality** — Verify all settings return to defaults
4. **Test edge cases** — Toggle combinations, rapid clicking, boundary values

### For visualization features (Sorting, Graph, etc.):
1. **Verify initial render** — Canvas/viewport shows expected visualization
2. **Test VCR controls** — Play, pause, step forward/backward
3. **Test input changes** — Different arrays, graph structures
4. **Test algorithm switching** — Verify visualization updates

## Verification via DOM

The computer-use tool returns DOM alongside screenshots. Use the DOM to verify:
- Text content changes (labels, badges, generated code)
- Element presence/absence (toggled sections)
- Select/input values
- Button active states

This is more reliable than visual inspection alone.

## Known Limitations

- **Clipboard operations** cannot be verified via computer-use tool (no paste target). Rely on unit tests for clipboard functionality.
- **Range slider interaction** is unreliable via automated browser tool due to precise coordinate requirements. Verify slider values at defaults and after reset. Rely on unit tests for slider clamping logic.
- **Port conflicts** are common. The dev server may start on 5174, 5175, etc. Always check terminal output.

## Unit Tests

Run all tests:
```bash
cd /home/ubuntu/VisualizationDSA/frontend && npx vitest run
```

There may be 1 pre-existing failure in `ForceDirectedLayout` test — this is a known issue on the main branch, not caused by new features.

## Devin Secrets Needed

None — this is a frontend-only application with no authentication or external API dependencies.
