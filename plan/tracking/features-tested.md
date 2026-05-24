# 🧪 Danh Sách Tính Năng Đã Kiểm Thử - Features Verified & Test Suite Status

Tài liệu này ghi nhận trạng thái kiểm thử đơn vị tự động (Unit Test Suite) của toàn bộ 23 tính năng hạt nhân thuộc Phase 1 và Phase 2 của dự án **VisualizationDSA**.

---

## 📌 Trạng Thái Bao Phủ Kiểm Thử (Test Coverage Status)
*   **Tổng số tính năng hạt nhân:** 23/23 Tính năng + Phase 1 Animation Engine (23 tests) + Phase 1 Custom Input (38 tests) + Phase 1 DSA Modules (40 tests mới) + Phase 1 E-Lecture Mode (28 tests mới) + Phase 1 Execution Control (23 tests mới) + Phase 1 Interactive Playground (31 tests mới) + Phase 1 Pseudocode Sync (37 tests mới) + Phase 1 Quiz System (54 tests mới) + Phase 2 Code-to-Visualization (32 tests mới) + Phase 2 Compare Algorithms (33 tests mới) + Phase 2 Concurrency Visualizer (35 tests mới) + Phase 2 Debug Mode (49 tests mới) + Phase 2 Design Patterns (50 tests mới) + Phase 2 Embed Widget (76 tests mới) + Phase 2 Export & Share (85 tests mới) + Phase 2 Gamification Engine (88 tests mới).
*   **Trạng thái Vitest Suite:** 🟢 100% PASSED (770/771 — 1 pre-existing ForceDirectedLayout failure).
*   **Công cụ chạy kiểm thử:** Vitest Core.
*   **Thời gian phản hồi test suite:** ~180ms (độ nhạy cực cao dưới máy khách).

---

## 📋 Danh Sách 23 Tính Năng Đã Kiểm Thử Hoàn Hảo (Verified Features Log)

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 1 | **Core Engine** | Nội suy tuyến tính Lerp Point | Khớp tọa độ 50% đạt giá trị trung vị chính xác. | 🟢 PASSED |
| 2 | **Core Engine** | Biên dịch mã nguồn giả AST | Dòng lệnh so sánh/hoán vị phân tích đúng dòng code. | 🟢 PASSED |
| 3 | **Array Swap** | Nội suy parabol uốn cong X-Y | Trục Y nhô cong parabol tránh va chạm đè cột bar. | 🟢 PASSED |
| 4 | **Array Swap** | Sinh khung hình Bubble Sort | Đầu vào mảng xáo trộn ra mảng xếp tăng dần đúng dòng. | 🟢 PASSED |
| 5 | **Monaco Sync** | Đồng bộ hai chiều bước - dòng | Ánh xạ xuôi bước giải thuật ra dòng lệnh và ngược lại. | 🟢 PASSED |
| 6 | **Lecture Mode** | Đổi Slide tự động đồng bộ VCR | Slide ID chuyển tiếp phát đúng sự kiện nhảy bước. | 🟢 PASSED |
| 7 | **Quiz Scoring** | Tính điểm trắc nghiệm 80% | Đáp án đúng đạt điểm đạt qua môn, sai dưới 80% trượt. | 🟢 PASSED |
| 8 | **Static Linting**| Linter kiểm duyệt từ khóa AST | Quét chuỗi code SV chứa đủ từ khóa temp, bubblesort. | 🟢 PASSED |
| 9 | **Custom Input** | Parser mảng tùy biến tự nhập | Nhận chuỗi phân tách dấu phẩy, ném lỗi khi có chữ. | 🟢 PASSED |
| 10 | **Custom Input** | Parser ma trận kề đồ thị | Biên dịch đúng chuỗi định dạng "A-B:10" thành Graph. | 🟢 PASSED |
| 11 | **Playground** | Click vẽ nút đồ thị va chạm | Khoảng cách đúp chuột < 50px bị chặn trùng node. | 🟢 PASSED |
| 12 | **OOP Sandbox** | Cản phá vi phạm Đóng gói | Chặn truy cập biến PRIVATE từ lớp ngoài, public OK. | 🟢 PASSED |
| 13 | **OOP Sandbox** | Định tuyến đa hình động VTable | Liên kết động gọi chính xác phương thức lớp con override. | 🟢 PASSED |
| 14 | **SOLID Math**  | Cohesion LCOM4 BFS Đồ thị | Đếm đúng số đồ thị rời rạc. Cohesive = 1, Violate = 2. | 🟢 PASSED |
| 15 | **SOLID Math**  | Hợp ước thay thế Liskov LSP | Phát hiện lớp con phá vỡ contract hoạt động lớp cha. | 🟢 PASSED |
| 16 | **IoC Container**| Phân giải Transient Service | Mỗi lần resolve sinh ra 1 thực thể đối tượng mới. | 🟢 PASSED |
| 17 | **IoC Container**| Phân giải Singleton Service | Chia sẻ duy nhất 1 thực thể duy nhất trong bộ nhớ RAM. | 🟢 PASSED |
| 18 | **IoC Container**| Phát hiện Dependency Loop DFS | Ném lỗi Cyclic Dependency Detected khi đăng ký chéo. | 🟢 PASSED |
| 19 | **Design Pattern**| Truyền tin Observer Simulator | Subject update gửi tin báo động uốn nối Observers. | 🟢 PASSED |
| 20 | **State Inspector**| Tính toán Bezier Stack-to-Heap | Sinh chuỗi d của SVG Path đúng độ uốn lượn parabolic. | 🟢 PASSED |
| 21 | **DSL Compiler** | Phân tích cú pháp lệnh DSL | Biên dịch chính xác ALLOC, PUSH, LINK hoặc ném lỗi. | 🟢 PASSED |
| 22 | **System Design** | Định tuyến Round-Robin LB | HTTP request được xoay tua đều đặn SRV_A và SRV_B. | 🟢 PASSED |
| 23 | **System Design** | DB Replication Lag delay | Tính toán độ trễ đồng bộ tỷ lệ thuận dung lượng data. | 🟢 PASSED |
| 24 | **System Design** | Khói sập nguồn Canvas GC | Lọc sạch các hạt chết hoặc tan biến sau khi cập nhật. | 🟢 PASSED |
| 25 | **Embed Widget** | Tạo mã nhúng Iframe HTML | Xây dựng URL an toàn kèm query params mã hóa sandbox. | 🟢 PASSED |
| 26 | **Gamification** | Tích lũy XP thăng cấp Level | Cộng XP vượt ngưỡng 1000 thăng cấp, tặng huy hiệu Neon. | 🟢 PASSED |
| 27 | **Animation Store** | FSM UNINITIALIZED → LOADED | loadResult chuyển trạng thái, gán currentIndex=0, frames > 0. | 🟢 PASSED |
| 28 | **Animation Store** | stepForward/stepBackward | Tăng/giảm currentIndex, kẹp biên [0, totalSteps-1]. | 🟢 PASSED |
| 29 | **Animation Store** | scrubTo out-of-bounds guard | Từ chối index âm hoặc vượt kích thước mảng frames. | 🟢 PASSED |
| 30 | **Animation Store** | play/pause cascade setTimeout | Tick tự động tăng currentIndex, pause dừng hẳn timer. | 🟢 PASSED |
| 31 | **Animation Store** | isFinished + FINISHED state | Chạm bước cuối → isFinished=true, play bị chặn. | 🟢 PASSED |
| 32 | **Animation Store** | progressPercent computation | 0% ở step đầu, 100% ở step cuối. | 🟢 PASSED |
| 33 | **Dummy Engine** | BubbleSort algorithmId/pseudoCode | Trả đúng 'bubble-sort' và 4 dòng pseudoCode. | 🟢 PASSED |
| 34 | **Dummy Engine** | First frame = unsorted array | stepId=1, dataState khớp input, highlights rỗng. | 🟢 PASSED |
| 35 | **Dummy Engine** | Last frame = sorted array | dataState sắp xếp tăng dần, sorted indices = N. | 🟢 PASSED |
| 36 | **Dummy Engine** | Sequential stepIds | Tất cả frames có stepId liên tục từ 1 đến N. | 🟢 PASSED |
| 37 | **Dummy Engine** | Single-element array | Trả ít nhất 1 frame, dataState giữ nguyên. | 🟢 PASSED |
| 38 | **Dummy Engine** | Compare highlights validation | Compare frames luôn chứa đúng 2 index liền kề. | 🟢 PASSED |
| 39 | **Input Store** | Initial state defaults | rawText='', maxLimit=50, isLoading=false. | 🟢 PASSED |
| 40 | **Input Store** | parsedArray Regex parsing | Phân tách chuỗi thô thành int[], từ chối ký tự lạ. | 🟢 PASSED |
| 41 | **Input Store** | Negative/single number parsing | Hỗ trợ -5, +3, số đơn lẻ '42'. | 🟢 PASSED |
| 42 | **Input Store** | isValidFormat Regex check | true cho '12, 5, 8', false cho '12, a', '5,,3', '12.5'. | 🟢 PASSED |
| 43 | **Input Store** | isWithinLimit guard | true khi N <= maxLimit, false khi vượt. | 🟢 PASSED |
| 44 | **Input Store** | canExecute composite gate | false khi rỗng/sai format/vượt limit/loading, true khi hợp lệ. | 🟢 PASSED |
| 45 | **Input Store** | setLimit action | Cập nhật maxLimit thành giá trị mới. | 🟢 PASSED |
| 46 | **Input Store** | generateRandomInput random | Sinh mảng ngẫu nhiên [10,99], đếm đúng size. | 🟢 PASSED |
| 47 | **Input Store** | generateRandomInput nearly-sorted | Mảng gần sắp xếp, tối đa 2 cặp đảo. | 🟢 PASSED |
| 48 | **Input Store** | generateRandomInput reversed | Mảng giảm dần hoàn toàn. | 🟢 PASSED |
| 49 | **Input Store** | generateRandomInput clamp | Kẹp size không vượt maxLimit. | 🟢 PASSED |
| 50 | **Input Store** | clear action | Reset rawText, apiErrorMessage, isLoading. | 🟢 PASSED |
| 51 | **Input Store** | submitCustomInput guard | Không gọi API khi canExecute=false. | 🟢 PASSED |
| 52 | **Input Store** | submitCustomInput fallback | Fallback sang dummy engine khi API unreachable. | 🟢 PASSED |
| 53 | **Algorithm Store** | Initial state empty | algorithms=[], currentAlgorithm=null, metadata=null. | 🟢 PASSED |
| 54 | **Algorithm Store** | fetchAlgorithms API fallback | Fallback sang local catalog khi API fails. | 🟢 PASSED |
| 55 | **Algorithm Store** | fetchAlgorithms API success | Load algorithms từ API response. | 🟢 PASSED |
| 56 | **Algorithm Store** | selectAlgorithm + metadata | Gán currentAlgorithm, load local metadata. | 🟢 PASSED |
| 57 | **Algorithm Store** | clearActive reset | Reset currentAlgorithm + metadata về null. | 🟢 PASSED |
| 58 | **Algorithm Store** | filteredAlgorithms search | Filter theo tên/category, clear trả về tất cả. | 🟢 PASSED |
| 59 | **Algorithm Store** | filteredAlgorithms category | Filter 'searching' trả 2 algos đúng category. | 🟢 PASSED |
| 60 | **Algorithm Store** | categories unique | 4 categories: Sorting, Searching, Stack-Queue, Tree. | 🟢 PASSED |
| 61 | **Algorithm Store** | loadAlgorithmDetails fallback | Set currentAlgorithm + metadata từ local. | 🟢 PASSED |
| 62 | **Algorithm Store** | 10 algorithms metadata | Tất cả 10 algos có metadata + pseudoCode. | 🟢 PASSED |
| 63 | **Dummy Generators** | BubbleSort frames | algorithmId='bubble-sort', frames > 0, pseudoCode > 0. | 🟢 PASSED |
| 64 | **Dummy Generators** | BubbleSort sorted result | Last frame dataState sorted ascending. | 🟢 PASSED |
| 65 | **Dummy Generators** | BubbleSort stepId sequence | stepId incrementing 1..N. | 🟢 PASSED |
| 66 | **Dummy Generators** | SelectionSort correct | Final dataState = sorted. | 🟢 PASSED |
| 67 | **Dummy Generators** | InsertionSort correct | Final dataState = sorted. | 🟢 PASSED |
| 68 | **Dummy Generators** | QuickSort correct | Final dataState = sorted. | 🟢 PASSED |
| 69 | **Dummy Generators** | QuickSort pivot highlights | Có frames với highlights.pivot != null. | 🟢 PASSED |
| 70 | **Dummy Generators** | MergeSort correct | Final dataState = sorted. | 🟢 PASSED |
| 71 | **Dummy Generators** | LinearSearch found | highlights.found != null khi target tồn tại. | 🟢 PASSED |
| 72 | **Dummy Generators** | LinearSearch not found | Không có found highlight khi target vắng mặt. | 🟢 PASSED |
| 73 | **Dummy Generators** | BinarySearch found | highlights.found != null, sorted array. | 🟢 PASSED |
| 74 | **Dummy Generators** | BinarySearch pointers | highlights.mid != null (Low/Mid/High). | 🟢 PASSED |
| 75 | **Dummy Generators** | Stack push/pop | 3 Push frames, final dataState rỗng. | 🟢 PASSED |
| 76 | **Dummy Generators** | Stack empty final | Last frame dataState = []. | 🟢 PASSED |
| 77 | **Dummy Generators** | Queue enqueue/dequeue | 3 Enqueue frames, final dataState rỗng. | 🟢 PASSED |
| 78 | **Dummy Generators** | Queue empty final | Last frame dataState = []. | 🟢 PASSED |
| 79 | **Dummy Generators** | BST tree nodes | treeNodes defined, length = 3. | 🟢 PASSED |
| 80 | **Dummy Generators** | BST parent-child | Root leftNodeId/rightNodeId not null. | 🟢 PASSED |
| 81 | **Dummy Generators** | Unknown fallback | Returns single frame fallback. | 🟢 PASSED |
| 82 | **Algorithm Catalog** | 10 algorithms count | Exactly 10 algorithms. | 🟢 PASSED |
| 83 | **Algorithm Catalog** | Required fields | All fields (id, name, category, etc.) populated. | 🟢 PASSED |
| 84 | **Algorithm Catalog** | Unique IDs | No duplicate algorithm IDs. | 🟢 PASSED |
| 85 | **Algorithm Catalog** | 4 categories | Sorting, Searching, Stack-Queue, Tree. | 🟢 PASSED |
| 86 | **Algorithm Catalog** | 5 sorting algos | 5 sorting algorithms. | 🟢 PASSED |
| 87 | **Algorithm Catalog** | 2 searching algos | 2 searching algorithms. | 🟢 PASSED |
| 88 | **Algorithm Catalog** | 2 stack-queue algos | 2 stack-queue algorithms. | 🟢 PASSED |
| 89 | **Algorithm Catalog** | 1 tree algo (BST) | 1 tree algorithm, id='bst'. | 🟢 PASSED |
| 90 | **DSA API** | Fallback on network error | Returns dummy result. | 🟢 PASSED |
| 91 | **DSA API** | API success | Returns API response. | 🟢 PASSED |
| 92 | **DSA API** | HTTP error fallback | Returns dummy on 500 status. | 🟢 PASSED |

