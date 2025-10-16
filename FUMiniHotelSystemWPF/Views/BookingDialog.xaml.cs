using System.Windows;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Services;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class BookingDialog : Window
    {
        private readonly RoomService _roomService = new();
        private readonly BookingService _bookingService = new();
        private readonly int _customerId;

        public bool IsSaved { get; private set; }

        public BookingDialog(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadRooms();
            
            // Set minimum dates to today
            CheckInDatePicker.DisplayDateStart = DateTime.Today;
            CheckOutDatePicker.DisplayDateStart = DateTime.Today.AddDays(1);
            
            // Set default dates
            CheckInDatePicker.SelectedDate = DateTime.Today;
            CheckOutDatePicker.SelectedDate = DateTime.Today.AddDays(1);
        }

        private void LoadRooms()
        {
            var rooms = _roomService.GetAllRooms();
            RoomComboBox.ItemsSource = rooms;
            if (rooms.Count > 0)
            {
                RoomComboBox.SelectedIndex = 0;
            }
        }

        private void RoomComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CalculateTotalAmount();
        }

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Update minimum check-out date based on check-in date
            if (CheckInDatePicker.SelectedDate.HasValue)
            {
                CheckOutDatePicker.DisplayDateStart = CheckInDatePicker.SelectedDate.Value.AddDays(1);
                
                // If check-out is before check-in, adjust it
                if (CheckOutDatePicker.SelectedDate.HasValue && 
                    CheckOutDatePicker.SelectedDate.Value <= CheckInDatePicker.SelectedDate.Value)
                {
                    CheckOutDatePicker.SelectedDate = CheckInDatePicker.SelectedDate.Value.AddDays(1);
                }
            }
            
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

                    NumberOfDaysTextBlock.Text = $"{numberOfDays} ngày";
                    PricePerDayTextBlock.Text = $"{selectedRoom.RoomPricePerDate:N0} VND";
                    TotalAmountTextBlock.Text = $"{totalAmount:N0} VND";
                }
                else
                {
                    NumberOfDaysTextBlock.Text = "0 ngày";
                    PricePerDayTextBlock.Text = "0 VND";
                    TotalAmountTextBlock.Text = "0 VND";
                }
            }
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
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

                var newBooking = new Booking
                {
                    CustomerID = _customerId,
                    RoomID = selectedRoom.RoomID,
                    BookingDate = DateTime.Now,
                    CheckInDate = checkIn,
                    CheckOutDate = checkOut,
                    TotalAmount = totalAmount,
                    BookingStatus = 1 // Pending
                };

                _bookingService.AddBooking(newBooking);
                MessageBox.Show("Đặt phòng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

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

