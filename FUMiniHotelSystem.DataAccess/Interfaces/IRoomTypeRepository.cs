using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.DataAccess.Interfaces
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        Task<IEnumerable<RoomType>> GetActiveRoomTypesAsync();
        Task<bool> IsRoomTypeNameExistsAsync(string roomTypeName, int? excludeId = null);
    }
}