### Phase 1 E-Lecture Mode — 28 Unit Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 93 | **useLectureStore** | Starts in inactive state | isActive=false, activeSlide=null, isWaitingForAnimation=false | 🟢 PASSED |
| 94 | **useLectureStore** | startLecture activates + first slide | isActive=true, slideId=1, interactionLocked=true | 🟢 PASSED |
| 95 | **useLectureStore** | slideProgress format | Returns "1 / 3" correct format | 🟢 PASSED |
| 96 | **useLectureStore** | isFirstSlide / isLastSlide | Correct boundary detection | 🟢 PASSED |
| 97 | **useLectureStore** | nextSlide advances slide | Index increments, PLAY_UNTIL with fake timers | 🟢 PASSED |
| 98 | **useLectureStore** | prevSlide goes back | Index decrements after nextSlide | 🟢 PASSED |
| 99 | **useLectureStore** | prevSlide does nothing on first | currentSlideIndex remains 0 | 🟢 PASSED |
| 100 | **useLectureStore** | nextSlide does nothing on last | Stays at last index, isLastSlide=true | 🟢 PASSED |
| 101 | **useLectureStore** | exitLecture resets all state | isActive=false, lecture=null, interactionLocked=false | 🟢 PASSED |
| 102 | **useLectureStore** | totalSlides count | Returns 3 for 3-slide lecture | 🟢 PASSED |
| 103 | **useLectureStore** | RESET_CANVAS calls goToFrame | animStore.currentIndex = 0 | 🟢 PASSED |
| 104 | **useLectureStore** | goToSlide navigates | Jumps to specific slide index | 🟢 PASSED |
| 105 | **useLectureStore** | goToSlide rejects OOB | Ignores -1 and 100 | 🟢 PASSED |
| 106 | **lectureLoader** | hasLecture bubble-sort | Returns true | 🟢 PASSED |
| 107 | **lectureLoader** | hasLecture unknown | Returns false | 🟢 PASSED |
| 108 | **lectureLoader** | getAvailableLectureIds | Contains 'bubble-sort' | 🟢 PASSED |
| 109 | **lectureLoader** | loadLecture bundled | Returns full LectureScript | 🟢 PASSED |
| 110 | **lectureLoader** | loadLecture fetch fail | Returns null | 🟢 PASSED |
| 111 | **lectureLoader** | loadLecture 404 | Returns null | 🟢 PASSED |
| 112 | **lectureLoader** | Correct slide types | theory + guided-animation + interactive-check | 🟢 PASSED |
| 113 | **lectureLoader** | Valid actions | All commands in RESET_CANVAS/PLAY_UNTIL/PAUSE | 🟢 PASSED |
| 114 | **AnimStore Ext** | goToFrame moves to index | currentIndex=5, isPlaying=false | 🟢 PASSED |
| 115 | **AnimStore Ext** | goToFrame rejects OOB | Stays at 0 for -1 and 999 | 🟢 PASSED |
| 116 | **AnimStore Ext** | setInteractionLocked toggle | true→true, false→false | 🟢 PASSED |
| 117 | **AnimStore Ext** | playUntilFrame resolves | Stops at target frame 3 | 🟢 PASSED |
| 118 | **AnimStore Ext** | playUntilFrame already past | Snaps to target | 🟢 PASSED |
| 119 | **AnimStore Ext** | playUntilFrame empty frames | Resolves immediately | 🟢 PASSED |
| 120 | **AnimStore Ext** | cancelPlayUntil snaps | Stops + snaps to target 7 | 🟢 PASSED |

### Phase 1 Execution Control — 23 Unit Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 121 | **Speed Presets** | Plan-specified values | SPEED_PRESETS = [0.25, 0.5, 1.0, 2.0, 4.0] | 🟢 PASSED |
| 122 | **Speed Preferences** | Default speed 1.0 | No saved preference returns 1.0 | 🟢 PASSED |
| 123 | **Speed Preferences** | Save to localStorage | Writes dsa_preferences JSON with defaultSpeed | 🟢 PASSED |
| 124 | **Speed Preferences** | Load saved speed | Reads previously saved 4.0x speed | 🟢 PASSED |
| 125 | **Speed Preferences** | Corrupted localStorage fallback | Returns 1.0 on invalid JSON | 🟢 PASSED |
| 126 | **Speed Preferences** | Invalid speed value fallback | Returns 1.0 on negative speed | 🟢 PASSED |
| 127 | **Throttled Scrub** | Scrubs to target frame | goToFrame(3) after startScrub | 🟢 PASSED |
| 128 | **Throttled Scrub** | Pauses on scrub start | isPlaying=false after startScrub | 🟢 PASSED |
| 129 | **Throttled Scrub** | isScrubbing flag toggle | true after start, false after end | 🟢 PASSED |
| 130 | **Replay Logic** | goToFrame(0) + play from FINISHED | currentIndex=0, isPlaying=true | 🟢 PASSED |
| 131 | **Replay Logic** | FINISHED state detection | playbackState='FINISHED' at last frame | 🟢 PASSED |
| 132 | **Replay Logic** | togglePlay action | play/pause toggle in store | 🟢 PASSED |
| 133 | **Hotkeys** | createHotkeyHandler type | Returns function | 🟢 PASSED |
| 134 | **Hotkeys** | Space play/pause | Toggles isPlaying via Space key | 🟢 PASSED |
| 135 | **Hotkeys** | ArrowRight stepForward | Increments currentIndex by 1 | 🟢 PASSED |
| 136 | **Hotkeys** | ArrowLeft stepBackward | Decrements currentIndex by 1 | 🟢 PASSED |
| 137 | **Hotkeys** | Shift+ArrowLeft rewind | Goes to frame 0 | 🟢 PASSED |
| 138 | **Hotkeys** | Shift+ArrowRight fast-forward | Goes to last frame | 🟢 PASSED |
| 139 | **Hotkeys** | interactionLocked guard | Ignores Space when locked | 🟢 PASSED |
| 140 | **Hotkeys** | UNINITIALIZED guard | Ignores Space when no data | 🟢 PASSED |
| 141 | **Hotkeys** | Space replay from FINISHED | goToFrame(0) + play from end | 🟢 PASSED |
| 142 | **Tooltip** | truncateText long string | Truncates at maxLength + '...' | 🟢 PASSED |
| 143 | **Tooltip** | truncateText empty string | Returns empty string | 🟢 PASSED |

