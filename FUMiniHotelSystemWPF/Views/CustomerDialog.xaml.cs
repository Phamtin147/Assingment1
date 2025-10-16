using System.Windows;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Services;

namespace FUMiniHotelSystemWPF.Views
{
    public partial class CustomerDialog : Window
    {
        private readonly CustomerService _customerService = new();
        private Customer? _customer;
        private bool _isEditMode;

        public bool IsSaved { get; private set; }

        public CustomerDialog(Customer? customer = null)
        {
            InitializeComponent();
            _customer = customer;
            _isEditMode = customer != null;

            if (_isEditMode)
            {
                Title = "Sửa thông tin khách hàng";
                TitleTextBlock.Text = "Sửa thông tin khách hàng";
                LoadCustomerData();
                PasswordBox.IsEnabled = false;
                ConfirmPasswordBox.IsEnabled = false;
                PasswordBox.Visibility = Visibility.Collapsed;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                PasswordLabel.Visibility = Visibility.Collapsed;
                ConfirmPasswordLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                Title = "Thêm khách hàng mới";
                TitleTextBlock.Text = "Thêm khách hàng mới";
            }
        }

        private void LoadCustomerData()
        {
            if (_customer != null)
            {
                FullNameTextBox.Text = _customer.CustomerFullName;
                EmailTextBox.Text = _customer.EmailAddress;
                PhoneTextBox.Text = _customer.Telephone;
                BirthdayPicker.SelectedDate = _customer.CustomerBirthday;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    FullNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập email", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    EmailTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    PhoneTextBox.Focus();
                    return;
                }

                if (!BirthdayPicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn ngày sinh", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    BirthdayPicker.Focus();
                    return;
                }

                if (!_isEditMode)
                {
                    if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PasswordBox.Focus();
                        return;
                    }

                    if (PasswordBox.Password != ConfirmPasswordBox.Password)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        ConfirmPasswordBox.Focus();
                        return;
                    }
                }

                // Create or update customer
                if (_isEditMode && _customer != null)
                {
                    _customer.CustomerFullName = FullNameTextBox.Text;
                    _customer.EmailAddress = EmailTextBox.Text;
                    _customer.Telephone = PhoneTextBox.Text;
                    _customer.CustomerBirthday = BirthdayPicker.SelectedDate.Value;
                    
                    _customerService.UpdateCustomer(_customer);
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var newCustomer = new Customer
                    {
                        CustomerFullName = FullNameTextBox.Text,
                        EmailAddress = EmailTextBox.Text,
                        Telephone = PhoneTextBox.Text,
                        CustomerBirthday = BirthdayPicker.SelectedDate.Value,
                        Password = PasswordBox.Password,
                        CustomerStatus = 1
                    };

                    _customerService.AddCustomer(newCustomer);
                    MessageBox.Show("Thêm khách hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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

