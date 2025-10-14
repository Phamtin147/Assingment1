using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.BusinessLogic.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomInformation>> GetAllRoomsAsync();
        Task<RoomInformation?> GetRoomByIdAsync(int id);
        Task<RoomInformation> CreateRoomAsync(RoomInformation room);
        Task<RoomInformation> UpdateRoomAsync(RoomInformation room);
        Task DeleteRoomAsync(int id);
        Task<IEnumerable<RoomInformation>> SearchRoomsAsync(string searchTerm);
        Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<RoomInformation>> GetRoomsByTypeAsync(int roomTypeId);
        Task<bool> IsRoomNumberExistsAsync(string roomNumber, int? excludeId = null);
    }
}