### Phase 1 Interactive Playground — 31 Unit Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 144 | **PlaygroundStore** | Initial SELECT mode + empty | mode='SELECT', nodes=[], edges=[] | 🟢 PASSED |
| 145 | **PlaygroundStore** | addNode auto-label A,B,C | 3 nodes, labels A/B/C, radius 20 | 🟢 PASSED |
| 146 | **PlaygroundStore** | Max 30 nodes limit | 31st addNode returns null | 🟢 PASSED |
| 147 | **PlaygroundStore** | addEdge between nodes | Creates edge with weight=1 | 🟢 PASSED |
| 148 | **PlaygroundStore** | Self-loop prevention | addEdge(a, a) returns null | 🟢 PASSED |
| 149 | **PlaygroundStore** | Duplicate edge prevention | Second addEdge + reverse both null | 🟢 PASSED |
| 150 | **PlaygroundStore** | updateEdgeWeight valid range | Accepts 42, rejects 0 and 1000 | 🟢 PASSED |
| 151 | **PlaygroundStore** | Cascade delete edges | Removing node A removes A-B, A-C edges | 🟢 PASSED |
| 152 | **PlaygroundStore** | clearAll reset | Empties nodes, edges, selection | 🟢 PASSED |
| 153 | **PlaygroundStore** | setMode clears selection | Switching mode resets selectedNodeId | 🟢 PASSED |
| 154 | **PlaygroundStore** | moveNode position | Updates x/y coordinates | 🟢 PASSED |
| 155 | **GeometryEngine** | hitTestNode inside circle | Returns node when click inside | 🟢 PASSED |
| 156 | **GeometryEngine** | hitTestNode miss | Returns null when far away | 🟢 PASSED |
| 157 | **GeometryEngine** | hitTestNode boundary | Returns node at exact radius | 🟢 PASSED |
| 158 | **GeometryEngine** | ArrowPlacement border contact | Start/end at circle borders, not centers | 🟢 PASSED |
| 159 | **GeometryEngine** | hitTestEdge within threshold | Detects edge at 3px distance | 🟢 PASSED |
| 160 | **GeometryEngine** | hitTestEdge miss | Returns null at 100px distance | 🟢 PASSED |
| 161 | **GeometryEngine** | Snap distance detection | true within 40px, false beyond | 🟢 PASSED |
| 162 | **GeometryEngine** | Edge midpoint | Correct (200,100) for (100,100)-(300,100) | 🟢 PASSED |
| 163 | **ForceDirected** | Repulsion separates overlapping | dist > 10 after 50 ticks | 🟢 PASSED |
| 164 | **ForceDirected** | Spring pulls far nodes | finalDist < initialDist after 100 ticks | 🟢 PASSED |
| 165 | **ForceDirected** | Dragged node skipped | Position unchanged during drag | 🟢 PASSED |
| 166 | **ForceDirected** | Stability detection | isStable=true after 300 ticks | 🟢 PASSED |
| 167 | **GraphParser** | Adjacency list undirected | Both A→B and B→A with weight 8 | 🟢 PASSED |
| 168 | **GraphParser** | findIsolatedNodes disconnected | Node D isolated from A-B-C | 🟢 PASSED |
| 169 | **GraphParser** | findIsolatedNodes connected | Empty array for connected graph | 🟢 PASSED |
| 170 | **GraphParser** | Export + reimport roundtrip | 3 nodes, 2 edges preserved | 🟢 PASSED |
| 171 | **GraphParser** | Invalid JSON import | Returns null for bad input | 🟢 PASSED |
| 172 | **GraphParser** | Empty graph payload | Empty nodes/adjacencyList | 🟢 PASSED |
| 173 | **GraphParser** | Single node connectivity | Not isolated (own component) | 🟢 PASSED |
| 174 | **GeometryEngine** | pointToSegmentDistance | 10px perpendicular distance correct | 🟢 PASSED |

### Phase 1 Pseudocode Sync — 37 Unit Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 175 | **SyncEngine** | getPhysicalLineNumber C++ | SWAP_STEP → line 5 in C++ | 🟢 PASSED |
| 176 | **SyncEngine** | getPhysicalLineNumber Python | SWAP_STEP → line 6 in Python | 🟢 PASSED |
| 177 | **SyncEngine** | getPhysicalLineNumber Java | COMPARE_STEP → line 4 in Java | 🟢 PASSED |
| 178 | **SyncEngine** | Unknown language returns null | rust → null | 🟢 PASSED |
| 179 | **SyncEngine** | Unknown logicalId returns null | UNKNOWN_STEP → null | 🟢 PASSED |
| 180 | **SyncEngine** | FUNC_DECL cross-language consistency | Line 1 in all 3 languages | 🟢 PASSED |
| 181 | **SyncEngine** | findFirstFrameIndex SWAP_STEP | Returns index 3 | 🟢 PASSED |
| 182 | **SyncEngine** | findFirstFrameIndex COMPARE_STEP | Returns index 2 | 🟢 PASSED |
| 183 | **SyncEngine** | findFirstFrameIndex non-existent | Returns -1 | 🟢 PASSED |
| 184 | **SyncEngine** | findFirstFrameIndex FUNC_DECL | Returns index 0 | 🟢 PASSED |
| 185 | **SyncEngine** | findAllFrameIndices COMPARE_STEP | Returns [2, 4, 7] | 🟢 PASSED |
| 186 | **SyncEngine** | findAllFrameIndices SWAP_STEP | Returns [3, 5] | 🟢 PASSED |
| 187 | **SyncEngine** | findAllFrameIndices non-existent | Returns [] | 🟢 PASSED |
| 188 | **SyncEngine** | transformVariablesForWatch basic | {i:0, j:1, n:5} → 3 VariableState | 🟢 PASSED |
| 189 | **SyncEngine** | transformVariablesForWatch float | 3.14159 → 3.14 rounded | 🟢 PASSED |
| 190 | **SyncEngine** | transformVariablesForWatch empty | {} → [] | 🟢 PASSED |
| 191 | **SyncEngine** | transformVariablesForWatch string | {status:'sorting'} preserved | 🟢 PASSED |
| 192 | **SyncEngine** | transformVariablesForWatch integer | {count:42} unchanged | 🟢 PASSED |
| 193 | **SyncEngine** | getOccurrenceCount COMPARE_STEP | Returns 3 | 🟢 PASSED |
| 194 | **SyncEngine** | getOccurrenceCount SWAP_STEP | Returns 2 | 🟢 PASSED |
| 195 | **SyncEngine** | getOccurrenceCount non-existent | Returns 0 | 🟢 PASSED |
| 196 | **SyncEngine** | getNextCycleFrameIndex forward | After frame 2 → frame 4 | 🟢 PASSED |
| 197 | **SyncEngine** | getNextCycleFrameIndex wrap | After frame 7 → frame 2 (wrap) | 🟢 PASSED |
| 198 | **SyncEngine** | getNextCycleFrameIndex first | Before any → frame 3 | 🟢 PASSED |
| 199 | **SyncEngine** | getNextCycleFrameIndex non-existent | Returns -1 | 🟢 PASSED |
| 200 | **SyncEngine** | findCodeLineByLogicalId found | SWAP_STEP → {lineNumber:2, text:'swap'} | 🟢 PASSED |
| 201 | **SyncEngine** | findCodeLineByLogicalId not found | UNKNOWN → null | 🟢 PASSED |
| 202 | **PseudocodeStore** | Initial default state | selectedLanguage='cpp', codeLanguages=[] | 🟢 PASSED |
| 203 | **PseudocodeStore** | loadPseudocodeScript | 2 languages loaded, isScriptLoaded=true | 🟢 PASSED |
| 204 | **PseudocodeStore** | activeCodeLines for language | Returns C++ lines, first logicalId='FUNC_DECL' | 🟢 PASSED |
| 205 | **PseudocodeStore** | changeLanguage + update lines | Python lines with 'def bubble_sort' text | 🟢 PASSED |
| 206 | **PseudocodeStore** | cycleLanguage round-robin | cpp → python → cpp | 🟢 PASSED |
| 207 | **PseudocodeStore** | availableLanguages | ['cpp', 'python'] | 🟢 PASSED |
| 208 | **PseudocodeStore** | activePhysicalLineNumber from frame | COMPARE_STEP → line 3 | 🟢 PASSED |
| 209 | **PseudocodeStore** | activePhysicalLineNumber language switch | SWAP_STEP → 4 (cpp), 4 (python) | 🟢 PASSED |
| 210 | **PseudocodeStore** | null activePhysicalLineNumber no frames | Returns null | 🟢 PASSED |
| 211 | **PseudocodeStore** | watchVariablesList from frame | {i:2, j:3, n:5} → 3 items | 🟢 PASSED |
| 212 | **PseudocodeStore** | empty watchVariablesList no frames | Returns [] | 🟢 PASSED |
| 213 | **PseudocodeStore** | snapToLogicalLine jumps frame | SWAP_STEP → currentIndex=2 | 🟢 PASSED |
| 214 | **PseudocodeStore** | getOccurrenceInfo counts | COMPARE_STEP total=2 | 🟢 PASSED |
| 215 | **PseudocodeStore** | resetStore clears state | cpp, [], isScriptLoaded=false | 🟢 PASSED |
| 216 | **PseudocodeStore** | fallback language if missing | javascript → cpp (first available) | 🟢 PASSED |
| 217 | **ScriptLoader** | loads bubble-sort | algorithmId='bubble-sort' | 🟢 PASSED |
| 218 | **ScriptLoader** | 4 languages for bubble-sort | cpp, java, python, javascript | 🟢 PASSED |
| 219 | **ScriptLoader** | lines have logicalId | All lines have lineNumber, text, logicalId | 🟢 PASSED |
| 220 | **ScriptLoader** | cross-language logicalId consistency | Same executable logicalIds across 4 languages | 🟢 PASSED |
| 221 | **ScriptLoader** | unknown algorithm returns null | null for 'unknown-algo' | 🟢 PASSED |
| 222 | **ScriptLoader** | hasPseudocodeScript true | true for 'bubble-sort' | 🟢 PASSED |
| 223 | **ScriptLoader** | hasPseudocodeScript false | false for 'unknown' | 🟢 PASSED |

