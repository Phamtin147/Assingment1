using System.Windows;
using Assignment_01.ViewModels;
using FUMiniHotelSystem.BusinessObjects.DTOs;

namespace Assignment_01.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            
            var viewModel = (LoginViewModel)DataContext;
            viewModel.LoginSuccessful += OnLoginSuccessful;
        }

        private void OnLoginSuccessful(object? sender, LoginResponse response)
        {
            if (response.IsAdmin)
            {
                // Open Admin Dashboard
                var adminWindow = new AdminDashboardWindow(response);
                adminWindow.Show();
            }
            else
            {
                // Open Customer Dashboard
                var customerWindow = new CustomerDashboardWindow(response);
                customerWindow.Show();
            }
            
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((System.Windows.Controls.PasswordBox)sender).Password;
            }
        }
    }
}

