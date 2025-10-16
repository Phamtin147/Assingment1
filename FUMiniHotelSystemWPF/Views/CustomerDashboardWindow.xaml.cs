using System.Windows;
using System.Windows.Controls;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessLogic.Services;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class CustomerDashboardWindow : Window
    {
        private readonly CustomerService _customerService = new();
        private readonly BookingService _bookingService = new();
        private readonly LoginResponse _currentUser;

        public CustomerDashboardWindow(LoginResponse user)
        {
            InitializeComponent();
            _currentUser = user;
            WelcomeTextBlock.Text = $"{user.CustomerFullName}";
            LoadProfile();
            LoadBookingHistory();
        }

        private void LoadProfile()
        {
            var customer = _customerService.GetCustomerById(_currentUser.CustomerID);
            if (customer != null)
            {
                FullNameTextBox.Text = customer.CustomerFullName;
                EmailTextBox.Text = customer.EmailAddress;
                PhoneTextBox.Text = customer.Telephone;
                BirthdayPicker.SelectedDate = customer.CustomerBirthday;
            }
        }

        private void LoadBookingHistory()
        {
            if (BookingHistoryDataGrid != null)
            {
                var bookings = _bookingService.GetBookingsByCustomer(_currentUser.CustomerID);
                BookingHistoryDataGrid.ItemsSource = bookings;
            }
        }

        // Navigation Methods
        private void NavigateToProfile(object sender, RoutedEventArgs e)
        {
            SetActiveView(ProfileView);
            SetActiveButton((Button)sender);
            LoadProfile();
        }

        private void NavigateToBooking(object sender, RoutedEventArgs e)
        {
            SetActiveView(BookingView);
            SetActiveButton((Button)sender);
        }

        private void NavigateToHistory(object sender, RoutedEventArgs e)
        {
            SetActiveView(HistoryView);
            SetActiveButton((Button)sender);
            LoadBookingHistory();
        }

        private void SetActiveView(UIElement activeView)
        {
            ProfileView.Visibility = Visibility.Collapsed;
            BookingView.Visibility = Visibility.Collapsed;
            HistoryView.Visibility = Visibility.Collapsed;
            
            activeView.Visibility = Visibility.Visible;
        }

        private void SetActiveButton(Button activeButton)
        {
            ProfileBtn.Tag = null;
            BookingBtn.Tag = null;
            HistoryBtn.Tag = null;
            
            activeButton.Tag = "Selected";
        }

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = _customerService.GetCustomerById(_currentUser.CustomerID);
                if (customer != null)
                {
                    customer.CustomerFullName = FullNameTextBox.Text;
                    customer.Telephone = PhoneTextBox.Text;
                    customer.CustomerBirthday = BirthdayPicker.SelectedDate ?? customer.CustomerBirthday;

                    _customerService.UpdateCustomer(customer);
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewBooking_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BookingDialog(_currentUser.CustomerID);
            if (dialog.ShowDialog() == true || dialog.IsSaved)
            {
                LoadBookingHistory();
                MessageBox.Show("Đặt phòng thành công!", "Thành công", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshBookings_Click(object sender, RoutedEventArgs e)
        {
            LoadBookingHistory();
            MessageBox.Show("Đã làm mới danh sách đặt phòng!", "Thông báo", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

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