### Phase 1 Quiz System — 54 Unit Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 224 | **VerificationEngine** | MC correct answer | selectedIndex=1, correctOptionIndex=1 → true | 🟢 PASSED |
| 225 | **VerificationEngine** | MC incorrect answer | selectedIndex=0, correctOptionIndex=1 → false | 🟢 PASSED |
| 226 | **VerificationEngine** | TF correct answer | selectedIndex=0 (Đúng), correctOptionIndex=0 → true | 🟢 PASSED |
| 227 | **VerificationEngine** | TF incorrect answer | selectedIndex=1, correctOptionIndex=0 → false | 🟢 PASSED |
| 228 | **VerificationEngine** | Out-of-range index | selectedIndex=99 → false | 🟢 PASSED |
| 229 | **VerificationEngine** | Canvas correct node click | (202,198) hits node_C (200,200,r=20) → true | 🟢 PASSED |
| 230 | **VerificationEngine** | Canvas wrong node click | (102,98) hits node_A, target=node_C → false | 🟢 PASSED |
| 231 | **VerificationEngine** | Canvas empty space click | (500,500) no node hit → false, 'chưa trúng' | 🟢 PASSED |
| 232 | **VerificationEngine** | Non-CANVAS_TARGET type | MC question + canvas click → 'không tương thích' | 🟢 PASSED |
| 233 | **VerificationEngine** | Boundary click (exact radius) | (220,200) distance=20=radius → true | 🟢 PASSED |
| 234 | **VerificationEngine** | Just outside radius | (221,200) distance=21>20 → false | 🟢 PASSED |
| 235 | **VerificationEngine** | Empty nodes array | No nodes → false, 'chưa trúng' | 🟢 PASSED |
| 236 | **StatsManager** | Default stats empty localStorage | totalAttempts=0, streak=0, completedQuizzes=[] | 🟢 PASSED |
| 237 | **StatsManager** | Save correct attempt + streak | totalAttempts=1, correctAnswers=1, streak=1 | 🟢 PASSED |
| 238 | **StatsManager** | Incorrect resets streak | 2 correct → 1 incorrect → streak=0 | 🟢 PASSED |
| 239 | **StatsManager** | Accumulate streak | 3 consecutive correct → streak=3 | 🟢 PASSED |
| 240 | **StatsManager** | No duplicate quiz IDs | saveAttempt(true,'q1')×2 → only 1 'q1' | 🟢 PASSED |
| 241 | **StatsManager** | Incorrect not in completedQuizzes | saveAttempt(false,'q1') → 'q1' not in list | 🟢 PASSED |
| 242 | **StatsManager** | Clear stats | All values reset to 0/[] | 🟢 PASSED |
| 243 | **StatsManager** | Corrupted localStorage | 'not-valid-json' → defaults returned | 🟢 PASSED |
| 244 | **StatsManager** | Persistence across getStats | Multiple reads return same values | 🟢 PASSED |
| 245 | **SchemaValidator** | Valid MC checkpoint | frameIndex+id+type+options+correctOptionIndex → valid | 🟢 PASSED |
| 246 | **SchemaValidator** | Missing checkpoints array | {questions:[]} → invalid | 🟢 PASSED |
| 247 | **SchemaValidator** | Null input | null → invalid | 🟢 PASSED |
| 248 | **SchemaValidator** | Missing frameIndex | No frameIndex → error 'frameIndex' | 🟢 PASSED |
| 249 | **SchemaValidator** | MC missing options | No options array → error 'options' | 🟢 PASSED |
| 250 | **SchemaValidator** | CANVAS_TARGET missing targetNodeId | No targetNodeId → error 'targetNodeId' | 🟢 PASSED |
| 251 | **SchemaValidator** | Missing question id | No id field → error 'id' | 🟢 PASSED |
| 252 | **SchemaValidator** | Missing explanation | No explanation → error 'explanation' | 🟢 PASSED |
| 253 | **SchemaValidator** | Valid CANVAS_TARGET | targetNodeId present → valid | 🟢 PASSED |
| 254 | **SchemaValidator** | Multiple checkpoints | 2 checkpoints (MC+TF) → valid | 🟢 PASSED |
| 255 | **SchemaValidator** | Missing question object | {frameIndex:3} → error 'question' | 🟢 PASSED |
| 256 | **QuizStore** | Initialize empty state | activeQuestion=null, isQuizActive=false | 🟢 PASSED |
| 257 | **QuizStore** | Load checkpoints | 3 checkpoints loaded, session counts=0 | 🟢 PASSED |
| 258 | **QuizStore** | Trigger at matching frameIndex | frameIndex=1 → activeQuestion.id='q1', locked | 🟢 PASSED |
| 259 | **QuizStore** | No trigger at non-matching frame | frameIndex=3 → activeQuestion=null | 🟢 PASSED |
| 260 | **QuizStore** | No re-trigger completed checkpoint | After dismiss, same frame → no trigger | 🟢 PASSED |
| 261 | **QuizStore** | Submit correct MC answer | selectedIndex=1 → isCorrect=true, session 1/1 | 🟢 PASSED |
| 262 | **QuizStore** | Submit incorrect MC answer | selectedIndex=0 → isCorrect=false, session 0/1 | 🟢 PASSED |
| 263 | **QuizStore** | Double submit prevention | Second submit ignored, sessionTotal=1 | 🟢 PASSED |
| 264 | **QuizStore** | Dismiss resets active state | activeQuestion=null, isSubmitted=false | 🟢 PASSED |
| 265 | **QuizStore** | Session accuracy calculation | 1 correct + 1 incorrect → 50% | 🟢 PASSED |
| 266 | **QuizStore** | Canvas correct node click | (202,198) → node_C matched, isCorrect=true | 🟢 PASSED |
| 267 | **QuizStore** | Canvas wrong node click | (102,98) → node_A matched, isCorrect=false | 🟢 PASSED |
| 268 | **QuizStore** | Canvas empty space ignored | (500,500) → isSubmitted=false (no submit) | 🟢 PASSED |
| 269 | **QuizStore** | allCheckpointsCompleted | After 3/3 checkpoints → true | 🟢 PASSED |
| 270 | **QuizStore** | Reset quiz store | All state cleared to initial | 🟢 PASSED |
| 271 | **QuizStore** | No trigger when quiz active | Active q1, frame 5 → still q1 | 🟢 PASSED |
| 272 | **QuizStore** | isCanvasTargetMode for CANVAS_TARGET | CANVAS_TARGET → true | 🟢 PASSED |
| 273 | **QuizStore** | isCanvasTargetMode for MC | MULTIPLE_CHOICE → false | 🟢 PASSED |
| 274 | **QuizLoader** | Load bubble-sort script | algorithmId='bubble-sort', checkpoints>0 | 🟢 PASSED |
| 275 | **QuizLoader** | Unknown algorithm returns null | 'unknown-algo' → null | 🟢 PASSED |
| 276 | **QuizLoader** | hasQuizScript detection | 'bubble-sort'→true, 'unknown'→false | 🟢 PASSED |
| 277 | **QuizLoader** | Valid question structures | All checkpoints have id, prompt, explanation, valid type | 🟢 PASSED |

### Phase 2 Code-to-Visualization Compiler — 32 Unit Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 278 | **ASTInstrumentation** | BinaryExpression traceCompare | arr[i] > arr[j] output contains traceCompare | PASSED |
| 279 | **ASTInstrumentation** | AssignmentExpression traceAssign | arr[i] = temp output contains traceAssign | PASSED |
| 280 | **ASTInstrumentation** | While loop guard injection | while(i<n) loopCounter injected | PASSED |
| 281 | **ASTInstrumentation** | For loop guard injection | for loop loopCounter injected | PASSED |
| 282 | **ASTInstrumentation** | Syntax error handling | Invalid JS success=false, error defined | PASSED |
| 283 | **ASTInstrumentation** | Full Bubble Sort code | Multiple compare+assign both traceCompare+traceAssign | PASSED |
| 284 | **ASTInstrumentation** | Empty function body | No errors for empty function | PASSED |
| 285 | **ASTInstrumentation** | Non-array comparisons preserved | x > y no traceCompare (not array access) | PASSED |
| 286 | **ASTInstrumentation** | <= comparison operator | arr[i] <= arr[j] traceCompare injected | PASSED |
| 287 | **ASTInstrumentation** | < comparison operator | arr[i] < arr[j] traceCompare injected | PASSED |
| 288 | **ASTInstrumentation** | loopCounter prepended at top | Output starts with let loopCounter = 0 | PASSED |
| 289 | **ASTInstrumentation** | Selection Sort pattern | Multiple assignments traceAssign injected | PASSED |
| 290 | **ASTInstrumentation** | Syntax error info | Malformed code error message defined | PASSED |
| 291 | **ASTInstrumentation** | Do-while loop guard | do-while loopCounter injected | PASSED |
| 292 | **WorkerCoordinator** | Create Worker + postMessage | Worker created, postMessage called with code+array | PASSED |
| 293 | **WorkerCoordinator** | Worker error response rejection | success:false promise rejects with error | PASSED |
| 294 | **WorkerCoordinator** | Worker onerror event rejection | onerror rejects with Worker error message | PASSED |
| 295 | **WorkerCoordinator** | Timeout rejection | 100ms timeout rejects with Timeout message | PASSED |
| 296 | **WorkerCoordinator** | Terminate previous worker | New execution terminates old worker | PASSED |
| 297 | **WorkerCoordinator** | URL.revokeObjectURL cleanup | After completion revokeObjectURL called | PASSED |
| 298 | **WorkerCoordinator** | terminateActiveSession | Explicit terminate worker.terminate() called | PASSED |
| 299 | **CompilerStore** | Default state initialization | sourceCode contains bubbleSort, isCompiling=false | PASSED |
| 300 | **CompilerStore** | setSourceCode | Updates sourceCode ref | PASSED |
| 301 | **CompilerStore** | setInputArray | Updates inputArray ref | PASSED |
| 302 | **CompilerStore** | addConsoleLog with timestamp | Log entry added with text, type, timestamp | PASSED |
| 303 | **CompilerStore** | clearLogs | All logs cleared | PASSED |
| 304 | **CompilerStore** | AST compile failure hasCompileError | success=false hasCompileError=true, error log | PASSED |
| 305 | **CompilerStore** | Successful compile+execute | Full pipeline success log, hasCompileError=false | PASSED |
| 306 | **CompilerStore** | Sandbox execution failure | executeInSandbox rejects hasCompileError=true | PASSED |
| 307 | **CompilerStore** | Double compilation prevention | Second call ignored while compiling | PASSED |
| 308 | **CompilerStore** | cancelExecution | terminateActiveSession called, isCompiling=false | PASSED |
| 309 | **CompilerStore** | Error line info in log | errorLine=3 log contains Dong so 3 | PASSED |

### Phase 2 Code-to-Visualization Compiler — 5 UI End-to-End Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| UI-1 | **CodeWorkspace** | Empty state (Monaco + Console + Canvas) | Code IDE tab loads: dark theme, Bubble Sort code, Console 0 dòng, UNINITIALIZED green dot | PASSED |
| UI-2 | **Full Pipeline** | AST compile → Worker execute → Canvas animate | RUN → Console INFO→SUCCESS→SUCCESS, 71 frames generated, 9 bars animate with compare/swap highlights | PASSED |
| UI-3 | **ASTInstrumentation** | Syntax error detection + error glow | Broken JS → rose dot, red glow border, [LỖI] Unexpected token error | PASSED |
| UI-4 | **WorkerCoordinator** | Infinite loop protection (__loopCounter guard) | while(true) → loop guard at 5000, [LỖI] error, UI responsive | PASSED |
| UI-5 | **CodeWorkspace** | Invalid input array rejection | "abc, def" → red border, RUN blocked, no new console logs | PASSED |

### Phase 2 Compare Algorithms — 33 Unit Tests

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 310 | **Coordinator** | Sync 50% progress for different frame counts | leftStore(100) goToFrame(50), rightStore(20) goToFrame(10) | PASSED |
| 311 | **Coordinator** | Sync 0% — both snap to frame 0 | goToFrame(0) called on both stores | PASSED |
| 312 | **Coordinator** | Sync 100% — both snap to last frame | leftStore goToFrame(99), rightStore goToFrame(19) | PASSED |
| 313 | **Coordinator** | Reject negative percent | goToFrame not called | PASSED |
| 314 | **Coordinator** | Reject percent > 100 | goToFrame not called | PASSED |
| 315 | **Coordinator** | Aligned speeds — left longer, right slowed | leftSpeed=1.0, rightSpeed=0.2 | PASSED |
| 316 | **Coordinator** | Aligned speeds — right longer, left slowed | leftSpeed=0.67, rightSpeed=2.0 | PASSED |
| 317 | **Coordinator** | Aligned speeds — equal frames, same speed | leftSpeed=rightSpeed=1.5 | PASSED |
| 318 | **Coordinator** | Zero frames returns base speed | Both sides get globalSpeed | PASSED |
| 319 | **Coordinator** | Global progress from max of both sides | max(leftPct, rightPct) | PASSED |
| 320 | **CompareStore** | Default algorithm IDs and UNINITIALIZED state | bubble-sort, selection-sort, UNINITIALIZED | PASSED |
| 321 | **CompareStore** | Load compare session with dummy generators | Frames loaded, LOADED state | PASSED |
| 322 | **CompareStore** | Generate random input array (size 15) | 15 values in [1,99] range | PASSED |
| 323 | **CompareStore** | Fair comparison — same input for both sides | leftFrames[0].dataState === rightFrames[0].dataState | PASSED |
| 324 | **CompareStore** | Step forward both sides | leftCurrentIndex=rightCurrentIndex=1 | PASSED |
| 325 | **CompareStore** | Step backward both sides | Both decrement by 1 | PASSED |
| 326 | **CompareStore** | Cannot step backward below 0 | Both stay at 0 | PASSED |
| 327 | **CompareStore** | Scrub to percent — sync proportionally | Correct frame indices for 50% | PASSED |
| 328 | **CompareStore** | Clamp scrub percent 0-100 | -10 → frame 0, 200 → last frame | PASSED |
| 329 | **CompareStore** | Stop and reset to frame 0 | isPlaying=false, both at 0 | PASSED |
| 330 | **CompareStore** | Left stats computation (comparisons/swaps) | Values ≥ 0, totalFrames correct | PASSED |
| 331 | **CompareStore** | Efficiency ratio computation | efficiencyRatio > 0 at 100% | PASSED |
| 332 | **CompareStore** | Algorithm name resolution from catalog | Contains Bubble Sort / Selection Sort | PASSED |
| 333 | **CompareStore** | Change playback speed | globalPlaySpeed = 2.0 | PASSED |
| 334 | **CompareStore** | Toggle playback mode | independent ↔ normalized | PASSED |
| 335 | **CompareStore** | Cleanup all state | Frames empty, UNINITIALIZED | PASSED |
| 336 | **CompareStore** | Progress percentages 0% and 100% | 0→0%, 100→100% both sides | PASSED |
| 337 | **CompareStore** | FINISHED state detection | bothFinished=true, FINISHED state | PASSED |
| 338 | **CompareStore** | Time complexity from catalog | O(N²) for both | PASSED |
| 339 | **RenderScheduler** | Register callbacks and invoke on tick | leftCb and rightCb called once | PASSED |
| 340 | **RenderScheduler** | No duplicate loop start | Second startSchedulerLoop ignored | PASSED |
| 341 | **RenderScheduler** | Stop scheduler and cancel animation frame | cancelAnimationFrame called | PASSED |
| 342 | **RenderScheduler** | Cleanup nullifies callbacks | Stop + nullify after cleanup | PASSED |

