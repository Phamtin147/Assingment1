using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class AuthenticationService
    {
        private readonly CustomerRepository _customerRepo = new();

        public LoginResponse Login(LoginRequest request)
        {
            var customer = _customerRepo.GetByEmail(request.Email);

            if (customer == null)
            {
                return new LoginResponse { IsSuccess = false, Message = "Email không tồn tại" };
            }

            if (customer.CustomerStatus != 1)
            {
                return new LoginResponse { IsSuccess = false, Message = "Tài khoản đã bị vô hiệu hóa" };
            }

            if (customer.Password != request.Password)
            {
                return new LoginResponse { IsSuccess = false, Message = "Mật khẩu không chính xác" };
            }

            bool isAdmin = customer.EmailAddress == "admin@FUMiniHotelSystem.com";

            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Đăng nhập thành công",
                CustomerID = customer.CustomerID,
                CustomerFullName = customer.CustomerFullName,
                EmailAddress = customer.EmailAddress,
                IsAdmin = isAdmin
            };
        }
    }
}




