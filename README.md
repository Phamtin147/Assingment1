# FU Mini Hotel System

## 📋 Tổng quan dự án
Hệ thống quản lý khách sạn mini được xây dựng bằng WPF (Windows Presentation Foundation) với kiến trúc 3-Layer và MVVM pattern.

## 🏗️ Kiến trúc

### 1. ✅ 3-Layers Architecture
```
├── Presentation Layer (FUMiniHotelSystemWPF)
│   ├── Views (XAML + Code-behind)
│   ├── ViewModels (MVVM Pattern)
│   ├── Commands (RelayCommand)
│   ├── Converters
│   └── Styles
│
├── Business Logic Layer (FUMiniHotelSystem.BusinessLogic)
│   └── Services
│       ├── AuthenticationService
│       ├── CustomerService
│       ├── RoomService
│       └── BookingService
│
└── Data Access Layer (FUMiniHotelSystem.DataAccess)
    ├── Database (InMemoryDatabase - Singleton)
    └── Repositories (Repository Pattern)
        ├── CustomerRepository
        ├── RoomRepository
        ├── BookingRepository
        └── RoomTypeRepository
```

### 2. ✅ MVVM Pattern Implementation
- **ViewModelBase**: Base class với INotifyPropertyChanged
- **RelayCommand**: ICommand implementation cho command binding
- **ViewModels**:
  - `LoginViewModel`: Xử lý đăng nhập
  - `AdminDashboardViewModel`: Quản lý dashboard admin
  - `CustomerDashboardViewModel`: Quản lý dashboard khách hàng
- **Data Binding**: Two-way binding cho tất cả UI elements
- **Command Binding**: Thay thế tất cả Click events bằng Commands
- **ObservableCollection**: Tự động cập nhật UI khi data thay đổi

### 3. ✅ Design Patterns
- **Repository Pattern**: Tách biệt data access logic
- **Singleton Pattern**: InMemoryDatabase instance duy nhất
- **DTO Pattern**: LoginRequest, LoginResponse
- **MVVM Pattern**: Tách biệt View và Business Logic

## 🎯 Các yêu cầu đã hoàn thành

| Yêu cầu | Trạng thái | Ghi chú |
|---------|-----------|---------|
| ✅ WPF + Class Library (.dll) | **Hoàn thành** | 4 projects: WPF + 3 DLLs |
| ✅ List persisting customers & rooms | **Hoàn thành** | InMemoryDatabase với List<> |
| ✅ LINQ to Object | **Hoàn thành** | Dùng LINQ trong Repositories & Services |
| ✅ Passing data in WPF | **Hoàn thành** | LoginResponse, DTOs, Data Binding |
| ✅ 3-Layers architecture | **Hoàn thành** | Presentation, Business Logic, Data Access |
| ✅ **MVVM pattern** | **Hoàn thành** | ViewModels, Commands, Data Binding |
| ✅ Repository pattern | **Hoàn thành** | 4 Repositories |
| ✅ Singleton pattern | **Hoàn thành** | InMemoryDatabase.Instance |
| ✅ CRUD + Searching | **Hoàn thành** | Đầy đủ cho Customer, Room, Booking |
| ✅ Data validation | **Hoàn thành** | Validation trong Services |

## 🚀 Cách chạy ứng dụng

