using System.Collections.ObjectModel;
using System.Windows.Input;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace Assignment_01.ViewModels
{
    public class BookingDialogViewModel : BaseViewModel
    {
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly IRoomTypeService _roomTypeService;

        private DateTime _checkInDate = DateTime.Today.AddDays(1);
        private DateTime _checkOutDate = DateTime.Today.AddDays(2);
        private int _selectedRoomTypeId;
        private RoomInformation? _selectedRoom;
        private decimal _totalAmount;
        private int _numberOfNights;
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;
        private int _customerId;

        public BookingDialogViewModel(int customerId)
        {
            _roomService = new RoomService(new RoomRepository(), new RoomTypeRepository());
            _bookingService = new BookingService(new BookingRepository(), new RoomRepository(), new CustomerRepository());
            _roomTypeService = new RoomTypeService(new RoomTypeRepository());
            
            _customerId = customerId;
            
            AvailableRooms = new ObservableCollection<RoomInformation>();
            RoomTypes = new ObservableCollection<RoomType>();
            
            SearchRoomsCommand = new RelayCommand(async () => await SearchAvailableRoomsAsync(), () => !IsLoading);
            FilterRoomsCommand = new RelayCommand(async () => await FilterRoomsAsync(), () => !IsLoading);
            ConfirmBookingCommand = new RelayCommand(async () => await ConfirmBookingAsync(), () => CanConfirmBooking && !IsLoading);
            
            LoadRoomTypesAsync();
        }

        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                SetProperty(ref _checkInDate, value);
                CalculateTotalAmount();
                ((RelayCommand)SearchRoomsCommand).RaiseCanExecuteChanged();
            }
        }

        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                SetProperty(ref _checkOutDate, value);
                CalculateTotalAmount();
                ((RelayCommand)SearchRoomsCommand).RaiseCanExecuteChanged();
            }
        }

        public int SelectedRoomTypeId
        {
            get => _selectedRoomTypeId;
            set
            {
                SetProperty(ref _selectedRoomTypeId, value);
                ((RelayCommand)FilterRoomsCommand).RaiseCanExecuteChanged();
            }
        }

        public RoomInformation? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                SetProperty(ref _selectedRoom, value);
                CalculateTotalAmount();
                ((RelayCommand)ConfirmBookingCommand).RaiseCanExecuteChanged();
            }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
        }

        public int NumberOfNights
        {
            get => _numberOfNights;
            set => SetProperty(ref _numberOfNights, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                ((RelayCommand)SearchRoomsCommand).RaiseCanExecuteChanged();
                ((RelayCommand)FilterRoomsCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ConfirmBookingCommand).RaiseCanExecuteChanged();
            }
        }

        public bool CanConfirmBooking => SelectedRoom != null && CheckInDate < CheckOutDate && CheckInDate >= DateTime.Today;

        public ObservableCollection<RoomInformation> AvailableRooms { get; set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }

        public ICommand SearchRoomsCommand { get; }
        public ICommand FilterRoomsCommand { get; }
        public ICommand ConfirmBookingCommand { get; }

        public event EventHandler<bool>? DialogResult;

        private async void LoadRoomTypesAsync()
        {
            try
            {
                var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
                RoomTypes.Clear();
                foreach (var roomType in roomTypes)
                {
                    RoomTypes.Add(roomType);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi tải loại phòng: {ex.Message}";
            }
        }

        private async Task SearchAvailableRoomsAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                if (!ValidateDates())
                {
                    return;
                }

                var rooms = await _roomService.GetAvailableRoomsAsync(CheckInDate, CheckOutDate);
                AvailableRooms.Clear();
                foreach (var room in rooms)
                {
                    AvailableRooms.Add(room);
                }

                if (AvailableRooms.Count == 0)
                {
                    ErrorMessage = "Không có phòng khả dụng trong khoảng thời gian đã chọn.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi tìm phòng: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FilterRoomsAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                if (!ValidateDates())
                {
                    return;
                }

                var rooms = await _roomService.GetAvailableRoomsAsync(CheckInDate, CheckOutDate);
                
                if (SelectedRoomTypeId > 0)
                {
                    rooms = rooms.Where(r => r.RoomTypeID == SelectedRoomTypeId);
                }

                AvailableRooms.Clear();
                foreach (var room in rooms)
                {
                    AvailableRooms.Add(room);
                }

                if (AvailableRooms.Count == 0)
                {
                    ErrorMessage = "Không có phòng khả dụng với bộ lọc đã chọn.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi lọc phòng: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ConfirmBookingAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                if (SelectedRoom == null)
                {
                    ErrorMessage = "Vui lòng chọn phòng để đặt.";
                    return;
                }

                if (!ValidateDates())
                {
                    return;
                }

                var booking = new Booking
                {
                    CustomerID = _customerId,
                    RoomID = SelectedRoom.RoomID,
                    BookingDate = DateTime.Now,
                    CheckInDate = CheckInDate,
                    CheckOutDate = CheckOutDate,
                    TotalAmount = TotalAmount,
                    BookingStatus = 1 // Pending
                };

                await _bookingService.CreateBookingAsync(booking);
                DialogResult?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi đặt phòng: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ValidateDates()
        {
            if (CheckInDate < DateTime.Today)
            {
                ErrorMessage = "Ngày check-in không thể là ngày trong quá khứ.";
                return false;
            }

            if (CheckInDate >= CheckOutDate)
            {
                ErrorMessage = "Ngày check-in phải nhỏ hơn ngày check-out.";
                return false;
            }

            return true;
        }

        private void CalculateTotalAmount()
        {
            if (SelectedRoom != null && CheckInDate < CheckOutDate)
            {
                NumberOfNights = (CheckOutDate - CheckInDate).Days;
                TotalAmount = SelectedRoom.RoomPricePerDate * NumberOfNights;
            }
            else
            {
                NumberOfNights = 0;
                TotalAmount = 0;
            }
        }
    }
}
