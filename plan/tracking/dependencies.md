# 📦 Quản Lý Phụ Thuộc Hệ Thống - Dependency Management Map

Tài liệu này đặc tả chi tiết danh mục thư viện công nghệ, các gói phụ thuộc (dependencies) và vai trò của chúng trong dự án **VisualizationDSA**.

---

## 1. Bản Đồ Công Nghệ Lõi (Core Technology Stack)

```
[Vue 3 Composition API] ===> (State Management via Pinia & TS) ===> [Canvas 2D / SVG Layer]
[Monaco Editor custom] ======> (Code editing shell read-only locks) ===> [Monaco Custom Theme]
[Vitest Testing Engine] ====> (Unit testing LCOM4, DI container DFS) ===> [Automated Test Specs]
[Custom HSL CSS Neon] ======> (Glassmorphism aesthetics variables) ======> [Premium Web UI]
```

---

## 2. Gói Phụ Thuộc Frontend (Frontend Dependencies)

| Tên Thư Viện | Phiên bản | Phân hệ sử dụng | Vai trò cốt lõi |
| :--- | :--- | :--- | :--- |
| **vue** | `^3.4.0` | Toàn hệ thống | Khung sườn Reactive UI Component, Composition API. |
| **pinia** | `^2.1.0` | Core Stores | Quản lý trạng thái chia sẻ (VCR Playback state, User Progress). |
| **typescript** | `^5.3.0` | Toàn hệ thống | Định kiểu tĩnh an toàn chặt chẽ cho toàn bộ động cơ hoạt ảnh. |
| **monaco-editor** | `^0.45.0` | Monaco Editor Shell | Trình soạn thảo viết mã giải thuật và mã giả, click gutter lề. |
| **canvas-confetti** | `^1.6.0` | Gamification | Phun pháo hoa chúc mừng thăng cấp Neon rực rỡ. |

---

## 3. Gói Phụ Thuộc Phát Triển & Kiểm Thử (DevDependencies)

| Tên Thư Viện | Phiên bản | Phân hệ sử dụng | Vai trò cốt lõi |
| :--- | :--- | :--- | :--- |
| **vite** | `^5.0.0` | Build Tooling | Máy chủ phát triển HMR siêu nhanh dưới máy khách. |
| **vitest** | `^1.0.0` | Unit Testing | Bộ chạy kiểm thử đơn vị tự động siêu nhạy bén. |
| **sass** | `^1.69.0` | Styling | Bộ biên dịch CSS Glassmorphism Neon hổ phách cao cấp. |

---

## 3.5 Gói Phụ Thuộc Backend (Backend Dependencies — Phase B1)

| Tên Thư Viện | Phiên bản | Phân hệ sử dụng | Vai trò cốt lõi |
| :--- | :--- | :--- | :--- |
| **BCrypt.Net-Next** | `4.0.3` | Infrastructure/AuthService | Hash password an toàn với adaptive work factor (12 rounds). |
| **FluentValidation** | `11.9.0` | Application/Validators | Validate DTOs: email format, password strength, XP bounds. |
| **FluentValidation.AspNetCore** | `11.3.0` | WebApi | Auto-validation pipeline tích hợp ASP.NET Core MVC. |
| **Serilog.AspNetCore** | `8.0.0` | WebApi | Structured logging cho Console + File sink. |
| **Serilog.Sinks.File** | `5.0.0` | WebApi | Rolling daily log files, 14-day retention. |
| **Microsoft.AspNetCore.Authentication.JwtBearer** | `8.0.0` | Infrastructure | JWT Bearer token auth + IdentityModel.Tokens. |
| **Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore** | `8.0.0` | WebApi | AddDbContextCheck for health endpoint. |

## 3.6 Gói Phụ Thuộc Testing Backend (Backend Test Dependencies — Phase B2)

| Tên Thư Viện | Phiên bản | Phân hệ sử dụng | Vai trò cốt lõi |
| :--- | :--- | :--- | :--- |
| **xunit** | `2.6.6` | All test projects | Test framework: [Fact], [Theory], [InlineData]. |
| **xunit.runner.visualstudio** | `2.5.6` | All test projects | Test runner cho Visual Studio & dotnet test CLI. |
| **Microsoft.NET.Test.Sdk** | `17.8.0` | All test projects | MSTest platform host cho xUnit runner. |
| **FluentAssertions** | `6.12.0` | All test projects | Readable assertions: .Should().Be(), .Should().ThrowAsync(). |
| **Moq** | `4.20.70` | Infrastructure.Tests | Mock IUnitOfWork/IRepository to isolate services from DB. |
| **Microsoft.Extensions.Configuration** | `8.0.0` | Infrastructure.Tests | In-memory IConfiguration for AuthService JWT config. |

---

## 4. Ràng Buộc Kiến Trúc Phụ Thuộc (Dependency Constraints)
*   **Không dùng thư viện ngoài cho hoạt ảnh DSA:** Toàn bộ thuật toán sắp xếp mảng Lerp Parabol, quay AVL nút cây, hạt HTTP request bay dọc cạnh và khói sập nguồn Canvas đều phải viết bằng mã nguồn TypeScript thuần túy kết hợp Canvas 2D/SVG để tối ưu hiệu năng 60 FPS bám sát rAF.
*   **Monaco Sandbox Security:** Phải cô lập Monaco Editor trong chế độ chặn nhấp chuột select văn bản (read-only pointer blockers) khi chạy VCR playback để bảo toàn dòng code.