### Yêu cầu hệ thống
- .NET 8.0 SDK
- Windows 10/11
- Visual Studio 2022 (hoặc VS Code với C# extension)

### Chạy ứng dụng

#### Option 1: Sử dụng Visual Studio
1. Mở file `FUMiniHotelSystem.sln`
2. Set `FUMiniHotelSystemWPF` làm startup project
3. Nhấn F5 hoặc click "Start"

#### Option 2: Sử dụng Command Line
```bash
cd FUMiniHotelSystemWPF
dotnet run
```

#### Option 3: Build và chạy
```bash
# Build toàn bộ solution
dotnet build

# Chạy ứng dụng
cd FUMiniHotelSystemWPF/bin/Debug/net8.0-windows
./PhamTrungTinWPF.exe
```

## 👤 Tài khoản đăng nhập

### Admin Account
- **Email**: `admin@FUMiniHotelSystem.com`
- **Password**: `@@abc123@@`

### Customer Accounts
| Email | Password | Họ tên |
|-------|----------|--------|
| nguyenvana@email.com | password123 | Nguyen Van A |
| tranthib@email.com | password456 | Tran Thi B |
| levanc@email.com | password789 | Le Van C |
| phamthid@email.com | password321 | Pham Thi D |

## 📦 Cấu trúc Projects

### FUMiniHotelSystem.BusinessObjects (Class Library)
- **Models**: Customer, RoomInformation, RoomType, Booking
- **DTOs**: LoginRequest, LoginResponse

### FUMiniHotelSystem.DataAccess (Class Library)
- **Database**: InMemoryDatabase (Singleton)
- **Repositories**: CRUD operations với LINQ

### FUMiniHotelSystem.BusinessLogic (Class Library)
- **Services**: Business logic & validation
- Dependency: BusinessObjects, DataAccess

### FUMiniHotelSystemWPF (WPF Application)
- **Views**: XAML files
- **ViewModels**: MVVM implementation
- **Commands**: RelayCommand
- **Converters**: BookingStatusConverter
- **Styles**: Modern UI với gradient theme
- Dependency: BusinessObjects, BusinessLogic

## 🎨 Tính năng UI/UX

### Modern Design
- ✅ Purple gradient theme
- ✅ Sidebar navigation
- ✅ Dashboard cards với icons
- ✅ Responsive layout
- ✅ Custom styled controls (TextBox, ComboBox, DatePicker, DataGrid)
- ✅ Smooth transitions

### Admin Dashboard
- ✅ **Dashboard**: Thống kê tổng quan (Customers, Rooms, Bookings, Revenue)
- ✅ **Customer Management**: CRUD operations với search
- ✅ **Room Management**: CRUD operations với search
- ✅ **Booking Management**: View, Edit, Cancel bookings với filter
- ✅ **Reports**: Báo cáo theo khoảng thời gian

### Customer Portal
- ✅ **Profile**: Xem và cập nhật thông tin cá nhân
- ✅ **Booking**: Đặt phòng mới
- ✅ **History**: Xem lịch sử đặt phòng

## 🔧 Công nghệ sử dụng

- **Framework**: .NET 8.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Pattern**: MVVM, Repository, Singleton
- **Data Query**: LINQ to Objects
- **Data Storage**: In-Memory List<>
- **Architecture**: 3-Layers (Presentation, Business Logic, Data Access)

## 📝 LINQ Examples trong Code

### Repository Layer
```csharp
// CustomerRepository.cs
public List<Customer> GetAll() => 
    _db.Customers.Where(c => c.CustomerStatus == 1).ToList();

public Customer? GetByEmail(string email) => 
    _db.Customers.FirstOrDefault(c => c.EmailAddress == email);

public bool EmailExists(string email, int? excludeId = null) =>
    _db.Customers.Any(c => c.EmailAddress == email && 
        (!excludeId.HasValue || c.CustomerID != excludeId.Value));
```

### Service Layer
```csharp
// CustomerService.cs
public List<Customer> SearchCustomers(string searchTerm) =>
    _customerRepository.GetAll()
        .Where(c => c.CustomerFullName.Contains(searchTerm, 
            StringComparison.OrdinalIgnoreCase) ||
            c.EmailAddress.Contains(searchTerm, 
            StringComparison.OrdinalIgnoreCase))
        .ToList();
```

### ViewModel Layer
```csharp
// AdminDashboardViewModel.cs
private void FilterBookings()
{
    Bookings.Clear();
    var allBookings = _bookingService.GetAllBookings();
    
    var filtered = SelectedBookingStatusFilter == 0 
        ? allBookings 
        : allBookings.Where(b => b.BookingStatus == SelectedBookingStatusFilter);
    
    foreach (var booking in filtered)
    {
        Bookings.Add(booking);
    }
}
```

## 🎯 MVVM Implementation Details

### ViewModelBase
```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    protected bool SetProperty<T>(ref T field, T value, 
        [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
```

### RelayCommand
```csharp
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;
    
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    public bool CanExecute(object? parameter) => 
        _canExecute == null || _canExecute(parameter);
    
    public void Execute(object? parameter) => _execute(parameter);
}
```

### Data Binding Examples
```xaml
<!-- Two-way binding -->
<TextBox Text="{Binding Email, UpdateSourceTrigger=PropertyChanged}"/>

<!-- Command binding -->
<Button Content="Login" Command="{Binding LoginCommand}"/>

<!-- Collection binding -->
<DataGrid ItemsSource="{Binding Customers}"/>

<!-- Visibility binding -->
<Grid Visibility="{Binding DashboardViewVisibility}"/>
```

## 📊 Database Schema (In-Memory)

### Customer
- CustomerID (int, PK)
- CustomerFullName (string)
- Telephone (string)
- EmailAddress (string, unique)
- CustomerBirthday (DateTime)
- CustomerStatus (byte): 1=Active, 2=Deleted
- Password (string)

### RoomType
- RoomTypeID (int, PK)
- RoomTypeName (string)
- TypeDescription (string)
- TypeNote (string)

### RoomInformation
- RoomID (int, PK)
- RoomNumber (string, unique)
- RoomDescription (string)
- RoomMaxCapacity (int)
- RoomStatus (byte): 1=Active, 2=Deleted
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
- BookingStatus (byte): 1=Pending, 2=Confirmed, 3=Cancelled
- CreatedDate (DateTime)

## 🔐 Security Features
- Password-based authentication
- Role-based access control (Admin vs Customer)
- Soft delete for data integrity

## 📈 Future Enhancements
- [ ] Add ViewModels for Dialog windows
- [ ] Implement Unit Tests
- [ ] Add database persistence (SQL Server/SQLite)
- [ ] Add payment integration
- [ ] Email notifications
- [ ] Multi-language support
- [ ] Advanced reporting features

## 👨‍💻 Developed By
**Student Name**: [Your Name]  
**Course**: PRN212  
**University**: FPT University

---

## 📝 Notes
- Dự án sử dụng In-Memory database, data sẽ mất khi tắt ứng dụng
- MVVM pattern đã được implement đầy đủ cho main windows
- Dialog windows vẫn dùng code-behind (có thể nâng cấp thêm)
- UI/UX được thiết kế hiện đại với purple gradient theme
