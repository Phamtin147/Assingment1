using System.Windows;
using System.Windows.Controls;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Services;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class RoomDialog : Window
    {
        private readonly RoomService _roomService = new();
        private RoomInformation? _room;
        private bool _isEditMode;

        public bool IsSaved { get; private set; }

        public RoomDialog(RoomInformation? room = null)
        {
            InitializeComponent();
            _room = room;
            _isEditMode = room != null;

            LoadRoomTypes();

            if (_isEditMode)
            {
                Title = "Sửa thông tin phòng";
                TitleTextBlock.Text = "Sửa thông tin phòng";
                LoadRoomData();
            }
            else
            {
                Title = "Thêm phòng mới";
                TitleTextBlock.Text = "Thêm phòng mới";
            }
        }

        private void LoadRoomTypes()
        {
            var roomTypes = _roomService.GetAllRoomTypes();
            RoomTypeComboBox.ItemsSource = roomTypes;
            if (roomTypes.Count > 0)
            {
                RoomTypeComboBox.SelectedIndex = 0;
            }
        }

        private void LoadRoomData()
        {
            if (_room != null)
            {
                RoomNumberTextBox.Text = _room.RoomNumber;
                DescriptionTextBox.Text = _room.RoomDescription;
                MaxCapacityTextBox.Text = _room.RoomMaxCapacity.ToString();
                PriceTextBox.Text = _room.RoomPricePerDate.ToString("F0");
                
                RoomTypeComboBox.SelectedValue = _room.RoomTypeID;
                
                foreach (ComboBoxItem item in StatusComboBox.Items)
                {
                    if (item.Tag.ToString() == _room.RoomStatus.ToString())
                    {
                        StatusComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(RoomNumberTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập số phòng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    RoomNumberTextBox.Focus();
                    return;
                }

                if (RoomTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại phòng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    RoomTypeComboBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(MaxCapacityTextBox.Text) || !int.TryParse(MaxCapacityTextBox.Text, out int maxCapacity) || maxCapacity <= 0)
                {
                    MessageBox.Show("Vui lòng nhập sức chứa hợp lệ (số nguyên dương)", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MaxCapacityTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(PriceTextBox.Text) || !decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Vui lòng nhập giá thuê hợp lệ (số dương)", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    PriceTextBox.Focus();
                    return;
                }

                var selectedRoomType = RoomTypeComboBox.SelectedItem as RoomType;
                var selectedStatus = int.Parse(((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString()!);

                // Create or update room
                if (_isEditMode && _room != null)
                {
                    _room.RoomNumber = RoomNumberTextBox.Text;
                    _room.RoomDescription = DescriptionTextBox.Text;
                    _room.RoomMaxCapacity = maxCapacity;
                    _room.RoomPricePerDate = price;
                    _room.RoomTypeID = selectedRoomType!.RoomTypeID;
                    _room.RoomStatus = selectedStatus;

                    _roomService.UpdateRoom(_room);
                    MessageBox.Show("Cập nhật phòng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var newRoom = new RoomInformation
                    {
                        RoomNumber = RoomNumberTextBox.Text,
                        RoomDescription = DescriptionTextBox.Text,
                        RoomMaxCapacity = maxCapacity,
                        RoomPricePerDate = price,
                        RoomTypeID = selectedRoomType!.RoomTypeID,
                        RoomStatus = selectedStatus
                    };

                    _roomService.AddRoom(newRoom);
                    MessageBox.Show("Thêm phòng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }

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