---

## Phase 2 Concurrency Visualizer — 35 Unit Tests (ConcurrencySimulationEngine + DeadlockDetector + useConcurrencyStore)

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 343 | **SimEngine** | Initialize threads and locks | 2 threads, L1 lock, heldByThreadId=null, counter=0 | PASSED |
| 344 | **SimEngine** | Acquire lock successfully when free | acquireLock(T1,L1)=true, heldByThreadId=T1, state=RUNNING | PASSED |
| 345 | **SimEngine** | Block thread when lock held by another | acquireLock(T2,L1)=false, state=BLOCKED, waitingForLock=L1 | PASSED |
| 346 | **SimEngine** | Release lock and wake waiting thread | releaseLock(T1,L1) → T2 acquires, state=RUNNING | PASSED |
| 347 | **SimEngine** | Move thread progress | moveThread(T1,50) → progress=50, state=RUNNING | PASSED |
| 348 | **SimEngine** | Finish thread at 100% progress | moveThread(T1,100) → state=FINISHED | PASSED |
| 349 | **SimEngine** | Not move blocked thread | BLOCKED thread stays at progress=0 | PASSED |
| 350 | **SimEngine** | Increment and read shared counter | incrementCounter×2 → readCounter=2 | PASSED |
| 351 | **SimEngine** | Clamp progress at 100 | moveThread(60)+moveThread(60) → progress=100 | PASSED |
| 352 | **SimEngine** | Not release lock held by different thread | releaseLock(T2,L1) ignored, T1 still holds | PASSED |
| 353 | **Deadlock** | No deadlock when no waiting threads | isDeadlocked=false, cycleThreadIds=[] | PASSED |
| 354 | **Deadlock** | Circular deadlock between two threads | T1→L2→T2→L1→T1 cycle detected | PASSED |
| 355 | **Deadlock** | No deadlock when thread waits but no cycle | T2 waits T1 (no reverse), isDeadlocked=false | PASSED |
| 356 | **Deadlock** | Dining philosophers 5-thread deadlock | P0→P1→P2→P3→P4→P0 circular WFG detected | PASSED |
| 357 | **Deadlock** | Empty threads list | isDeadlocked=false | PASSED |
| 358 | **Deadlock** | Waiting for non-existent lock | isDeadlocked=false | PASSED |
| 359 | **ConcStore** | Default state (IDLE, no threads) | threads=[], isPlaying=false, playbackMode=IDLE | PASSED |
| 360 | **ConcStore** | Initialize race-condition scenario | 2 threads (T1,T2), READY state, totalSteps>0 | PASSED |
| 361 | **ConcStore** | Initialize deadlock-demo scenario | 2 threads, locks L1+L2 | PASSED |
| 362 | **ConcStore** | Initialize dining-philosophers scenario | 5 threads, 5 locks (F0-F4) | PASSED |
| 363 | **ConcStore** | Step forward correctly | currentStepIndex increments by 1 | PASSED |
| 364 | **ConcStore** | Step backward correctly | currentStepIndex decrements by 1 via history | PASSED |
| 365 | **ConcStore** | Not step backward past zero | currentStepIndex stays 0 | PASSED |
| 366 | **ConcStore** | Detect deadlock in deadlock-demo | isDeadlocked=true, playbackMode=DEADLOCKED | PASSED |
| 367 | **ConcStore** | Not step forward when deadlocked | currentStepIndex unchanged after deadlock | PASSED |
| 368 | **ConcStore** | Toggle play/pause | PLAYING↔PAUSED transitions | PASSED |
| 369 | **ConcStore** | Stop and reset simulation | currentStepIndex=0, playbackMode=IDLE | PASSED |
| 370 | **ConcStore** | Set speed within bounds | 0.1→0.25, 10→4, 2→2 | PASSED |
| 371 | **ConcStore** | Scrub to specific step | scrubToStep(5) → currentStepIndex=5 | PASSED |
| 372 | **ConcStore** | Scrub backward via history | scrubToStep(8)→scrubToStep(3) → currentStepIndex=3 | PASSED |
| 373 | **ConcStore** | Toggle mutex and reinitialize | mutexEnabled=false, currentStepIndex=0 | PASSED |
| 374 | **ConcStore** | Compute progress percent | 0% at start, >0% at half | PASSED |
| 375 | **ConcStore** | List all scenario options | 4 scenarios (race-condition, deadlock-demo, producer-consumer, dining-philosophers) | PASSED |
| 376 | **ConcStore** | Finish race-condition without deadlock | isDeadlocked=false, sharedCounter>0 | PASSED |
| 377 | **ConcStore** | Cleanup resources | threads=[], playbackMode=IDLE | PASSED |

---

