using System.Collections.ObjectModel;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace Assignment_01.ViewModels
{
    public class CustomerDashboardViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly IRoomTypeService _roomTypeService;

        private Customer _customer = null!;
        private Booking? _selectedBooking;
        private RoomInformation? _selectedAvailableRoom;
        private int _selectedRoomTypeId;
        private decimal _totalAmount;
        private string _statusMessage = "Sẵn sàng";
        private string _currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        public CustomerDashboardViewModel(LoginResponse loginResponse)
        {
            _customerService = new CustomerService(new CustomerRepository());
            _roomService = new RoomService(new RoomRepository(), new RoomTypeRepository());
            _bookingService = new BookingService(new BookingRepository(), new RoomRepository(), new CustomerRepository());
            _roomTypeService = new RoomTypeService(new RoomTypeRepository());

            WelcomeMessage = $"Xin chào, {loginResponse.CustomerFullName}";
            
            BookingHistory = new ObservableCollection<Booking>();
            AvailableRooms = new ObservableCollection<RoomInformation>();
            RoomTypes = new ObservableCollection<RoomType>();
            NewBooking = new Booking { CustomerID = loginResponse.CustomerID };

            LoadCustomerDataAsync(loginResponse.CustomerID);
            LoadRoomTypesAsync();
            LoadBookingHistoryAsync(loginResponse.CustomerID);
            
            // Update time every second
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => CurrentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
        }

        public string WelcomeMessage { get; set; } = string.Empty;

        public Customer Customer
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }

        public ObservableCollection<Booking> BookingHistory { get; set; }
        public ObservableCollection<RoomInformation> AvailableRooms { get; set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }

        public Booking NewBooking { get; set; } = new();

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set => SetProperty(ref _selectedBooking, value);
        }

        public RoomInformation? SelectedAvailableRoom
        {
            get => _selectedAvailableRoom;
            set
            {
                SetProperty(ref _selectedAvailableRoom, value);
                if (value != null)
                {
                    CalculateTotalAmount();
                }
            }
        }

        public int SelectedRoomTypeId
        {
            get => _selectedRoomTypeId;
            set => SetProperty(ref _selectedRoomTypeId, value);
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
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

        private async void LoadCustomerDataAsync(int customerId)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(customerId);
                if (customer != null)
                {
                    Customer = customer;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tải thông tin khách hàng: {ex.Message}";
            }
        }

        private async void LoadRoomTypesAsync()
        {
            try
            {
                var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
                RoomTypes.Clear();
                foreach (var roomType in roomTypes)
                {
                    RoomTypes.Add(roomType);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tải loại phòng: {ex.Message}";
            }
        }

        private async void LoadBookingHistoryAsync(int customerId)
        {
            await LoadBookingHistoryAsync();
        }

        public async Task LoadBookingHistoryAsync()
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByCustomerAsync(Customer.CustomerID);
                BookingHistory.Clear();
                foreach (var booking in bookings)
                {
                    BookingHistory.Add(booking);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tải lịch sử đặt phòng: {ex.Message}";
            }
        }

        private async void CalculateTotalAmount()
        {
            if (SelectedAvailableRoom != null && NewBooking.CheckInDate != default && NewBooking.CheckOutDate != default)
            {
                try
                {
                    TotalAmount = await _bookingService.CalculateTotalAmountAsync(
                        SelectedAvailableRoom.RoomID, 
                        NewBooking.CheckInDate, 
                        NewBooking.CheckOutDate);
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Lỗi khi tính tổng tiền: {ex.Message}";
                }
            }
        }
    }
}
