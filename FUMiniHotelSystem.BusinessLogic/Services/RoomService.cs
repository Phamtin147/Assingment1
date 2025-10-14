using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomService(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<IEnumerable<RoomInformation>> GetAllRoomsAsync()
        {
            return await _roomRepository.FindAsync(r => r.RoomStatus == 1);
        }

        public async Task<RoomInformation?> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<RoomInformation> CreateRoomAsync(RoomInformation room)
        {
            // Validate room number uniqueness
            if (await _roomRepository.IsRoomNumberExistsAsync(room.RoomNumber))
            {
                throw new InvalidOperationException("Số phòng đã tồn tại trong hệ thống.");
            }

            // Validate room type exists
            var roomType = await _roomTypeRepository.GetByIdAsync(room.RoomTypeID);
            if (roomType == null)
            {
                throw new InvalidOperationException("Loại phòng không tồn tại.");
            }

            room.RoomStatus = 1; // Active
            return await _roomRepository.AddAsync(room);
        }

        public async Task<RoomInformation> UpdateRoomAsync(RoomInformation room)
        {
            var existingRoom = await _roomRepository.GetByIdAsync(room.RoomID);
            if (existingRoom == null)
            {
                throw new InvalidOperationException("Phòng không tồn tại.");
            }

            // Validate room number uniqueness (excluding current room)
            if (await _roomRepository.IsRoomNumberExistsAsync(room.RoomNumber, room.RoomID))
            {
                throw new InvalidOperationException("Số phòng đã tồn tại trong hệ thống.");
            }

            // Validate room type exists
            var roomType = await _roomTypeRepository.GetByIdAsync(room.RoomTypeID);
            if (roomType == null)
            {
                throw new InvalidOperationException("Loại phòng không tồn tại.");
            }

            await _roomRepository.UpdateAsync(room);
            return room;
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                throw new InvalidOperationException("Phòng không tồn tại.");
            }

            await _roomRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RoomInformation>> SearchRoomsAsync(string searchTerm)
        {
            var allRooms = await GetAllRoomsAsync();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return allRooms;
            }

            return allRooms.Where(r => 
                r.RoomNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                r.RoomDescription.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public async Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
        {
            return await _roomRepository.GetAvailableRoomsAsync(checkIn, checkOut);
        }

        public async Task<IEnumerable<RoomInformation>> GetRoomsByTypeAsync(int roomTypeId)
        {
            return await _roomRepository.GetRoomsByTypeAsync(roomTypeId);
        }

        public async Task<bool> IsRoomNumberExistsAsync(string roomNumber, int? excludeId = null)
        {
            return await _roomRepository.IsRoomNumberExistsAsync(roomNumber, excludeId);
        }
    }
}

