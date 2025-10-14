using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace Assignment_01.ViewModels
{
    public class RoomDialogViewModel : BaseViewModel
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        private RoomInformation _room;
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;
        private bool _isNewRoom;
        private Dictionary<string, string> _validationErrors = new();

        public RoomDialogViewModel(RoomInformation? room = null)
        {
            _roomService = new RoomService(new RoomRepository(), new RoomTypeRepository());
            _roomTypeService = new RoomTypeService(new RoomTypeRepository());
            
            if (room == null)
            {
                _room = new RoomInformation { RoomStatus = 1 };
                _isNewRoom = true;
                DialogTitle = "Thêm phòng mới";
            }
            else
            {
                _room = new RoomInformation
                {
                    RoomID = room.RoomID,
                    RoomNumber = room.RoomNumber,
                    RoomDescription = room.RoomDescription,
                    RoomMaxCapacity = room.RoomMaxCapacity,
                    RoomStatus = room.RoomStatus,
                    RoomPricePerDate = room.RoomPricePerDate,
                    RoomTypeID = room.RoomTypeID
                };
                _isNewRoom = false;
                DialogTitle = "Sửa thông tin phòng";
            }

            RoomTypes = new ObservableCollection<RoomType>();
            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => !IsLoading);
            
            LoadRoomTypesAsync();
        }

        public string DialogTitle { get; set; } = string.Empty;

        public RoomInformation Room
        {
            get => _room;
            set => SetProperty(ref _room, value);
        }

        public ObservableCollection<RoomType> RoomTypes { get; set; }

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
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsNewRoom
        {
            get => _isNewRoom;
            set => SetProperty(ref _isNewRoom, value);
        }

        public Dictionary<string, string> ValidationErrors
        {
            get => _validationErrors;
            set => SetProperty(ref _validationErrors, value);
        }

        public ICommand SaveCommand { get; }

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

        private async Task SaveAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                ValidationErrors.Clear();

                // Validate input
                if (!ValidateInput())
                {
                    return;
                }

                if (IsNewRoom)
                {
                    await _roomService.CreateRoomAsync(Room);
                }
                else
                {
                    await _roomService.UpdateRoomAsync(Room);
                }

                DialogResult?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ValidateInput()
        {
            var errors = new Dictionary<string, string>();

            // Validate RoomNumber
            if (string.IsNullOrWhiteSpace(Room.RoomNumber))
            {
                errors["RoomNumber"] = "Số phòng không được để trống";
            }
            else if (Room.RoomNumber.Length > 50)
            {
                errors["RoomNumber"] = "Số phòng không được quá 50 ký tự";
            }

            // Validate RoomDescription
            if (!string.IsNullOrWhiteSpace(Room.RoomDescription) && Room.RoomDescription.Length > 220)
            {
                errors["RoomDescription"] = "Mô tả phòng không được quá 220 ký tự";
            }

            // Validate RoomMaxCapacity
            if (Room.RoomMaxCapacity <= 0)
            {
                errors["RoomMaxCapacity"] = "Sức chứa tối đa phải lớn hơn 0";
            }

            // Validate RoomPricePerDate
            if (Room.RoomPricePerDate <= 0)
            {
                errors["RoomPricePerDate"] = "Giá mỗi ngày phải lớn hơn 0";
            }

            // Validate RoomTypeID
            if (Room.RoomTypeID <= 0)
            {
                errors["RoomTypeID"] = "Vui lòng chọn loại phòng";
            }

            ValidationErrors = errors;
            return errors.Count == 0;
        }
    }
}
