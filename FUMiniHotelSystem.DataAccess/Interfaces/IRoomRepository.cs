using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.DataAccess.Interfaces
{
    public interface IRoomRepository : IRepository<RoomInformation>
    {
        Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<RoomInformation>> GetRoomsByTypeAsync(int roomTypeId);
        Task<bool> IsRoomNumberExistsAsync(string roomNumber, int? excludeId = null);
    }
}

