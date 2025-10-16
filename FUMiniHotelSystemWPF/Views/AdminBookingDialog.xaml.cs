using System.Windows;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Services;
using System.Windows.Controls;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class AdminBookingDialog : Window
    {
        private readonly BookingService _bookingService = new();
        private readonly CustomerService _customerService = new();
        private readonly RoomService _roomService = new();
        private Booking _booking;

        public bool IsSaved { get; private set; }

        public AdminBookingDialog(Booking booking)
        {
            InitializeComponent();
            _booking = booking;
            
            LoadCustomers();
            LoadRooms();
            LoadBookingData();
        }

        private void LoadCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            CustomerComboBox.ItemsSource = customers;
        }

        private void LoadRooms()
        {
            var rooms = _roomService.GetAllRooms();
            RoomComboBox.ItemsSource = rooms;
        }

        private void LoadBookingData()
        {
            BookingIdTextBox.Text = _booking.BookingID.ToString();
            CustomerComboBox.SelectedValue = _booking.CustomerID;
            RoomComboBox.SelectedValue = _booking.RoomID;
            CheckInDatePicker.SelectedDate = _booking.CheckInDate;
            CheckOutDatePicker.SelectedDate = _booking.CheckOutDate;
            
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Tag.ToString() == _booking.BookingStatus.ToString())
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }

            CalculateTotalAmount();
        }

        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateTotalAmount();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            if (RoomComboBox.SelectedItem is RoomInformation selectedRoom &&
                CheckInDatePicker.SelectedDate.HasValue &&
                CheckOutDatePicker.SelectedDate.HasValue)
            {
                var checkIn = CheckInDatePicker.SelectedDate.Value;
                var checkOut = CheckOutDatePicker.SelectedDate.Value;
                
                if (checkOut > checkIn)
                {
                    int numberOfDays = (checkOut - checkIn).Days;
                    decimal totalAmount = numberOfDays * selectedRoom.RoomPricePerDate;
                    TotalAmountTextBlock.Text = $"{totalAmount:N0} VND";
                }
                else
                {
                    TotalAmountTextBlock.Text = "0 VND";
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (CustomerComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CustomerComboBox.Focus();
                    return;
                }

                if (RoomComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    RoomComboBox.Focus();
                    return;
                }

                if (!CheckInDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn ngày check-in", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CheckInDatePicker.Focus();
                    return;
                }

                if (!CheckOutDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn ngày check-out", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CheckOutDatePicker.Focus();
                    return;
                }

                if (StatusComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    StatusComboBox.Focus();
                    return;
                }

                var selectedRoom = (RoomInformation)RoomComboBox.SelectedItem;
                var checkIn = CheckInDatePicker.SelectedDate.Value;
                var checkOut = CheckOutDatePicker.SelectedDate.Value;

                if (checkOut <= checkIn)
                {
                    MessageBox.Show("Ngày check-out phải sau ngày check-in", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CheckOutDatePicker.Focus();
                    return;
                }

                int numberOfDays = (checkOut - checkIn).Days;
                decimal totalAmount = numberOfDays * selectedRoom.RoomPricePerDate;

                _booking.CustomerID = int.Parse(CustomerComboBox.SelectedValue.ToString()!);
                _booking.RoomID = selectedRoom.RoomID;
                _booking.CheckInDate = checkIn;
                _booking.CheckOutDate = checkOut;
                _booking.TotalAmount = totalAmount;
                _booking.BookingStatus = int.Parse(((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString()!);

                _bookingService.UpdateBooking(_booking);
                MessageBox.Show("Cập nhật đặt phòng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                IsSaved = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsSaved = false;
            Close();
        }
    }
}

