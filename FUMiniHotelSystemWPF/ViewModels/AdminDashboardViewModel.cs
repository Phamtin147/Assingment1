using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystemWPF.Commands;
using FUMiniHotelSystemWPF.Views;

namespace FUMiniHotelSystemWPF.ViewModels
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        private readonly CustomerService _customerService = new();
        private readonly RoomService _roomService = new();
        private readonly BookingService _bookingService = new();
        private readonly LoginResponse _currentUser;

        private string _welcomeText = string.Empty;
        private string _totalCustomers = "0";
        private string _totalRooms = "0";
        private string _totalBookings = "0";
        private string _totalRevenue = "0 VND";
        private string _customerSearchText = string.Empty;
        private string _roomSearchText = string.Empty;
        private int _selectedBookingStatusFilter = 0;

        private Visibility _dashboardViewVisibility = Visibility.Visible;
        private Visibility _customersViewVisibility = Visibility.Collapsed;
        private Visibility _roomsViewVisibility = Visibility.Collapsed;
        private Visibility _bookingsViewVisibility = Visibility.Collapsed;
        private Visibility _reportsViewVisibility = Visibility.Collapsed;

        private string? _dashboardButtonTag = "Selected";
        private string? _customersButtonTag;
        private string? _roomsButtonTag;
        private string? _bookingsButtonTag;
        private string? _reportsButtonTag;

        private DateTime? _reportStartDate;
        private DateTime? _reportEndDate;

        public ObservableCollection<Customer> Customers { get; } = new();
        public ObservableCollection<RoomInformation> Rooms { get; } = new();
        public ObservableCollection<dynamic> Bookings { get; } = new();
        public ObservableCollection<dynamic> ReportBookings { get; } = new();

        public string WelcomeText
        {
            get => _welcomeText;
            set => SetProperty(ref _welcomeText, value);
        }

        public string TotalCustomers
        {
            get => _totalCustomers;
            set => SetProperty(ref _totalCustomers, value);
        }

        public string TotalRooms
        {
            get => _totalRooms;
            set => SetProperty(ref _totalRooms, value);
        }

        public string TotalBookings
        {
            get => _totalBookings;
            set => SetProperty(ref _totalBookings, value);
        }

        public string TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        public string CustomerSearchText
        {
            get => _customerSearchText;
            set => SetProperty(ref _customerSearchText, value);
        }

        public string RoomSearchText
        {
            get => _roomSearchText;
            set => SetProperty(ref _roomSearchText, value);
        }

        public int SelectedBookingStatusFilter
        {
            get => _selectedBookingStatusFilter;
            set
            {
                if (SetProperty(ref _selectedBookingStatusFilter, value))
                {
                    FilterBookings();
                }
            }
        }

        public DateTime? ReportStartDate
        {
            get => _reportStartDate;
            set => SetProperty(ref _reportStartDate, value);
        }

        public DateTime? ReportEndDate
        {
            get => _reportEndDate;
            set => SetProperty(ref _reportEndDate, value);
        }

        public Visibility DashboardViewVisibility
        {
            get => _dashboardViewVisibility;
            set => SetProperty(ref _dashboardViewVisibility, value);
        }

        public Visibility CustomersViewVisibility
        {
            get => _customersViewVisibility;
            set => SetProperty(ref _customersViewVisibility, value);
        }

        public Visibility RoomsViewVisibility
        {
            get => _roomsViewVisibility;
            set => SetProperty(ref _roomsViewVisibility, value);
        }

        public Visibility BookingsViewVisibility
        {
            get => _bookingsViewVisibility;
            set => SetProperty(ref _bookingsViewVisibility, value);
        }

        public Visibility ReportsViewVisibility
        {
            get => _reportsViewVisibility;
            set => SetProperty(ref _reportsViewVisibility, value);
        }

        public string? DashboardButtonTag
        {
            get => _dashboardButtonTag;
            set => SetProperty(ref _dashboardButtonTag, value);
        }

        public string? CustomersButtonTag
        {
            get => _customersButtonTag;
            set => SetProperty(ref _customersButtonTag, value);
        }

        public string? RoomsButtonTag
        {
            get => _roomsButtonTag;
            set => SetProperty(ref _roomsButtonTag, value);
        }

        public string? BookingsButtonTag
        {
            get => _bookingsButtonTag;
            set => SetProperty(ref _bookingsButtonTag, value);
        }

        public string? ReportsButtonTag
        {
            get => _reportsButtonTag;
            set => SetProperty(ref _reportsButtonTag, value);
        }

        // Commands
        public ICommand NavigateToDashboardCommand { get; }
        public ICommand NavigateToCustomersCommand { get; }
        public ICommand NavigateToRoomsCommand { get; }
        public ICommand NavigateToBookingsCommand { get; }
        public ICommand NavigateToReportsCommand { get; }
        public ICommand SearchCustomerCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }
        public ICommand SearchRoomCommand { get; }
        public ICommand AddRoomCommand { get; }
        public ICommand EditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }
        public ICommand RefreshBookingsCommand { get; }
        public ICommand EditBookingCommand { get; }
        public ICommand CancelBookingCommand { get; }
        public ICommand ViewReportCommand { get; }
        public ICommand LogoutCommand { get; }

        public event EventHandler? LogoutRequested;

        public AdminDashboardViewModel(LoginResponse user)
        {
            _currentUser = user;
            WelcomeText = user.CustomerFullName;

            // Initialize Commands
            NavigateToDashboardCommand = new RelayCommand(_ => NavigateToDashboard());
            NavigateToCustomersCommand = new RelayCommand(_ => NavigateToCustomers());
            NavigateToRoomsCommand = new RelayCommand(_ => NavigateToRooms());
            NavigateToBookingsCommand = new RelayCommand(_ => NavigateToBookings());
            NavigateToReportsCommand = new RelayCommand(_ => NavigateToReports());
            
            SearchCustomerCommand = new RelayCommand(_ => SearchCustomers());
            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            EditCustomerCommand = new RelayCommand(param => EditCustomer(param as int?));
            DeleteCustomerCommand = new RelayCommand(param => DeleteCustomer(param as int?));
            
            SearchRoomCommand = new RelayCommand(_ => SearchRooms());
            AddRoomCommand = new RelayCommand(_ => AddRoom());
            EditRoomCommand = new RelayCommand(param => EditRoom(param as int?));
            DeleteRoomCommand = new RelayCommand(param => DeleteRoom(param as int?));
            
            RefreshBookingsCommand = new RelayCommand(_ => RefreshBookings());
            EditBookingCommand = new RelayCommand(param => EditBooking(param as int?));
            CancelBookingCommand = new RelayCommand(param => CancelBooking(param as int?));
            
            ViewReportCommand = new RelayCommand(_ => ViewReport());
            LogoutCommand = new RelayCommand(_ => Logout());

            // Load initial data
            LoadDashboardData();
            LoadCustomers();
            LoadRooms();
            LoadAllBookings();
        }

        private void NavigateToDashboard()
        {
            SetActiveView(0);
            LoadDashboardData();
        }

        private void NavigateToCustomers()
        {
            SetActiveView(1);
            LoadCustomers();
        }

        private void NavigateToRooms()
        {
            SetActiveView(2);
            LoadRooms();
        }

        private void NavigateToBookings()
        {
            SetActiveView(3);
            LoadAllBookings();
        }

        private void NavigateToReports()
        {
            SetActiveView(4);
        }

        private void SetActiveView(int viewIndex)
        {
            DashboardViewVisibility = viewIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            CustomersViewVisibility = viewIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            RoomsViewVisibility = viewIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
            BookingsViewVisibility = viewIndex == 3 ? Visibility.Visible : Visibility.Collapsed;
            ReportsViewVisibility = viewIndex == 4 ? Visibility.Visible : Visibility.Collapsed;

            // Update button tags for highlighting
            DashboardButtonTag = viewIndex == 0 ? "Selected" : null;
            CustomersButtonTag = viewIndex == 1 ? "Selected" : null;
            RoomsButtonTag = viewIndex == 2 ? "Selected" : null;
            BookingsButtonTag = viewIndex == 3 ? "Selected" : null;
            ReportsButtonTag = viewIndex == 4 ? "Selected" : null;
        }

        private void LoadDashboardData()
        {
            var customers = _customerService.GetAllCustomers();
            var rooms = _roomService.GetAllRooms();
            var bookings = _bookingService.GetAllBookings();

            TotalCustomers = customers.Count.ToString();
            TotalRooms = rooms.Count.ToString();
            TotalBookings = bookings.Count.ToString();
            TotalRevenue = $"{bookings.Sum(b => b.TotalAmount):N0} VND";
        }

        private void LoadCustomers()
        {
            Customers.Clear();
            foreach (var customer in _customerService.GetAllCustomers())
            {
                Customers.Add(customer);
            }
        }

        private void SearchCustomers()
        {
            Customers.Clear();
            foreach (var customer in _customerService.SearchCustomers(CustomerSearchText))
            {
                Customers.Add(customer);
            }
        }

        private void AddCustomer()
        {
            var dialog = new CustomerDialog();
            if (dialog.ShowDialog() == true || dialog.IsSaved)
            {
                LoadCustomers();
                LoadDashboardData();
            }
        }

        private void EditCustomer(int? customerId)
        {
            if (customerId == null) return;

            var customer = _customerService.GetCustomerById(customerId.Value);
            if (customer != null)
            {
                var dialog = new CustomerDialog(customer);
                if (dialog.ShowDialog() == true || dialog.IsSaved)
                {
                    LoadCustomers();
                    LoadDashboardData();
                }
            }
        }

        private void DeleteCustomer(int? customerId)
        {
            if (customerId == null) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _customerService.DeleteCustomer(customerId.Value);
                LoadCustomers();
                LoadDashboardData();
                MessageBox.Show("Đã xóa thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadRooms()
        {
            Rooms.Clear();
            foreach (var room in _roomService.GetAllRooms())
            {
                Rooms.Add(room);
            }
        }

        private void SearchRooms()
        {
            Rooms.Clear();
            foreach (var room in _roomService.SearchRooms(RoomSearchText))
            {
                Rooms.Add(room);
            }
        }

        private void AddRoom()
        {
            var dialog = new RoomDialog();
            if (dialog.ShowDialog() == true || dialog.IsSaved)
            {
                LoadRooms();
                LoadDashboardData();
            }
        }

        private void EditRoom(int? roomId)
        {
            if (roomId == null) return;

            var room = _roomService.GetRoomById(roomId.Value);
            if (room != null)
            {
                var dialog = new RoomDialog(room);
                if (dialog.ShowDialog() == true || dialog.IsSaved)
                {
                    LoadRooms();
                    LoadDashboardData();
                }
            }
        }

        private void DeleteRoom(int? roomId)
        {
            if (roomId == null) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa phòng này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _roomService.DeleteRoom(roomId.Value);
                LoadRooms();
                LoadDashboardData();
                MessageBox.Show("Đã xóa thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadAllBookings()
        {
            Bookings.Clear();
            foreach (var booking in _bookingService.GetAllBookings())
            {
                Bookings.Add(booking);
            }
        }

        private void FilterBookings()
        {
            Bookings.Clear();
            var allBookings = _bookingService.GetAllBookings();

            if (SelectedBookingStatusFilter == 0)
            {
                foreach (var booking in allBookings)
                {
                    Bookings.Add(booking);
                }
            }
            else
            {
                foreach (var booking in allBookings.Where(b => b.BookingStatus == SelectedBookingStatusFilter))
                {
                    Bookings.Add(booking);
                }
            }
        }

        private void RefreshBookings()
        {
            LoadAllBookings();
            SelectedBookingStatusFilter = 0;
            MessageBox.Show("Đã làm mới danh sách đặt phòng!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditBooking(int? bookingId)
        {
            if (bookingId == null) return;

            var booking = _bookingService.GetAllBookings().FirstOrDefault(b => b.BookingID == bookingId.Value);
            if (booking != null)
            {
                var dialog = new AdminBookingDialog(booking);
                if (dialog.ShowDialog() == true || dialog.IsSaved)
                {
                    LoadAllBookings();
                    LoadDashboardData();
                }
            }
        }

        private void CancelBooking(int? bookingId)
        {
            if (bookingId == null) return;

            if (MessageBox.Show("Bạn có chắc muốn hủy đặt phòng này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _bookingService.DeleteBooking(bookingId.Value);
                LoadAllBookings();
                LoadDashboardData();
                MessageBox.Show("Đã hủy đặt phòng thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ViewReport()
        {
            if (ReportStartDate == null || ReportEndDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và kết thúc", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ReportBookings.Clear();
            var bookings = _bookingService.GetBookingsByDateRange(ReportStartDate.Value, ReportEndDate.Value);
            foreach (var booking in bookings)
            {
                ReportBookings.Add(booking);
            }
        }

        private void Logout()
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LogoutRequested?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

