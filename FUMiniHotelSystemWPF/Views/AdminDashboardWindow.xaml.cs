using System.Windows;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystemWPF.ViewModels;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class AdminDashboardWindow : Window
    {
        private readonly AdminDashboardViewModel _viewModel;

        public AdminDashboardWindow(LoginResponse user)
        {
            InitializeComponent();
            _viewModel = new AdminDashboardViewModel(user);
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

