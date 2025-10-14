using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.BusinessLogic.Interfaces
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();
        Task<RoomType?> GetRoomTypeByIdAsync(int id);
        Task<RoomType> CreateRoomTypeAsync(RoomType roomType);
        Task<RoomType> UpdateRoomTypeAsync(RoomType roomType);
        Task DeleteRoomTypeAsync(int id);
        Task<bool> IsRoomTypeNameExistsAsync(string roomTypeName, int? excludeId = null);
    }
}

