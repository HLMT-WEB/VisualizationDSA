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

### For state-driven features (like Learning Path, Gamification):
1. **Verify initial state** — Check header badges, node/item statuses, sidebar lists match expected defaults from the Pinia store's initial data
2. **Test state transitions** — Use demo/action buttons to trigger state changes, verify:
   - Items change status reactively (e.g., LOCKED→UNLOCKED→COMPLETED)
   - Header stats update (percentages, scores)
   - AI/recommendation cards update to reflect new state
   - Progress bars reflect new completion percentage
3. **Test completion/end state** — Trigger all items to complete, verify congratulations/trophy UI
4. **Test reset** — Verify all state reverts to clean defaults (0%, all items reset)
5. **Test detail selection** — Click items in sidebar list to show detail panels

### For Pinia store testing via browser console:
If you need to manipulate store state directly (e.g., set streak values, bypass limitations):
```javascript
// Access any Pinia store from the Vue app instance
const app = document.querySelector('#app').__vue_app__;
const pinia = app.config.globalProperties.$pinia;
const store = pinia._s.get('storeName'); // e.g., 'gamification', 'learningPath'
store.someAction(); // Call store actions directly
```

### For multi-panel synchronized features (like Multi-View):
1. **Verify initial panel layout** — Check panel headers, badges, and content match expected defaults
2. **Test step navigation** — Use ⏭/⏮ buttons and verify ALL panels update in sync:
   - Code Editor badge ("Dòng X") changes
   - Active line highlight moves to correct line
   - SVG bars update values and comparing/sorted colors
   - Flowchart active node badge changes (if in 3-panel mode)
   - Sync status bar updates step counter and percentage
3. **Test layout toggle** — Switch between 2-panel and 3-panel, verify new panel appears/disappears
4. **Test speed controls** — Click speed buttons (0.25x-4x), verify active state toggles
5. **Test reset** — Verify ALL state returns to defaults (step, speed, layout, pane widths)
6. **Adversarial checks** — Verify ⏮ is disabled at step 0, badges show correct values not off-by-one

### For OOP/concept visualization features (like OOP Viz):
1. **Verify UML class cards** — Check class names, inheritance labels ("Base Class"/"extends X"), field/method listings
2. **Verify access modifier badges** — public (green), protected (yellow + 🔓), private (red + 🔒)
3. **Test VTable dispatch** — Click method entries in VTable Dispatch Map, verify:
   - Status changes to seeking ("Đang tra cứu VTable...") with blinking cyan dot
   - After ~800ms, resolves to "Dynamic Dispatch thành công!" with green dot
   - Resolved class name shown correctly
   - SVG laser path animates between cards
4. **Test encapsulation violations** — Click PRIVATE fields on UML cards, verify:
   - Red violation alert "VI PHẠM ĐÓNG GÓI!" with ENCAPSULATION_ERROR message
   - Card wiggle/shake animation with red border
   - Alert auto-clears after ~2 seconds
5. **Test heap allocation** — Use class selector + instantiate button, verify:
   - New object appears with sequential hex address (offset +16 bytes)
   - Object shows correct fields and VTable summary badges
   - Counter updates (e.g., 1/10 → 2/10)
6. **Test reset** — Verify heap, class selection, and dispatch status all return to initial state

### For interactive quiz features (like Smart Quiz):
1. **Verify initial state** — SVG canvas with bars, session dashboard (stats 0/0/0), accuracy 0%, demo trigger buttons
2. **Test quiz trigger** — Click demo button, verify:
   - Quiz overlay slides in with correct question type badge (SVG Click=cyan, Trắc nghiệm=emerald, Monaco=amber)
   - VCR LOCKED badge appears (red), TIMELINE READY→TIMELINE LOCKED transition
   - SUBMIT button disabled until selection made
   - Session stats increment question count
3. **Test selection mechanics** — For SVG quizzes:
   - Click bars to select (Amber glow), verify node-id badges appear below hint
   - Counter updates "Đã chọn: X / Y"
   - **Max selection enforcement**: clicking beyond max is IGNORED (counter unchanged)
   - SUBMIT enables after first selection
4. **Test incorrect submission** — Submit wrong answers, verify:
   - Crimson banner "✗ Chưa đúng" with partial score percentage
   - THỬ LẠI (retry) + TIẾP TỤC (continue) buttons appear
   - No XP badge on incorrect answer
5. **Test retry with 0 XP policy** — Click THỬ LẠI, reselect correct answers, submit:
   - Emerald banner "✓ Chính xác!" with 100%
   - **NO XP badge** — first-try bonus policy means retry = 0 XP
   - Only TIẾP TỤC button (no retry needed for correct answer)
6. **Test quiz close + VCR unlock** — Click TIẾP TỤC:
   - Overlay slides out, TIMELINE READY (green) badge restored
   - Demo buttons re-enabled
7. **Test MC quiz with first-try XP** — Trigger MC quiz, select correct answer first try:
   - "+X XP Thưởng lần đầu!" badge appears (first-try bonus works)
   - Session stats update: Đúng lần đầu increments, XP accumulates
8. **Test session reset** — Click Reset Session, verify all stats return to 0

#### Off-screen SVG bar workaround
When SVG bars are off-screen (common when quiz dashboard sidebar takes space), use DevTools console to programmatically click:
```javascript
document.querySelector('[data-node-id="node-bar-2"]').dispatchEvent(new MouseEvent('click', {bubbles: true}))
```

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
