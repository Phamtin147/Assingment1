using System;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystemWPF.Commands;
using FUMiniHotelSystemWPF.Views;

namespace FUMiniHotelSystemWPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthenticationService _authService = new();
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private Visibility _errorVisibility = Visibility.Collapsed;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public Visibility ErrorVisibility
        {
            get => _errorVisibility;
            set => SetProperty(ref _errorVisibility, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand CloseCommand { get; }

        public event EventHandler<LoginResponse>? LoginSuccess;
        public event EventHandler? CloseRequested;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(_ => ExecuteLogin());
            CloseCommand = new RelayCommand(_ => ExecuteClose());
        }

        private void ExecuteLogin()
        {
            try
            {
                var request = new LoginRequest
                {
                    Email = Email,
                    Password = Password
                };

                var response = _authService.Login(request);

                if (response.IsSuccess)
                {
                    LoginSuccess?.Invoke(this, response);
                }
                else
                {
                    ErrorMessage = response.Message;
                    ErrorVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi: {ex.Message}";
                ErrorVisibility = Visibility.Visible;
            }
        }

        private void ExecuteClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public void ClearError()
        {
            ErrorMessage = string.Empty;
            ErrorVisibility = Visibility.Collapsed;
        }
    }
}

