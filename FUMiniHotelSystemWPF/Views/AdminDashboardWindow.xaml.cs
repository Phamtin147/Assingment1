using System.Windows;
using System.Windows.Controls;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessLogic.Services;
using System.Linq;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class AdminDashboardWindow : Window
    {
        private readonly CustomerService _customerService = new();
        private readonly RoomService _roomService = new();
        private readonly BookingService _bookingService = new();
        private readonly LoginResponse _currentUser;

        public AdminDashboardWindow(LoginResponse user)
        {
            InitializeComponent();
            _currentUser = user;
            WelcomeTextBlock.Text = $"{user.CustomerFullName}";
            
            LoadDashboardData();
            LoadCustomers();
            LoadRooms();
            LoadAllBookings();
        }

        private void LoadDashboardData()
        {
            var customers = _customerService.GetAllCustomers();
            var rooms = _roomService.GetAllRooms();
            var bookings = _bookingService.GetAllBookings();

            TotalCustomersText.Text = customers.Count.ToString();
            TotalRoomsText.Text = rooms.Count.ToString();
            TotalBookingsText.Text = bookings.Count.ToString();
            TotalRevenueText.Text = $"{bookings.Sum(b => b.TotalAmount):N0} VND";
        }

        private void LoadCustomers()
        {
            if (CustomerDataGrid != null)
            {
                CustomerDataGrid.ItemsSource = _customerService.GetAllCustomers();
            }
        }

        private void LoadRooms()
        {
            if (RoomDataGrid != null)
            {
                RoomDataGrid.ItemsSource = _roomService.GetAllRooms();
            }
        }

        private void LoadAllBookings()
        {
            if (AllBookingsDataGrid != null)
            {
                var bookings = _bookingService.GetAllBookings();
                AllBookingsDataGrid.ItemsSource = bookings;
            }
        }

        // Navigation Methods
        private void NavigateToDashboard(object sender, RoutedEventArgs e)
        {
            SetActiveView(DashboardView);
            SetActiveButton((Button)sender);
            LoadDashboardData();
        }

        private void NavigateToCustomers(object sender, RoutedEventArgs e)
        {
            SetActiveView(CustomersView);
            SetActiveButton((Button)sender);
            LoadCustomers();
        }

        private void NavigateToRooms(object sender, RoutedEventArgs e)
        {
            SetActiveView(RoomsView);
            SetActiveButton((Button)sender);
            LoadRooms();
        }

        private void NavigateToBookings(object sender, RoutedEventArgs e)
        {
            SetActiveView(BookingsView);
            SetActiveButton((Button)sender);
            LoadAllBookings();
        }

        private void NavigateToReports(object sender, RoutedEventArgs e)
        {
            SetActiveView(ReportsView);
            SetActiveButton((Button)sender);
        }

        private void SetActiveView(UIElement activeView)
        {
            DashboardView.Visibility = Visibility.Collapsed;
            CustomersView.Visibility = Visibility.Collapsed;
            RoomsView.Visibility = Visibility.Collapsed;
            BookingsView.Visibility = Visibility.Collapsed;
            ReportsView.Visibility = Visibility.Collapsed;
            
            activeView.Visibility = Visibility.Visible;
        }

        private void SetActiveButton(Button activeButton)
        {
            DashboardBtn.Tag = null;
            CustomersBtn.Tag = null;
            RoomsBtn.Tag = null;
            BookingsBtn.Tag = null;
            ReportsBtn.Tag = null;
            
            activeButton.Tag = "Selected";
        }

        // Customer Methods
        private void SearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerDataGrid.ItemsSource = _customerService.SearchCustomers(CustomerSearchBox.Text);
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CustomerDialog();
            if (dialog.ShowDialog() == true || dialog.IsSaved)
            {
                LoadCustomers();
                LoadDashboardData();
            }
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                var customer = _customerService.GetCustomerById(customerId);
                if (customer != null)
                {
                    var dialog = new CustomerDialog(customer);
                    if (dialog.ShowDialog() == true || dialog.IsSaved)
                    {
                        LoadCustomers();
                    }
                }
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _customerService.DeleteCustomer(customerId);
                    LoadCustomers();
                    LoadDashboardData();
                    MessageBox.Show("Đã xóa thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Room Methods
        private void SearchRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomDataGrid.ItemsSource = _roomService.SearchRooms(RoomSearchBox.Text);
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RoomDialog();
            if (dialog.ShowDialog() == true || dialog.IsSaved)
            {
                LoadRooms();
                LoadDashboardData();
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int roomId)
            {
                var room = _roomService.GetRoomById(roomId);
                if (room != null)
                {
                    var dialog = new RoomDialog(room);
                    if (dialog.ShowDialog() == true || dialog.IsSaved)
                    {
                        LoadRooms();
                    }
                }
            }
        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int roomId)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa phòng này?", "Xác nhận", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _roomService.DeleteRoom(roomId);
                    LoadRooms();
                    LoadDashboardData();
                    MessageBox.Show("Đã xóa thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Booking Methods
        private void RefreshAllBookings_Click(object sender, RoutedEventArgs e)
        {
            LoadAllBookings();
            if (BookingStatusFilter != null)
            {
                BookingStatusFilter.SelectedIndex = 0;
            }
            MessageBox.Show("Đã làm mới danh sách đặt phòng!", "Thông báo", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BookingStatusFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (AllBookingsDataGrid != null && BookingStatusFilter.SelectedItem is ComboBoxItem selectedItem)
            {
                int statusFilter = int.Parse(selectedItem.Tag.ToString()!);
                var allBookings = _bookingService.GetAllBookings();

                if (statusFilter == 0)
                {
                    AllBookingsDataGrid.ItemsSource = allBookings;
                }
                else
                {
                    AllBookingsDataGrid.ItemsSource = allBookings.Where(b => b.BookingStatus == statusFilter).ToList();
                }
            }
        }

        private void EditBooking_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int bookingId)
            {
                var booking = _bookingService.GetAllBookings().FirstOrDefault(b => b.BookingID == bookingId);
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
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int bookingId)
            {
                if (MessageBox.Show("Bạn có chắc muốn hủy đặt phòng này?", "Xác nhận", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _bookingService.DeleteBooking(bookingId);
                    LoadAllBookings();
                    LoadDashboardData();
                    MessageBox.Show("Đã hủy đặt phòng thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Report Methods
        private void ViewReport_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và kết thúc", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var bookings = _bookingService.GetBookingsByDateRange(
                StartDatePicker.SelectedDate.Value,
                EndDatePicker.SelectedDate.Value
            );
            ReportDataGrid.ItemsSource = bookings;
        }

        // Logout Method
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}