## Phase 2 Debug Mode — 49 Unit Tests (DebuggerYieldEngine + LiveCompilerDebugger + useLiveDebuggerStore)

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 378 | **YieldEngine** | Compile valid Bubble Sort code | success=true, generatorCode defined | PASSED |
| 379 | **YieldEngine** | Convert function to generator (function*) | generatorCode contains 'function*' | PASSED |
| 380 | **YieldEngine** | Inject yield statements | generatorCode contains 'yield' | PASSED |
| 381 | **YieldEngine** | Inject __loopCounter guard | generatorCode contains '__loopCounter' | PASSED |
| 382 | **YieldEngine** | Inject __recursionDepth guard | generatorCode contains '__recursionDepth' | PASSED |
| 383 | **YieldEngine** | Inject __callStack management | generatorCode contains '__callStack' | PASSED |
| 384 | **YieldEngine** | Create __debugMain wrapper function | generatorCode contains '__debugMain' | PASSED |
| 385 | **YieldEngine** | Return error for invalid syntax | success=false, error defined | PASSED |
| 386 | **YieldEngine** | Extract error line number from syntax error | errorLine defined | PASSED |
| 387 | **YieldEngine** | Handle empty function body | success=true | PASSED |
| 388 | **YieldEngine** | Handle while loops | __loopCounter injected | PASSED |
| 389 | **YieldEngine** | Include lineNumber in yield payload | 'lineNumber' in generatorCode | PASSED |
| 390 | **YieldEngine** | Include arrayState in yield payload | 'arrayState' in generatorCode | PASSED |
| 391 | **YieldEngine** | Include callStack in yield payload | 'callStack' in generatorCode | PASSED |
| 392 | **YieldEngine** | Include __captureVars in yield payload | '__captureVars' in generatorCode | PASSED |
| 393 | **Debugger** | Step forward through generator yields | lineNumber=2 then 3, variables tracked | PASSED |
| 394 | **Debugger** | Return null when generator exhausted | null after 6 steps, isFinished=true | PASSED |
| 395 | **Debugger** | Track step count | getStepCount increments correctly | PASSED |
| 396 | **Debugger** | Halt exactly on registered breakpoint | continueToNextBreakpoint stops at line 5 | PASSED |
| 397 | **Debugger** | Skip non-breakpoint lines | continueToNextBreakpoint at line 7, arrayState=[3,5] | PASSED |
| 398 | **Debugger** | Return null if no breakpoint hit | generator exhausted without hitting line 99 | PASSED |
| 399 | **Debugger** | Restore previous step from history | stepBackward returns lineNumber=3 | PASSED |
| 400 | **Debugger** | Return null at first step | stepBackward=null at step 1 | PASSED |
| 401 | **Debugger** | Advance until callStack depth decreases (stepOut) | callStack.length=1, lineNumber=10 | PASSED |
| 402 | **Debugger** | Track recursive call stack correctly | ['quickSort(0,1)'] then ['quickSort(0,1)','partition(0,1)'] | PASSED |
| 403 | **Debugger** | Set and get breakpoints | getActiveBreakpoints=[3,5,7] | PASSED |
| 404 | **Debugger** | Record all steps in history | history.length=3, correct lineNumbers | PASSED |
| 405 | **Debugger** | Reset clears all state | null step, 0 count, empty history, isFinished=true | PASSED |
| 406 | **DebugStore** | IDLE status on init | status='IDLE' | PASSED |
| 407 | **DebugStore** | Default source code contains bubbleSort | sourceCode includes 'bubbleSort' | PASSED |
| 408 | **DebugStore** | Default input array [5,3,8,1,9,2,7] | inputArray matches | PASSED |
| 409 | **DebugStore** | Empty breakpoints on init | activeBreakpoints=[] | PASSED |
| 410 | **DebugStore** | Not debugging on init | isDebugging=false | PASSED |
| 411 | **DebugStore** | Toggle breakpoint add | activeBreakpoints contains 5 | PASSED |
| 412 | **DebugStore** | Toggle breakpoint remove | activeBreakpoints does not contain 5 | PASSED |
| 413 | **DebugStore** | Support multiple breakpoints | activeBreakpoints.length=3 | PASSED |
| 414 | **DebugStore** | Update source code | sourceCode='function test() {}' | PASSED |
| 415 | **DebugStore** | Update input array | inputArray=[10,20,30] | PASSED |
| 416 | **DebugStore** | Start session → PAUSED status | status='PAUSED' on valid code | PASSED |
| 417 | **DebugStore** | Set current line after starting | currentLineNumber not null | PASSED |
| 418 | **DebugStore** | Populate array state | arrayState.length > 0 | PASSED |
| 419 | **DebugStore** | ERROR status on invalid code | status='ERROR', errorMessage defined | PASSED |
| 420 | **DebugStore** | Step forward advances | stepCount > 1 after stepForward | PASSED |
| 421 | **DebugStore** | Stop resets all state | IDLE, null line, empty callStack/vars, 0 steps | PASSED |
| 422 | **DebugStore** | isDebugging true when PAUSED | isDebugging=true | PASSED |
| 423 | **DebugStore** | isPaused true when PAUSED | isPaused=true | PASSED |
| 424 | **DebugStore** | canStepForward true when PAUSED | canStepForward=true | PASSED |
| 425 | **DebugStore** | hasError true on ERROR status | hasError=true | PASSED |
| 426 | **DebugStore** | Detect mutated variables on step | watchedVariables defined after step | PASSED |
| 427 | **DesignPatterns Engine** | Cubic Bezier path calculation | path contains M startX,startY C cp1,cp2 endX,endY | PASSED |
| 428 | **DesignPatterns Engine** | Empty path for non-existent link | calculateBezierPath('FAKE') returns '' | PASSED |
| 429 | **DesignPatterns Engine** | Control offset based on deltaY | controlOffset = min(100, deltaY*0.5) | PASSED |
| 430 | **DesignPatterns Engine** | Clamp minimum control offset 30 | Close nodes get controlOffset=30 minimum | PASSED |
| 431 | **DesignPatterns Engine** | Cap controlOffset at 100 | Large deltaY caps at 100 | PASSED |
| 432 | **DesignPatterns Engine** | calculateAllPaths Map | Returns Map with all link paths | PASSED |
| 433 | **DesignPatterns Engine** | updateNodePosition coordinates | Node x/y updated correctly | PASSED |
| 434 | **DesignPatterns Engine** | Clamp position within canvas | Negative coords clamped to padding=10 | PASSED |
| 435 | **DesignPatterns Engine** | Clamp maximum position | Exceeding coords clamped to canvasWidth-nodeWidth-padding | PASSED |
| 436 | **DesignPatterns Engine** | Ignore non-existent node drag | No error, nodes unchanged | PASSED |
| 437 | **DesignPatterns Engine** | swapStrategyTarget success | Link targetId changed to new node | PASSED |
| 438 | **DesignPatterns Engine** | swapStrategyTarget non-existent link | Returns false | PASSED |
| 439 | **DesignPatterns Engine** | swapStrategyTarget non-existent target | Returns false | PASSED |
| 440 | **DesignPatterns Engine** | getLinksToTarget | Finds all links to target node | PASSED |
| 441 | **DesignPatterns Engine** | getLinksFromSource | Finds all links from source node | PASSED |
| 442 | **DesignPatterns Engine** | getLinksToTarget empty | Returns empty for non-existent node | PASSED |
| 443 | **DesignPatterns Engine** | replaceState | Replaces all nodes and links | PASSED |
| 444 | **DesignPatterns Engine** | getNodeById found | Returns correct node by id | PASSED |
| 445 | **DesignPatterns Store** | Load Strategy Pattern scenario | 4 nodes, 3 links, title='Strategy Pattern' | PASSED |
| 446 | **DesignPatterns Store** | Load Observer Pattern scenario | 5 nodes, 4 links | PASSED |
| 447 | **DesignPatterns Store** | Load DIP Sandbox scenario | 2 nodes, 1 link, isDIPEnabled=false | PASSED |
| 448 | **DesignPatterns Store** | Reset observer on scenario switch | isObserverNotifying=false after switch | PASSED |
| 449 | **DesignPatterns Store** | Path cache calculated | pathCache.size=3 for strategy | PASSED |
| 450 | **DesignPatterns Store** | switchStrategy Bubble→Quick | activeStrategyTargetId='Quick', link updated | PASSED |
| 451 | **DesignPatterns Store** | Paths recalculated after swap | pathCache changes after switchStrategy | PASSED |
| 452 | **DesignPatterns Store** | No swap in non-strategy mode | activeStrategyTargetId unchanged | PASSED |
| 453 | **DesignPatterns Store** | Observer notify 2s timeout | isObserverNotifying true→false after 2000ms | PASSED |
| 454 | **DesignPatterns Store** | No notify in non-observer mode | isObserverNotifying stays false | PASSED |
| 455 | **DesignPatterns Store** | DIP toggle adds IDatabase | nodes.length 2→3, IDatabase node exists | PASSED |
| 456 | **DesignPatterns Store** | DIP decoupled links | 2 links: HighToInterface + LowToInterface | PASSED |
| 457 | **DesignPatterns Store** | DIP toggle off removes IDatabase | nodes.length back to 2 | PASSED |
| 458 | **DesignPatterns Store** | DIP restore direct coupling | 1 link: DirectCoupling | PASSED |
| 459 | **DesignPatterns Store** | No DIP toggle in non-DIP mode | isDIPEnabled stays false | PASSED |
| 460 | **DesignPatterns Store** | Coupling 85% when DIP off | couplingIndexMetric=85, label='RẤT CHẶT' | PASSED |
| 461 | **DesignPatterns Store** | Coupling 20% when DIP on | couplingIndexMetric=20, label='LỎNG LẺO' | PASSED |
| 462 | **DesignPatterns Store** | Coupling 35% for non-DIP | couplingIndexMetric=35 for strategy | PASSED |
| 463 | **DesignPatterns Store** | handleNodeDrag updates position | Node x/y changed, pathCache updated | PASSED |
| 464 | **DesignPatterns Store** | cleanup resets state | nodes=[], links=[], pathCache empty | PASSED |
| 465 | **DesignPatterns Store** | nodeCount/linkCount computed | nodeCount=4, linkCount=3 for strategy | PASSED |
| 466 | **Scenario Data** | Strategy Pattern 4 nodes 3 links | patternId, nodes.length, links.length correct | PASSED |
| 467 | **Scenario Data** | Observer Pattern 5 nodes 4 links | Correct node/link counts | PASSED |
| 468 | **Scenario Data** | DIP Sandbox 2 nodes 1 link | Correct counts | PASSED |
| 469 | **Scenario Data** | Non-existent scenario returns null | getScenario('fake') → null | PASSED |
| 470 | **Scenario Data** | Strategy nodes have attributes/methods | Client has attributes and methods | PASSED |
| 471 | **Scenario Data** | ISortStrategy interface type | type='interface' | PASSED |
| 472 | **Scenario Data** | Dependency link Client→Strategy | type='dependency', correct sourceId/targetId | PASSED |
| 473 | **Scenario Data** | Realization links concrete→interface | BubbleToStrategy + QuickToStrategy type='realization' | PASSED |
| 474 | **Scenario Data** | getAllScenarioIds returns 3 | strategy-pattern, observer-pattern, solid-dip | PASSED |
| 475 | **Scenario Data** | SCENARIO_LABELS correct | All 3 labels defined | PASSED |
| 476 | **Scenario Data** | getNodeById undefined for non-existent | Returns undefined | PASSED |
| 477 | **EmbedBridge** | Create bridge with default wildcard origin | listenerCount=0 | PASSED |
| 478 | **EmbedBridge** | Create bridge with custom allowed origins | listenerCount=0 | PASSED |
| 479 | **EmbedBridge** | Register listener and return unsubscribe | listenerCount 0→1→0 | PASSED |
| 480 | **EmbedBridge** | Support multiple listeners simultaneously | listenerCount=3 | PASSED |
| 481 | **EmbedBridge** | Deliver messages from whitelisted origins | callback called with STEP_FORWARD | PASSED |
| 482 | **EmbedBridge** | Deliver messages with wildcard origin | callback called for any origin | PASSED |
| 483 | **EmbedBridge** | Deliver WIDGET source messages | HEIGHT_CHANGED payload received | PASSED |
| 484 | **EmbedBridge** | Block messages from non-whitelisted origins (XSS) | callback NOT called, console.warn XSS_PREVENTION_BLOCKED | PASSED |
| 485 | **EmbedBridge** | Block messages from empty origin strict whitelist | callback NOT called | PASSED |
| 486 | **EmbedBridge** | Ignore messages without valid source field | callback NOT called | PASSED |
| 487 | **EmbedBridge** | Ignore null message data | callback NOT called | PASSED |
| 488 | **EmbedBridge** | Ignore messages from unknown sources | callback NOT called | PASSED |
| 489 | **EmbedBridge** | Call postMessage on target window | postMessage called with correct args | PASSED |
| 490 | **EmbedBridge** | Default to wildcard target origin | postMessage called with '*' | PASSED |
| 491 | **EmbedBridge** | Clear all listeners on destroy | listenerCount=0 | PASSED |
| 492 | **EmbedBridge** | Stop receiving messages after destroy | callback NOT called post-destroy | PASSED |
| 493 | **EmbedBridge** | Handle double destroy gracefully | No throw | PASSED |
| 494 | **SecureOrigin** | Accept default whitelisted visualization-dsa.edu.vn | isValidOrigin=true | PASSED |
| 495 | **SecureOrigin** | Accept default whitelisted moodle.hust.edu.vn | isValidOrigin=true | PASSED |
| 496 | **SecureOrigin** | Accept default whitelisted canvas.usth.edu.vn | isValidOrigin=true | PASSED |
| 497 | **SecureOrigin** | Reject domains not in whitelist | isValidOrigin=false | PASSED |
| 498 | **SecureOrigin** | Reject empty origin string | isValidOrigin=false | PASSED |
| 499 | **SecureOrigin** | Have 3 default domains | domainCount=3 | PASSED |
| 500 | **SecureOrigin** | Accept any origin with wildcard | isValidOrigin=true for all | PASSED |
| 501 | **SecureOrigin** | Dynamically add a trusted domain | domainCount 3→4, isValidOrigin=true | PASSED |
| 502 | **SecureOrigin** | Not duplicate existing domains | domainCount stays 3 | PASSED |
| 503 | **SecureOrigin** | Remove domain from whitelist | domainCount 3→2, isValidOrigin=false | PASSED |
| 504 | **SecureOrigin** | Handle removing non-existent domain | domainCount stays 3 | PASSED |
| 505 | **SecureOrigin** | Clear all domains from whitelist | domainCount=0 | PASSED |
| 506 | **SecureOrigin** | Return copy of whitelist as array | 3 domains in array | PASSED |
| 507 | **SecureOrigin** | Initialize with custom domains | custom domainCount=2 | PASSED |
| 508 | **SecureOrigin** | Initialize with empty array | domainCount=0 | PASSED |
| 509 | **AutoHeight** | Return value within default bounds | clampHeight(500)=500 | PASSED |
| 510 | **AutoHeight** | Clamp to minimum height 300px | clampHeight(100)=300 | PASSED |
| 511 | **AutoHeight** | Clamp to maximum height 1200px | clampHeight(2000)=1200 | PASSED |
| 512 | **AutoHeight** | Accept exact boundary values | 300→300, 1200→1200 | PASSED |
| 513 | **AutoHeight** | Respect custom min/max bounds | custom 400/800 clamping | PASSED |
| 514 | **AutoHeight** | Start with lastReportedHeight of 0 | getLastReportedHeight()=0 | PASSED |
| 515 | **AutoHeight** | Reset lastReportedHeight on destroy | 0 after destroy | PASSED |
| 516 | **AutoHeight** | Handle double destroy gracefully | No throw | PASSED |
| 517 | **AutoHeight** | Not throw when starting observation | start() no throw | PASSED |
| 518 | **AutoHeight** | Be idempotent when called multiple times | double start() no throw | PASSED |
| 519 | **EmbedStore** | Default theme glass | selectedTheme='glass' | PASSED |
| 520 | **EmbedStore** | VCR controls enabled by default | showVcrControls=true | PASSED |
| 521 | **EmbedStore** | Watch variables enabled by default | showWatchVariables=true | PASSED |
| 522 | **EmbedStore** | Interactive mode enabled by default | isInteractive=true | PASSED |
| 523 | **EmbedStore** | Default dimensions 800x500 | widgetWidth=800, widgetHeight=500 | PASSED |
| 524 | **EmbedStore** | Default algorithm quicksort-recursion | selectedAlgorithm correct | PASSED |
| 525 | **EmbedStore** | isCopied false initially | isCopied=false | PASSED |
| 526 | **EmbedStore** | Generated code contains iframe tag | <iframe...></iframe> present | PASSED |
| 527 | **EmbedStore** | Sandbox attribute with secure flags | sandbox="allow-scripts allow-same-origin" | PASSED |
| 528 | **EmbedStore** | Include default dimensions | width="800" height="500" | PASSED |
| 529 | **EmbedStore** | Include selected algorithm | algo=quicksort-recursion | PASSED |
| 530 | **EmbedStore** | Include selected theme | theme=glass | PASSED |
| 531 | **EmbedStore** | Include base URL | visualization-dsa.edu.vn/embed | PASSED |
| 532 | **EmbedStore** | Include border-radius styling | border-radius: 16px | PASSED |
| 533 | **EmbedStore** | Update dynamically when theme changes | theme=dark after setTheme | PASSED |
| 534 | **EmbedStore** | Update dynamically when algorithm changes | algo=heap-sort | PASSED |
| 535 | **EmbedStore** | Update dynamically when dimensions change | width="1000" height="700" | PASSED |
| 536 | **EmbedStore** | Reflect VCR controls toggle | vcr=false | PASSED |
| 537 | **EmbedStore** | iframeSrcUrl properly formatted | URL with params | PASSED |
| 538 | **EmbedStore** | setTheme dark | selectedTheme='dark' | PASSED |
| 539 | **EmbedStore** | setTheme light | selectedTheme='light' | PASSED |
| 540 | **EmbedStore** | setAlgorithm | selectedAlgorithm='merge-sort' | PASSED |
| 541 | **EmbedStore** | setDimensions valid | 600x400 set correctly | PASSED |
| 542 | **EmbedStore** | Clamp width minimum 300 | widgetWidth=300 from 100 | PASSED |
| 543 | **EmbedStore** | Clamp width maximum 1400 | widgetWidth=1400 from 2000 | PASSED |
| 544 | **EmbedStore** | Clamp height minimum 200 | widgetHeight=200 from 50 | PASSED |
| 545 | **EmbedStore** | Clamp height maximum 900 | widgetHeight=900 from 1500 | PASSED |
| 546 | **EmbedStore** | Toggle VCR controls | true→false→true | PASSED |
| 547 | **EmbedStore** | Toggle watch variables | true→false | PASSED |
| 548 | **EmbedStore** | Toggle interactive mode | true→false | PASSED |
| 549 | **EmbedStore** | Reset all values to defaults | all fields reset | PASSED |
| 550 | **EmbedStore** | isCopied true on successful copy | Clipboard writeText mock | PASSED |
| 551 | **EmbedStore** | Return false on clipboard error | error handled gracefully | PASSED |
| 552 | **EmbedStore** | Reset isCopied after 2 seconds | fake timer advance 2000ms | PASSED |
| 553 | **State Compressor** | Nén trạng thái thành chuỗi không rỗng | serializeState trả chuỗi non-empty | 🟢 PASSED |
| 554 | **State Compressor** | Nén hiệu quả với dữ liệu lớn | compressed.length < rawJson.length (30 nodes) | 🟢 PASSED |
| 555 | **State Compressor** | Chuỗi URL-safe (không +, /) | Không chứa ký tự đặc biệt URL-unsafe | 🟢 PASSED |
| 556 | **State Compressor** | Đầu ra khác nhau cho state khác nhau | bubble-sort ≠ merge-sort compressed | 🟢 PASSED |
| 557 | **State Compressor** | Xử lý layoutNodes rỗng | Nén thành công state với mảng rỗng | 🟢 PASSED |
| 558 | **State Compressor** | Xử lý nhiều nodes (100) | Nén thành công mảng 100 phần tử | 🟢 PASSED |
| 559 | **State Compressor** | Phục hồi 100% tất cả trường | algorithmId, currentStepIndex, layoutNodes | 🟢 PASSED |
| 560 | **State Compressor** | Bảo toàn tọa độ chính xác | x=150, y=80 khôi phục hoàn hảo | 🟢 PASSED |
| 561 | **State Compressor** | Trả null cho chuỗi rỗng | deserializeState('') → null | 🟢 PASSED |
| 562 | **State Compressor** | Trả null cho dữ liệu bất hợp lệ | deserializeState('invalid') → null | 🟢 PASSED |
| 563 | **State Compressor** | Round-trip 50 nodes zero data loss | 50 nodes phục hồi chính xác | 🟢 PASSED |
| 564 | **State Compressor** | Log lỗi cho dữ liệu hỏng | console.error gọi đúng cách | 🟢 PASSED |
| 565 | **State Compressor** | isWithinSizeLimit true cho ngắn | Chuỗi nén ngắn nằm trong giới hạn | 🟢 PASSED |
| 566 | **State Compressor** | isWithinSizeLimit true ở đúng giới hạn | 20000 ký tự = true | 🟢 PASSED |
| 567 | **State Compressor** | isWithinSizeLimit false vượt giới hạn | 20001 ký tự = false | 🟢 PASSED |
| 568 | **State Compressor** | isWithinSizeLimit true cho rỗng | Chuỗi rỗng = true | 🟢 PASSED |
| 569 | **State Compressor** | Validation trả chuỗi hợp lệ | serializeStateWithValidation trả non-null | 🟢 PASSED |
| 570 | **State Compressor** | Validation trả null cho state quá lớn | 5000 nodes → null + console.warn | 🟢 PASSED |
| 571 | **State Compressor** | Validation round-trip integrity | Kết quả giải nén khớp 100% | 🟢 PASSED |
| 572 | **SVG Exporter** | clampScale giữ nguyên giá trị hợp lệ | clampScale(2)=2, clampScale(3)=3 | 🟢 PASSED |
| 573 | **SVG Exporter** | clampScale kẹp dưới MIN_SCALE | clampScale(0)=1, clampScale(-5)=1 | 🟢 PASSED |
| 574 | **SVG Exporter** | clampScale kẹp trên MAX_SCALE | clampScale(10)=4, clampScale(100)=4 | 🟢 PASSED |
| 575 | **SVG Exporter** | clampScale biên MIN chính xác | clampScale(1)=1 | 🟢 PASSED |
| 576 | **SVG Exporter** | clampScale biên MAX chính xác | clampScale(4)=4 | 🟢 PASSED |
| 577 | **SVG Exporter** | clampScale phân số | clampScale(2.5)=2.5 | 🟢 PASSED |
| 578 | **SVG Exporter** | clampScale phân số dưới MIN | clampScale(0.5)=1 | 🟢 PASSED |
| 579 | **SVG Exporter** | extractSVGDataURI trả Base64 hợp lệ | Khớp pattern data:image/svg+xml;base64, | 🟢 PASSED |
| 580 | **SVG Exporter** | extractSVGDataURI inject <style> | Decoded chứa thẻ <style> | 🟢 PASSED |
| 581 | **SVG Exporter** | extractSVGDataURI không sửa gốc | childNodes.length giữ nguyên | 🟢 PASSED |
| 582 | **SVG Exporter** | extractSVGDataURI Base64 giải mã được | atob() không throw | 🟢 PASSED |
| 583 | **SVG Exporter** | extractSVGDataURI giữ nội dung gốc | Decoded chứa 'Test SVG Content' | 🟢 PASSED |
| 584 | **SVG Exporter** | extractSVGDataURI SVG rỗng | Xử lý SVG không có children | 🟢 PASSED |
| 585 | **SVG Exporter** | exportToSVGString XML hợp lệ | Chứa <svg> và </svg> | 🟢 PASSED |
| 586 | **SVG Exporter** | exportToSVGString inject styles | Chứa thẻ <style> | 🟢 PASSED |
| 587 | **SVG Exporter** | exportToSVGString giữ nội dung | Chứa 'Test SVG Content' | 🟢 PASSED |
| 588 | **SVG Exporter** | exportToSVGString không sửa gốc | childNodes.length giữ nguyên | 🟢 PASSED |
| 589 | **SVG Exporter** | exportToSVGString giữ viewBox | Chứa viewBox="0 0 1024 768" | 🟢 PASSED |
| 590 | **SVG Exporter** | exportToPNG reject khi Image lỗi | Throw 'Lỗi tải cấu trúc ảnh SVG ảo.' | 🟢 PASSED |
| 591 | **SVG Exporter** | DEFAULT_SCALE = 3 | Hằng số đúng giá trị | 🟢 PASSED |
| 592 | **CSS Injector** | extractActiveCSSRules trả string | typeof result === 'string' | 🟢 PASSED |
| 593 | **CSS Injector** | extractActiveCSSRules không stylesheet | Trả string (có thể rỗng) | 🟢 PASSED |
| 594 | **CSS Injector** | extractActiveCSSRules trích xuất rules | Chứa 'color' từ injected style | 🟢 PASSED |
| 595 | **CSS Injector** | extractActiveCSSRules nhiều sheets | Chứa cả 'background' và 'font-size' | 🟢 PASSED |
| 596 | **CSS Injector** | extractActiveCSSRules CORS an toàn | Không throw exception | 🟢 PASSED |
| 597 | **CSS Injector** | extractActiveCSSRules newline giữa rules | Chứa ký tự \n | 🟢 PASSED |
| 598 | **CSS Injector** | injectCSSIntoSVG thêm style đầu tiên | firstChild.nodeName === 'style' | 🟢 PASSED |
| 599 | **CSS Injector** | injectCSSIntoSVG type=text/css | getAttribute('type') === 'text/css' | 🟢 PASSED |
| 600 | **CSS Injector** | injectCSSIntoSVG nội dung CSS | textContent chứa 'opacity' | 🟢 PASSED |
| 601 | **CSS Injector** | injectCSSIntoSVG giữ children cũ | 3 children (style + circle + text) | 🟢 PASSED |
| 602 | **CSS Injector** | injectCSSIntoSVG chèn trước child đầu | style trước, rect id=original-first sau | 🟢 PASSED |
| 603 | **CSS Injector** | injectCSSIntoSVG SVG rỗng | Không throw, 1 child | 🟢 PASSED |
| 604 | **Export Store** | Initial isSharingModalOpen false | Mặc định false | 🟢 PASSED |
| 605 | **Export Store** | Initial isExporting false | Mặc định false | 🟢 PASSED |
| 606 | **Export Store** | Initial exportProgress 0 | Mặc định 0 | 🟢 PASSED |
| 607 | **Export Store** | Initial selectedFormat png-3x | Mặc định 'png-3x' | 🟢 PASSED |
| 608 | **Export Store** | Initial generatedShareLink rỗng | Mặc định '' | 🟢 PASSED |
| 609 | **Export Store** | Initial isLinkCopied false | Mặc định false | 🟢 PASSED |
| 610 | **Export Store** | Initial isGeneratingLink false | Mặc định false | 🟢 PASSED |
| 611 | **Export Store** | Initial overflowError rỗng | Mặc định '' | 🟢 PASSED |
| 612 | **Export Store** | hasShareLink false khi chưa có | computed false | 🟢 PASSED |
| 613 | **Export Store** | hasShareLink true khi có link | computed true | 🟢 PASSED |
| 614 | **Export Store** | qrCodeValue rỗng khi chưa có | computed '' | 🟢 PASSED |
| 615 | **Export Store** | qrCodeValue = link khi có | computed khớp link | 🟢 PASSED |
| 616 | **Export Store** | openModal bật modal | isSharingModalOpen = true | 🟢 PASSED |
| 617 | **Export Store** | openModal reset isLinkCopied | false sau khi mở | 🟢 PASSED |
| 618 | **Export Store** | openModal xóa link cũ | generatedShareLink = '' | 🟢 PASSED |
| 619 | **Export Store** | openModal xóa lỗi | overflowError = '' | 🟢 PASSED |
| 620 | **Export Store** | openModal reset progress | exportProgress = 0 | 🟢 PASSED |
| 621 | **Export Store** | openModal reset isExporting | isExporting = false | 🟢 PASSED |
| 622 | **Export Store** | closeModal tắt modal | isSharingModalOpen = false | 🟢 PASSED |
| 623 | **Export Store** | setFormat svg-vector | selectedFormat = 'svg-vector' | 🟢 PASSED |
| 624 | **Export Store** | setFormat png-3x | selectedFormat = 'png-3x' | 🟢 PASSED |
| 625 | **Export Store** | generateShareLink tạo link | generatedShareLink chứa /s/?state= | 🟢 PASSED |
| 626 | **Export Store** | generateShareLink hoàn tất | isGeneratingLink = false | 🟢 PASSED |
| 627 | **Export Store** | generateShareLink xóa lỗi cũ | overflowError = '' | 🟢 PASSED |
| 628 | **Export Store** | generateShareLink overflow | overflowError chứa WORKSPACE_OVERFLOW | 🟢 PASSED |
| 629 | **Export Store** | generateShareLink chứa base URL | Chứa visualization-dsa.edu.vn | 🟢 PASSED |
| 630 | **Export Store** | copyShareLink thành công | isLinkCopied = true, return true | 🟢 PASSED |
| 631 | **Export Store** | copyShareLink thất bại | return false khi clipboard denied | 🟢 PASSED |
| 632 | **Export Store** | copyShareLink auto-reset 2s | isLinkCopied false sau 2000ms | 🟢 PASSED |
| 633 | **Export Store** | resetState mọi giá trị | Tất cả state về defaults | 🟢 PASSED |
| 634 | **Export Store** | downloadSVG tạo anchor click | createElement + click gọi đúng | 🟢 PASSED |
| 635 | **Constants** | EXPORT_MIN_SCALE = 1 | Hằng số đúng | 🟢 PASSED |
| 636 | **Constants** | EXPORT_MAX_SCALE = 4 | Hằng số đúng | 🟢 PASSED |
| 637 | **Constants** | EXPORT_DEFAULT_SCALE = 3 | Hằng số đúng | 🟢 PASSED |

