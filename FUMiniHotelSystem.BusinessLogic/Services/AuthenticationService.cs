using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;

        public AuthenticationService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var customer = await _customerRepository.GetByEmailAsync(request.Email);
            
            if (customer == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Email không tồn tại trong hệ thống."
                };
            }

            if (customer.CustomerStatus != 1) // Not Active
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Tài khoản đã bị vô hiệu hóa."
                };
            }

            if (customer.Password != request.Password)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Mật khẩu không chính xác."
                };
            }

            var isAdmin = await IsAdminAsync(request.Email);

            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Đăng nhập thành công.",
                CustomerID = customer.CustomerID,
                CustomerFullName = customer.CustomerFullName,
                EmailAddress = customer.EmailAddress,
                IsAdmin = isAdmin
            };
        }

        public async Task<bool> IsAdminAsync(string email)
        {
            return await Task.FromResult(email == "admin@FUMiniHotelSystem.com");
        }
    }
}

