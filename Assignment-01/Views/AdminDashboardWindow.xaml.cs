using System.Windows;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using Assignment_01.ViewModels;

namespace Assignment_01.Views
{
    public partial class AdminDashboardWindow : Window
    {
        public AdminDashboardWindow(LoginResponse loginResponse)
        {
            InitializeComponent();
            DataContext = new AdminDashboardViewModel(loginResponse);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new CustomerDialogViewModel();
            var dialog = new CustomerDialog(viewModel);
            dialog.Owner = this;
            
            if (dialog.ShowDialog() == true)
            {
                // Refresh customer list
                if (DataContext is AdminDashboardViewModel adminViewModel)
                {
                    adminViewModel.LoadCustomersAsync();
                }
            }
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel adminViewModel && adminViewModel.SelectedCustomer != null)
            {
                var viewModel = new CustomerDialogViewModel(adminViewModel.SelectedCustomer);
                var dialog = new CustomerDialog(viewModel);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Refresh customer list
                    adminViewModel.LoadCustomersAsync();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel adminViewModel && adminViewModel.SelectedCustomer != null)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa khách hàng '{adminViewModel.SelectedCustomer.CustomerFullName}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await adminViewModel.DeleteCustomerAsync(adminViewModel.SelectedCustomer.CustomerID);
                        MessageBox.Show("Xóa khách hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        adminViewModel.LoadCustomersAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void CustomerSearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel adminViewModel)
            {
                var searchTerm = ((System.Windows.Controls.TextBox)sender).Text;
                await adminViewModel.SearchCustomersAsync(searchTerm);
            }
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new RoomDialogViewModel();
            var dialog = new RoomDialog(viewModel);
            dialog.Owner = this;
            
            if (dialog.ShowDialog() == true)
            {
                // Refresh room list
                if (DataContext is AdminDashboardViewModel adminViewModel)
                {
                    adminViewModel.LoadRoomsAsync();
                }
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel adminViewModel && adminViewModel.SelectedRoom != null)
            {
                var viewModel = new RoomDialogViewModel(adminViewModel.SelectedRoom);
                var dialog = new RoomDialog(viewModel);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Refresh room list
                    adminViewModel.LoadRoomsAsync();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel adminViewModel && adminViewModel.SelectedRoom != null)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phòng '{adminViewModel.SelectedRoom.RoomNumber}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await adminViewModel.DeleteRoomAsync(adminViewModel.SelectedRoom.RoomID);
                        MessageBox.Show("Xóa phòng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        adminViewModel.LoadRoomsAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa phòng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void RoomSearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel adminViewModel)
            {
                var searchTerm = ((System.Windows.Controls.TextBox)sender).Text;
                await adminViewModel.SearchRoomsAsync(searchTerm);
            }
        }

        private void CreateBooking_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Create Booking dialog
            MessageBox.Show("Chức năng tạo đặt phòng sẽ được triển khai", "Thông báo");
        }

        private void ViewBooking_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement View Booking details
            MessageBox.Show("Chức năng xem chi tiết đặt phòng sẽ được triển khai", "Thông báo");
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Cancel Booking confirmation
            MessageBox.Show("Chức năng hủy đặt phòng sẽ được triển khai", "Thông báo");
        }

        private void FilterBookings_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement booking filter
        }

        private async void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel adminViewModel)
            {
                await adminViewModel.GenerateReportAsync();
            }
        }

        private void ExportReport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement export to Excel
            MessageBox.Show("Chức năng xuất Excel sẽ được triển khai", "Thông báo");
        }
    }
}