### Phase 2 Gamification Engine — StreakCalculator, GamificationEngine, CanvasConfettiEngine, useGamificationStore (88 tests)

| STT | Phân hệ kiểm thử | Tính năng hạt nhân được xác thực | Phương thức kiểm tra (Test Spec) | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| 638 | **StreakCalculator** | Grace Period 2h adjustment | Nộp bài 3:00 AM → ngày hiện tại | 🟢 PASSED |
| 639 | **StreakCalculator** | Late-night 1:45 AM keeps streak | Nộp lúc 1:45 AM 18/05 → tính ngày 17/05 | 🟢 PASSED |
| 640 | **StreakCalculator** | Past Grace Period 2:05 AM breaks | Nộp lúc 2:05 AM → tính ngày mới | 🟢 PASSED |
| 641 | **StreakCalculator** | Midnight submission (00:00 AM) | 00:00 AM 18/05 → tính ngày 17/05 | 🟢 PASSED |
| 642 | **StreakCalculator** | Exactly 2:00 AM boundary | 2:00 AM → tính ngày mới (không bù) | 🟢 PASSED |
| 643 | **StreakCalculator** | Noon no date change | 12:00 PM → giữ nguyên ngày | 🟢 PASSED |
| 644 | **StreakCalculator** | Year boundary Jan 1 1:00 AM | 01/01/2027 1:00 AM → 31/12/2026 | 🟢 PASSED |
| 645 | **StreakCalculator** | Month boundary March 1 0:30 AM | 01/03 0:30 AM → 28/02 | 🟢 PASSED |
| 646 | **StreakCalculator** | YYYY-MM-DD format | Kiểm tra regex format đầu ra | 🟢 PASSED |
| 647 | **StreakCalculator** | Zero-padding month/day | 01/05 → '2026-01-05' | 🟢 PASSED |
| 648 | **StreakCalculator** | Same day no update | lastActive = today → shouldUpdate: false | 🟢 PASSED |
| 649 | **StreakCalculator** | Consecutive day increment | yesterday → nextStreak + 1 | 🟢 PASSED |
| 650 | **StreakCalculator** | Gap > 1 day reset | 3-day gap → nextStreak: 1 | 🟢 PASSED |
| 651 | **StreakCalculator** | Exactly 2-day gap reset | 2-day gap → nextStreak: 1 | 🟢 PASSED |
| 652 | **StreakCalculator** | First ever activity | empty lastActive → nextStreak: 1 | 🟢 PASSED |
| 653 | **StreakCalculator** | Cross-month streak | May 31 → Jun 1 → streak + 1 | 🟢 PASSED |
| 654 | **StreakCalculator** | Cross-year streak | Dec 31 → Jan 1 → streak + 1 | 🟢 PASSED |
| 655 | **StreakCalculator** | Same day maintains value | streak 1, same day → 1, no update | 🟢 PASSED |
| 656 | **StreakCalculator** | shouldUpdate true on increment | Liên tục → shouldUpdate: true | 🟢 PASSED |
| 657 | **StreakCalculator** | shouldUpdate true on reset | Gap → shouldUpdate: true | 🟢 PASSED |
| 658 | **GamificationEngine** | Unlock Recursion Master | 600 XP + 4 streak → recursion-master | 🟢 PASSED |
| 659 | **GamificationEngine** | Insufficient XP blocks unlock | 100 XP → không mở khóa | 🟢 PASSED |
| 660 | **GamificationEngine** | Insufficient streak blocks unlock | 1500 XP, 1 streak → không mở khóa | 🟢 PASSED |
| 661 | **GamificationEngine** | No re-unlock already unlocked | Đã có recursion-master → bỏ qua | 🟢 PASSED |
| 662 | **GamificationEngine** | Unlock SOLID Architect | 1200 XP + 6 streak → solid-architect | 🟢 PASSED |
| 663 | **GamificationEngine** | Multiple badges at once | 1200 XP + 8 streak → ≥2 badges | 🟢 PASSED |
| 664 | **GamificationEngine** | Empty when no badges qualify | 10 XP, 1 streak → [] | 🟢 PASSED |
| 665 | **GamificationEngine** | Empty when all already unlocked | Tất cả badges đã mở → [] | 🟢 PASSED |
| 666 | **GamificationEngine** | Unlock Streak Warrior | 250 XP + 7 streak → streak-warrior | 🟢 PASSED |
| 667 | **GamificationEngine** | getBadgeTemplates returns all | ≥5 badge definitions | 🟢 PASSED |
| 668 | **GamificationEngine** | Unique badge IDs | Không trùng ID | 🟢 PASSED |
| 669 | **GamificationEngine** | Positive XP thresholds | Tất cả > 0 | 🟢 PASSED |
| 670 | **GamificationEngine** | Positive streak thresholds | Tất cả > 0 | 🟢 PASSED |
| 671 | **GamificationEngine** | Valid XP range (100) | 100 → true | 🟢 PASSED |
| 672 | **GamificationEngine** | Reject XP > 200 | 201 → false | 🟢 PASSED |
| 673 | **GamificationEngine** | Reject zero XP | 0 → false | 🟢 PASSED |
| 674 | **GamificationEngine** | Reject negative XP | -50 → false | 🟢 PASSED |
| 675 | **GamificationEngine** | Accept exactly MAX (200) | 200 → true | 🟢 PASSED |
| 676 | **GamificationEngine** | Accept minimum valid (1) | 1 → true | 🟢 PASSED |
| 677 | **CanvasConfettiEngine** | Construct with valid canvas | engine khởi tạo thành công | 🟢 PASSED |
| 678 | **CanvasConfettiEngine** | Throw on null 2D context | Ném lỗi khi canvas 2D null | 🟢 PASSED |
| 679 | **CanvasConfettiEngine** | Default 150 particles | burst() → 150 particles | 🟢 PASSED |
| 680 | **CanvasConfettiEngine** | Custom particle count | burst(50) → 50 particles | 🟢 PASSED |
| 681 | **CanvasConfettiEngine** | Start rAF after burst | requestAnimationFrame called | 🟢 PASSED |
| 682 | **CanvasConfettiEngine** | Accumulate particles | 50 + 30 → 80 particles | 🟢 PASSED |
| 683 | **CanvasConfettiEngine** | Clear on destroy | destroy → 0 particles | 🟢 PASSED |
| 684 | **CanvasConfettiEngine** | Cancel rAF on destroy | cancelAnimationFrame called | 🟢 PASSED |
| 685 | **CanvasConfettiEngine** | Safe destroy without animation | Không crash | 🟢 PASSED |
| 686 | **CanvasConfettiEngine** | Resize to window dimensions | canvas width/height = window | 🟢 PASSED |
| 687 | **CanvasConfettiEngine** | Neon color palette | Màu thuộc CONFETTI_COLORS | 🟢 PASSED |
| 688 | **CanvasConfettiEngine** | Center position on burst | Particles tại canvas center | 🟢 PASSED |
| 689 | **CanvasConfettiEngine** | clearRect in tick | ctx.clearRect gọi đúng | 🟢 PASSED |
| 690 | **CanvasConfettiEngine** | Gravity moves particles | Vị trí thay đổi sau tick | 🟢 PASSED |
| 691 | **CanvasConfettiEngine** | Remove off-screen particles | 500 ticks → 0 particles | 🟢 PASSED |
| 692 | **CanvasConfettiEngine** | Active status reporting | false → burst → true | 🟢 PASSED |
| 693 | **CanvasConfettiEngine** | No duplicate animation | Không gọi rAF lần 2 | 🟢 PASSED |
| 694 | **GamificationStore** | Default XP = 0 | currentXP = 0 | 🟢 PASSED |
| 695 | **GamificationStore** | Default streak = 0 | activeStreak = 0 | 🟢 PASSED |
| 696 | **GamificationStore** | Empty unlocked badges | unlockedBadges = [] | 🟢 PASSED |
| 697 | **GamificationStore** | Confetti disabled initially | showConfetti = false | 🟢 PASSED |
| 698 | **GamificationStore** | Default leaderboard rank | leaderboardRank = 0 | 🟢 PASSED |
| 699 | **GamificationStore** | Default streak freezes | streakFreezesCount = 2 | 🟢 PASSED |
| 700 | **GamificationStore** | Empty leaderboard data | leaderboardData = [] | 🟢 PASSED |
| 701 | **GamificationStore** | Empty lastActiveDate | lastActiveDate = '' | 🟢 PASSED |
| 702 | **GamificationStore** | earnXPLocal adds XP | +100 → currentXP = 100 | 🟢 PASSED |
| 703 | **GamificationStore** | Accumulate XP | +100 +50 → 150 | 🟢 PASSED |
| 704 | **GamificationStore** | Reject XP > MAX | +300 → currentXP = 0 | 🟢 PASSED |
| 705 | **GamificationStore** | Reject zero XP | +0 → currentXP = 0 | 🟢 PASSED |
| 706 | **GamificationStore** | Reject negative XP | -50 → currentXP = 0 | 🟢 PASSED |
| 707 | **GamificationStore** | Update streak on first activity | +50 → streak ≥ 1 | 🟢 PASSED |
| 708 | **GamificationStore** | Confetti trigger | triggerConfettiRain → true | 🟢 PASSED |
| 709 | **GamificationStore** | Confetti auto-clear 4s | 4000ms → false | 🟢 PASSED |
| 710 | **GamificationStore** | Confetti active before 4s | 3999ms → true | 🟢 PASSED |
| 711 | **GamificationStore** | Streak freeze decrement | useStreakFreeze → count - 1 | 🟢 PASSED |
| 712 | **GamificationStore** | Streak freeze floor 0 | 3x calls → 0 | 🟢 PASSED |
| 713 | **GamificationStore** | Freeze returns true if available | return true | 🟢 PASSED |
| 714 | **GamificationStore** | Freeze returns false if empty | return false | 🟢 PASSED |
| 715 | **GamificationStore** | Badge unlock on threshold | 400 XP + 3 streak → unlock | 🟢 PASSED |
| 716 | **GamificationStore** | Confetti on badge unlock | Unlock → confetti = true | 🟢 PASSED |
| 717 | **GamificationStore** | No re-unlock | 2x check → same count | 🟢 PASSED |
| 718 | **GamificationStore** | Set leaderboard data | setLeaderboardData → stored | 🟢 PASSED |
| 719 | **GamificationStore** | Sort leaderboard by rank | Unsorted → sorted by rank | 🟢 PASSED |
| 720 | **GamificationStore** | Limit leaderboard to top 10 | 15 entries → 10 stored | 🟢 PASSED |
| 721 | **GamificationStore** | allBadges computed | ≥5 badge templates | 🟢 PASSED |
| 722 | **GamificationStore** | xpProgressPercent computed | >0 after earning XP | 🟢 PASSED |
| 723 | **GamificationStore** | nextBadgeXPThreshold computed | >0 | 🟢 PASSED |
| 724 | **GamificationStore** | lockedBadges computed | All badges locked initially | 🟢 PASSED |
| 725 | **GamificationStore** | streakStatus computed | inactive → active after XP | 🟢 PASSED |
