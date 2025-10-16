using System.Windows;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessLogic.Services;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthenticationService _authService = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";
            ErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var request = new LoginRequest
                {
                    Email = EmailTextBox.Text,
                    Password = PasswordBox.Password
                };

                var response = _authService.Login(request);

                if (response.IsSuccess)
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
                else
                {
                    ErrorTextBlock.Text = response.Message;
                    ErrorTextBlock.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Lỗi: {ex.Message}";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}



