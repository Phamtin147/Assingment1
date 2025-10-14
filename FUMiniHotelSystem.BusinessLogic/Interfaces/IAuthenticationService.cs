using FUMiniHotelSystem.BusinessObjects.DTOs;

namespace FUMiniHotelSystem.BusinessLogic.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> IsAdminAsync(string email);
    }
}

