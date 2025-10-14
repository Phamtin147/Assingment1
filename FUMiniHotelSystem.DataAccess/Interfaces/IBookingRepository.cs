using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.DataAccess.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(int customerId);
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Booking>> GetBookingsByRoomAsync(int roomId);
        Task<IEnumerable<Booking>> GetActiveBookingsAsync();
    }
}

