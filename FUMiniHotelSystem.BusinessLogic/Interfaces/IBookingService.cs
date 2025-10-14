using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessObjects.DTOs;

namespace FUMiniHotelSystem.BusinessLogic.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(int customerId);
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(ReportRequest request);
        Task<IEnumerable<Booking>> GetBookingsByRoomAsync(int roomId);
        Task<decimal> CalculateTotalAmountAsync(int roomId, DateTime checkIn, DateTime checkOut);
    }
}

