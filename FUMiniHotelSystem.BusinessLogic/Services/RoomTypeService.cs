using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _roomTypeRepository.GetActiveRoomTypesAsync();
        }

        public async Task<RoomType?> GetRoomTypeByIdAsync(int id)
        {
            return await _roomTypeRepository.GetByIdAsync(id);
        }

        public async Task<RoomType> CreateRoomTypeAsync(RoomType roomType)
        {
            // Validate room type name uniqueness
            if (await _roomTypeRepository.IsRoomTypeNameExistsAsync(roomType.RoomTypeName))
            {
                throw new InvalidOperationException("Tên loại phòng đã tồn tại trong hệ thống.");
            }

            return await _roomTypeRepository.AddAsync(roomType);
        }

        public async Task<RoomType> UpdateRoomTypeAsync(RoomType roomType)
        {
            var existingRoomType = await _roomTypeRepository.GetByIdAsync(roomType.RoomTypeID);
            if (existingRoomType == null)
            {
                throw new InvalidOperationException("Loại phòng không tồn tại.");
            }

            // Validate room type name uniqueness (excluding current room type)
            if (await _roomTypeRepository.IsRoomTypeNameExistsAsync(roomType.RoomTypeName, roomType.RoomTypeID))
            {
                throw new InvalidOperationException("Tên loại phòng đã tồn tại trong hệ thống.");
            }

            await _roomTypeRepository.UpdateAsync(roomType);
            return roomType;
        }

        public async Task DeleteRoomTypeAsync(int id)
        {
            var roomType = await _roomTypeRepository.GetByIdAsync(id);
            if (roomType == null)
            {
                throw new InvalidOperationException("Loại phòng không tồn tại.");
            }

            await _roomTypeRepository.DeleteAsync(id);
        }

        public async Task<bool> IsRoomTypeNameExistsAsync(string roomTypeName, int? excludeId = null)
        {
            return await _roomTypeRepository.IsRoomTypeNameExistsAsync(roomTypeName, excludeId);
        }
    }
}

