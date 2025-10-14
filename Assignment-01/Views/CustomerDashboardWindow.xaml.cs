using System.Windows;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using Assignment_01.ViewModels;

namespace Assignment_01.Views
{
    public partial class CustomerDashboardWindow : Window
    {
        public CustomerDashboardWindow(LoginResponse loginResponse)
        {
            InitializeComponent();
            DataContext = new CustomerDashboardViewModel(loginResponse);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement update profile
            MessageBox.Show("Chức năng cập nhật thông tin sẽ được triển khai", "Thông báo");
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement change password
            MessageBox.Show("Chức năng đổi mật khẩu sẽ được triển khai", "Thông báo");
        }

        private void NewBooking_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomerDashboardViewModel customerViewModel)
            {
                var viewModel = new BookingDialogViewModel(customerViewModel.Customer.CustomerID);
                var dialog = new BookingDialog(viewModel);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Refresh booking history
                    customerViewModel.LoadBookingHistoryAsync();
                    MessageBox.Show("Đặt phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ViewBookingDetails_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement view booking details
            MessageBox.Show("Chức năng xem chi tiết đặt phòng sẽ được triển khai", "Thông báo");
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement cancel booking
            MessageBox.Show("Chức năng hủy đặt phòng sẽ được triển khai", "Thông báo");
        }

        private void SearchAvailableRooms_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement search available rooms
            MessageBox.Show("Chức năng tìm phòng khả dụng sẽ được triển khai", "Thông báo");
        }

        private void ConfirmBooking_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement confirm booking
            MessageBox.Show("Chức năng xác nhận đặt phòng sẽ được triển khai", "Thông báo");
        }
    }
}

