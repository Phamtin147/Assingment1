using System.Windows;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystemWPF.ViewModels;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;

            _viewModel.LoginSuccess += OnLoginSuccess;
            _viewModel.CloseRequested += OnCloseRequested;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
            _viewModel.ClearError();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
            _viewModel.LoginCommand.Execute(null);
        }

        private void OnLoginSuccess(object? sender, LoginResponse response)
        {
            if (response.IsAdmin)
            {
                var adminWindow = new AdminDashboardWindow(response);
                adminWindow.Show();
            }
            else
            {
                var customerWindow = new CustomerDashboardWindow(response);
                customerWindow.Show();
            }
            this.Close();
        }

        private void OnCloseRequested(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}



