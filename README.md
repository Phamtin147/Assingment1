# 🏨 FU Mini Hotel Management System

Hệ thống quản lý khách sạn hiện đại với giao diện WPF đẹp mắt, sử dụng gradient màu tím và thiết kế UI/UX chuyên nghiệp.

## ✨ Tính năng

### 👤 Dành cho Khách hàng
- ✅ Đăng nhập an toàn
- ✅ Quản lý thông tin cá nhân
- ✅ Đặt phòng trực tuyến
- ✅ Xem lịch sử đặt phòng
- ✅ Tính toán tự động giá phòng

### 👨‍💼 Dành cho Admin
- ✅ Dashboard với thống kê tổng quan
- ✅ Quản lý khách hàng (CRUD)
- ✅ Quản lý phòng (CRUD)
- ✅ Quản lý đặt phòng (CRUD)
- ✅ Báo cáo theo khoảng thời gian
- ✅ Tìm kiếm và lọc dữ liệu

## 🎨 Giao diện

### Màu sắc chủ đạo
- **Primary**: Gradient tím (#1a0d2e → #2d1b69 → #4a2c7c)
- **Accent**: Purple (#6c3fb5 → #8b5fd3)
- **Success**: Green (#4CAF50)
- **Error**: Red (#F44336)

### Đặc điểm UI
- ✨ Gradient backgrounds đẹp mắt
- 🎯 Sidebar navigation hiện đại
- 📊 Cards với bo góc mềm mại
- 🔘 Buttons với hover effects
- 📝 Form inputs với border highlights
- 📱 Responsive layout

## 🚀 Cài đặt và Chạy

### Yêu cầu
- .NET 9.0 SDK
- Windows OS
- Visual Studio 2022 hoặc Rider

### Các bước chạy

1. **Clone repository**
```bash
git clone <repository-url>
cd Assingment1
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Build project**
```bash
dotnet build
```

4. **Run application**
```bash
dotnet run --project FUMiniHotelSystemWPF
```

Hoặc mở solution trong Visual Studio và nhấn F5.

## 🔐 Tài khoản mặc định

### Admin
- **Email**: `admin@FUMiniHotelSystem.com`
- **Password**: `@@abc123@@`

### Khách hàng mẫu
- **Email**: `nguyenvana@email.com`
- **Password**: `password123`

## 📁 Cấu trúc Project

```
FUMiniHotelSystem/
├── FUMiniHotelSystem.BusinessObjects/    # Models & DTOs
│   ├── Models/
│   │   ├── Customer.cs
│   │   ├── RoomInformation.cs
│   │   ├── RoomType.cs
│   │   └── Booking.cs
│   └── DTOs/
│       ├── LoginRequest.cs
│       └── LoginResponse.cs
│
├── FUMiniHotelSystem.DataAccess/         # Data Layer
│   ├── Database/
│   │   └── InMemoryDatabase.cs
│   └── Repositories/
│       ├── CustomerRepository.cs
│       ├── RoomRepository.cs
│       ├── RoomTypeRepository.cs
│       └── BookingRepository.cs
│
├── FUMiniHotelSystem.BusinessLogic/      # Business Logic
│   └── Services/
│       ├── AuthenticationService.cs
│       ├── CustomerService.cs
│       ├── RoomService.cs
│       └── BookingService.cs
│
└── FUMiniHotelSystemWPF/                 # WPF UI Layer
    ├── Styles/                           # Resource Dictionaries
    │   ├── Colors.xaml
    │   ├── ButtonStyles.xaml
    │   ├── TextBoxStyles.xaml
    │   └── DataGridStyles.xaml
    ├── Views/                            # Windows & Dialogs
    │   ├── LoginWindow.xaml
    │   ├── AdminDashboardWindow.xaml
    │   ├── CustomerDashboardWindow.xaml
    │   ├── CustomerDialog.xaml
    │   ├── RoomDialog.xaml
    │   ├── BookingDialog.xaml
    │   └── AdminBookingDialog.xaml
    └── Converters/
        └── BookingStatusConverter.cs
```

## 🛠️ Công nghệ sử dụng

- **Framework**: .NET 9.0
- **UI**: WPF (Windows Presentation Foundation)
- **Architecture**: 3-Layer Architecture
  - Presentation Layer (WPF)
  - Business Logic Layer
  - Data Access Layer
- **Data Storage**: In-Memory Database
- **Query**: LINQ
- **Design Pattern**: Repository Pattern, Singleton Pattern

## 📊 Database Schema

### Customer
- CustomerID (int, PK)
- CustomerFullName (string)
- Telephone (string)
- EmailAddress (string)
- CustomerBirthday (DateTime)
- CustomerStatus (int) - 1: Active, 2: Deleted
- Password (string)

### RoomType
- RoomTypeID (int, PK)
- RoomTypeName (string)
- TypeDescription (string)
- TypeNote (string)

### RoomInformation
- RoomID (int, PK)
- RoomNumber (string)
- RoomDescription (string)
- RoomMaxCapacity (int)
- RoomStatus (int) - 1: Active, 2: Deleted
- RoomPricePerDate (decimal)
- RoomTypeID (int, FK)

### Booking
- BookingID (int, PK)
- CustomerID (int, FK)
- RoomID (int, FK)
- BookingDate (DateTime)
- CheckInDate (DateTime)
- CheckOutDate (DateTime)
- TotalAmount (decimal)
- BookingStatus (int) - 1: Pending, 2: Confirmed, 3: Cancelled
- CreatedDate (DateTime)

## 🎯 Tính năng nổi bật

### 1. Authentication
- Đăng nhập với email và password
- Phân quyền Admin/Customer
- Session management

### 2. CRUD Operations
- **Create**: Thêm mới khách hàng, phòng, đặt phòng
- **Read**: Xem danh sách, tìm kiếm, lọc
- **Update**: Cập nhật thông tin
- **Delete**: Soft delete (đánh dấu xóa)

### 3. Business Logic
- Validation đầu vào
- Tính toán tự động giá phòng
- Kiểm tra trùng lặp (email, số phòng)
- Quản lý trạng thái

### 4. UI/UX Features
- Sidebar navigation với icons
- Search và filter functionality
- Modal dialogs cho CRUD
- Responsive data grids
- Toast notifications
- Confirmation dialogs

## 📝 Hướng dẫn sử dụng

### Đăng nhập
1. Mở ứng dụng
2. Nhập email và password
3. Click "Đăng nhập"

### Admin Dashboard
1. **Dashboard**: Xem thống kê tổng quan
2. **Customers**: Quản lý khách hàng
   - Click "➕ Thêm mới" để thêm khách hàng
   - Click "✏️" để sửa
   - Click "🗑️" để xóa
3. **Rooms**: Quản lý phòng
   - Tương tự như Customers
4. **Bookings**: Quản lý đặt phòng
   - Xem danh sách
   - Lọc theo trạng thái
   - Sửa hoặc hủy booking
5. **Reports**: Xem báo cáo theo thời gian

### Customer Dashboard
1. **Thông tin cá nhân**: Cập nhật profile
2. **Đặt phòng**: Đặt phòng mới
   - Chọn phòng
   - Chọn ngày check-in/check-out
   - Xem tổng tiền tự động
3. **Lịch sử**: Xem lịch sử đặt phòng

## 🐛 Troubleshooting

### Build errors
```bash
dotnet clean
dotnet restore
dotnet build
```

### Runtime errors
- Kiểm tra .NET 9.0 SDK đã cài đặt
- Kiểm tra Windows version compatibility

## 👨‍💻 Developer

Developed with ❤️ for FU Mini Hotel System

## 📄 License

This project is for educational purposes.

---

**Note**: Đây là project demo với in-memory database. Trong production, nên sử dụng database thực như SQL Server, PostgreSQL, etc.

