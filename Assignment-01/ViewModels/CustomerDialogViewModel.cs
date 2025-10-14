using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.BusinessLogic.Services;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace Assignment_01.ViewModels
{
    public class CustomerDialogViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private Customer _customer;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;
        private bool _isNewCustomer;
        private Dictionary<string, string> _validationErrors = new();

        public CustomerDialogViewModel(Customer? customer = null)
        {
            _customerService = new CustomerService(new CustomerRepository());
            
            if (customer == null)
            {
                _customer = new Customer();
                _isNewCustomer = true;
                DialogTitle = "Thêm khách hàng mới";
            }
            else
            {
                _customer = new Customer
                {
                    CustomerID = customer.CustomerID,
                    CustomerFullName = customer.CustomerFullName,
                    EmailAddress = customer.EmailAddress,
                    Telephone = customer.Telephone,
                    CustomerBirthday = customer.CustomerBirthday,
                    CustomerStatus = customer.CustomerStatus,
                    Password = customer.Password
                };
                _isNewCustomer = false;
                DialogTitle = "Sửa thông tin khách hàng";
            }

            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => !IsLoading);
        }

        public string DialogTitle { get; set; } = string.Empty;

        public Customer Customer
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
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
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsNewCustomer
        {
            get => _isNewCustomer;
            set => SetProperty(ref _isNewCustomer, value);
        }

        public Dictionary<string, string> ValidationErrors
        {
            get => _validationErrors;
            set => SetProperty(ref _validationErrors, value);
        }

        public ICommand SaveCommand { get; }

        public event EventHandler<bool>? DialogResult;

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

                if (IsNewCustomer)
                {
                    Customer.Password = Password;
                    await _customerService.CreateCustomerAsync(Customer);
                }
                else
                {
                    await _customerService.UpdateCustomerAsync(Customer);
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

            // Validate CustomerFullName
            if (string.IsNullOrWhiteSpace(Customer.CustomerFullName))
            {
                errors["CustomerFullName"] = "Họ và tên không được để trống";
            }
            else if (Customer.CustomerFullName.Length > 50)
            {
                errors["CustomerFullName"] = "Họ và tên không được quá 50 ký tự";
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(Customer.EmailAddress))
            {
                errors["EmailAddress"] = "Email không được để trống";
            }
            else if (!IsValidEmail(Customer.EmailAddress))
            {
                errors["EmailAddress"] = "Email không hợp lệ";
            }
            else if (Customer.EmailAddress.Length > 50)
            {
                errors["EmailAddress"] = "Email không được quá 50 ký tự";
            }

            // Validate Telephone
            if (string.IsNullOrWhiteSpace(Customer.Telephone))
            {
                errors["Telephone"] = "Số điện thoại không được để trống";
            }
            else if (Customer.Telephone.Length > 12)
            {
                errors["Telephone"] = "Số điện thoại không được quá 12 ký tự";
            }

            // Validate Birthday
            if (Customer.CustomerBirthday == default)
            {
                errors["CustomerBirthday"] = "Ngày sinh không được để trống";
            }
            else if (Customer.CustomerBirthday > DateTime.Today)
            {
                errors["CustomerBirthday"] = "Ngày sinh không thể là ngày trong tương lai";
            }

            // Validate Password for new customer
            if (IsNewCustomer)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    errors["Password"] = "Mật khẩu không được để trống";
                }
                else if (Password.Length > 50)
                {
                    errors["Password"] = "Mật khẩu không được quá 50 ký tự";
                }

                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    errors["ConfirmPassword"] = "Xác nhận mật khẩu không được để trống";
                }
                else if (Password != ConfirmPassword)
                {
                    errors["ConfirmPassword"] = "Mật khẩu xác nhận không khớp";
                }
            }

            ValidationErrors = errors;
            return errors.Count == 0;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
