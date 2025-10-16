using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystemWPF.Commands;
using FUMiniHotelSystemWPF.Views;

namespace FUMiniHotelSystemWPF.ViewModels
{
    public class CustomerDashboardViewModel : ViewModelBase
    {
        private readonly CustomerService _customerService = new();
        private readonly BookingService _bookingService = new();
        private readonly LoginResponse _currentUser;

        private string _welcomeText = string.Empty;
        private string _fullName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private DateTime? _birthday;

        private Visibility _profileViewVisibility = Visibility.Visible;
        private Visibility _bookingViewVisibility = Visibility.Collapsed;
        private Visibility _historyViewVisibility = Visibility.Collapsed;

        private string? _profileButtonTag = "Selected";
        private string? _bookingButtonTag;
        private string? _historyButtonTag;

        public ObservableCollection<dynamic> BookingHistory { get; } = new();

        public string WelcomeText
        {
            get => _welcomeText;
            set => SetProperty(ref _welcomeText, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public DateTime? Birthday
        {
            get => _birthday;
            set => SetProperty(ref _birthday, value);
        }

        public Visibility ProfileViewVisibility
        {
            get => _profileViewVisibility;
            set => SetProperty(ref _profileViewVisibility, value);
        }

        public Visibility BookingViewVisibility
        {
            get => _bookingViewVisibility;
            set => SetProperty(ref _bookingViewVisibility, value);
        }

        public Visibility HistoryViewVisibility
        {
            get => _historyViewVisibility;
            set => SetProperty(ref _historyViewVisibility, value);
        }

        public string? ProfileButtonTag
        {
            get => _profileButtonTag;
            set => SetProperty(ref _profileButtonTag, value);
        }

        public string? BookingButtonTag
        {
            get => _bookingButtonTag;
            set => SetProperty(ref _bookingButtonTag, value);
        }

        public string? HistoryButtonTag
        {
            get => _historyButtonTag;
            set => SetProperty(ref _historyButtonTag, value);
        }

        // Commands
        public ICommand NavigateToProfileCommand { get; }
        public ICommand NavigateToBookingCommand { get; }
        public ICommand NavigateToHistoryCommand { get; }
        public ICommand UpdateProfileCommand { get; }
        public ICommand NewBookingCommand { get; }
        public ICommand RefreshBookingsCommand { get; }
        public ICommand LogoutCommand { get; }

        public event EventHandler? LogoutRequested;

        public CustomerDashboardViewModel(LoginResponse user)
        {
            _currentUser = user;
            WelcomeText = user.CustomerFullName;

            // Initialize Commands
            NavigateToProfileCommand = new RelayCommand(_ => NavigateToProfile());
            NavigateToBookingCommand = new RelayCommand(_ => NavigateToBooking());
            NavigateToHistoryCommand = new RelayCommand(_ => NavigateToHistory());
            UpdateProfileCommand = new RelayCommand(_ => UpdateProfile());
            NewBookingCommand = new RelayCommand(_ => NewBooking());
            RefreshBookingsCommand = new RelayCommand(_ => RefreshBookings());
            LogoutCommand = new RelayCommand(_ => Logout());

            // Load initial data
            LoadProfile();
            LoadBookingHistory();
        }

        private void NavigateToProfile()
        {
            SetActiveView(0);
            LoadProfile();
        }

        private void NavigateToBooking()
        {
            SetActiveView(1);
        }

        private void NavigateToHistory()
        {
            SetActiveView(2);
            LoadBookingHistory();
        }

        private void SetActiveView(int viewIndex)
        {
            ProfileViewVisibility = viewIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            BookingViewVisibility = viewIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            HistoryViewVisibility = viewIndex == 2 ? Visibility.Visible : Visibility.Collapsed;

            // Update button tags for highlighting
            ProfileButtonTag = viewIndex == 0 ? "Selected" : null;
            BookingButtonTag = viewIndex == 1 ? "Selected" : null;
            HistoryButtonTag = viewIndex == 2 ? "Selected" : null;
        }

        private void LoadProfile()
        {
            var customer = _customerService.GetCustomerById(_currentUser.CustomerID);
            if (customer != null)
            {
                FullName = customer.CustomerFullName;
                Email = customer.EmailAddress;
                Phone = customer.Telephone;
                Birthday = customer.CustomerBirthday;
            }
        }

        private void UpdateProfile()
        {
            try
            {
                var customer = _customerService.GetCustomerById(_currentUser.CustomerID);
                if (customer != null)
                {
                    customer.CustomerFullName = FullName;
                    customer.Telephone = Phone;
                    customer.CustomerBirthday = Birthday ?? customer.CustomerBirthday;

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

        private void LoadBookingHistory()
        {
            BookingHistory.Clear();
            var bookings = _bookingService.GetBookingsByCustomer(_currentUser.CustomerID);
            foreach (var booking in bookings)
            {
                BookingHistory.Add(booking);
            }
        }

        private void NewBooking()
        {
            var dialog = new BookingDialog(_currentUser.CustomerID);
            if (dialog.ShowDialog() == true || dialog.IsSaved)
            {
                LoadBookingHistory();
                MessageBox.Show("Đặt phòng thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshBookings()
        {
            LoadBookingHistory();
            MessageBox.Show("Đã làm mới danh sách đặt phòng!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Logout()
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LogoutRequested?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

