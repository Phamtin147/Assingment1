using System.Collections.ObjectModel;
using System.Windows.Input;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace Assignment_01.ViewModels
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly IRoomTypeService _roomTypeService;

        private Customer? _selectedCustomer;
        private RoomInformation? _selectedRoom;
        private Booking? _selectedBooking;
        private DateTime _reportStartDate = DateTime.Today.AddDays(-30);
        private DateTime _reportEndDate = DateTime.Today;
        private string _statusMessage = "Sẵn sàng";
        private string _currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        public AdminDashboardViewModel(LoginResponse loginResponse)
        {
            _customerService = new CustomerService(new CustomerRepository());
            _roomService = new RoomService(new RoomRepository(), new RoomTypeRepository());
            _bookingService = new BookingService(new BookingRepository(), new RoomRepository(), new CustomerRepository());
            _roomTypeService = new RoomTypeService(new RoomTypeRepository());

            WelcomeMessage = $"Xin chào, {loginResponse.CustomerFullName} (Admin)";
            
            Customers = new ObservableCollection<Customer>();
            Rooms = new ObservableCollection<RoomInformation>();
            Bookings = new ObservableCollection<Booking>();
            ReportData = new ObservableCollection<ReportItem>();

            LoadDataAsync();
            
            // Update time every second
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => CurrentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
        }

        public string WelcomeMessage { get; set; } = string.Empty;

        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<RoomInformation> Rooms { get; set; }
        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<ReportItem> ReportData { get; set; }

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        public RoomInformation? SelectedRoom
        {
            get => _selectedRoom;
            set => SetProperty(ref _selectedRoom, value);
        }

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set => SetProperty(ref _selectedBooking, value);
        }

        public DateTime ReportStartDate
        {
            get => _reportStartDate;
            set => SetProperty(ref _reportStartDate, value);
        }

        public DateTime ReportEndDate
        {
            get => _reportEndDate;
            set => SetProperty(ref _reportEndDate, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public string CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }

        private async void LoadDataAsync()
        {
            await LoadCustomersAsync();
            await LoadRoomsAsync();
            await LoadBookingsAsync();
        }

        public async Task LoadCustomersAsync()
        {
            try
            {
                StatusMessage = "Đang tải danh sách khách hàng...";
                var customers = await _customerService.GetAllCustomersAsync();

                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }

                StatusMessage = "Dữ liệu đã được tải thành công";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tải danh sách khách hàng: {ex.Message}";
            }
        }

        public async Task LoadRoomsAsync()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();

                Rooms.Clear();
                foreach (var room in rooms)
                {
                    Rooms.Add(room);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tải danh sách phòng: {ex.Message}";
            }
        }

        public async Task LoadBookingsAsync()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();

                Bookings.Clear();
                foreach (var booking in bookings)
                {
                    Bookings.Add(booking);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tải danh sách đặt phòng: {ex.Message}";
            }
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            await _customerService.DeleteCustomerAsync(customerId);
        }

        public async Task DeleteRoomAsync(int roomId)
        {
            await _roomService.DeleteRoomAsync(roomId);
        }

        public async Task GenerateReportAsync()
        {
            try
            {
                StatusMessage = "Đang tạo báo cáo...";
                
                var request = new ReportRequest
                {
                    StartDate = ReportStartDate,
                    EndDate = ReportEndDate
                };

                var bookings = await _bookingService.GetBookingsByDateRangeAsync(request);
                
                // Group bookings by date and create report items
                var reportData = bookings
                    .GroupBy(b => b.BookingDate.Date)
                    .Select(g => new ReportItem
                    {
                        Date = g.Key,
                        BookingCount = g.Count(),
                        Revenue = g.Sum(b => b.TotalAmount),
                        PopularRoom = g.GroupBy(b => b.RoomInformation.RoomNumber)
                                      .OrderByDescending(rg => rg.Count())
                                      .First().Key
                    })
                    .OrderByDescending(r => r.Date)
                    .ToList();

                ReportData.Clear();
                foreach (var item in reportData)
                {
                    ReportData.Add(item);
                }

                StatusMessage = $"Báo cáo đã được tạo thành công. Tổng cộng {reportData.Count} ngày có dữ liệu.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tạo báo cáo: {ex.Message}";
            }
        }

        public async Task SearchCustomersAsync(string searchTerm)
        {
            try
            {
                var customers = await _customerService.SearchCustomersAsync(searchTerm);
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tìm kiếm khách hàng: {ex.Message}";
            }
        }

        public async Task SearchRoomsAsync(string searchTerm)
        {
            try
            {
                var rooms = await _roomService.SearchRoomsAsync(searchTerm);
                Rooms.Clear();
                foreach (var room in rooms)
                {
                    Rooms.Add(room);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tìm kiếm phòng: {ex.Message}";
            }
        }
    }

    public class ReportItem
    {
        public DateTime Date { get; set; }
        public int BookingCount { get; set; }
        public decimal Revenue { get; set; }
        public string PopularRoom { get; set; } = string.Empty;
    }
}
