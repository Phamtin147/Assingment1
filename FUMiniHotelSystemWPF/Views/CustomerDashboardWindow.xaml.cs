using System.Windows;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystemWPF.ViewModels;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class CustomerDashboardWindow : Window
    {
        private readonly CustomerDashboardViewModel _viewModel;

        public CustomerDashboardWindow(LoginResponse user)
        {
            InitializeComponent();
            _viewModel = new CustomerDashboardViewModel(user);
            DataContext = _viewModel;

            _viewModel.LogoutRequested += OnLogoutRequested;
        }

        private void OnLogoutRequested(object? sender, EventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
