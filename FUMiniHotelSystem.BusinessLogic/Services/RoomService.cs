using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class RoomService
    {
        private readonly RoomRepository _roomRepo = new();
        private readonly RoomTypeRepository _roomTypeRepo = new();

        public List<RoomInformation> GetAllRooms()
        {
            var rooms = _roomRepo.GetAll();
            foreach (var room in rooms)
            {
                room.RoomType = _roomTypeRepo.GetById(room.RoomTypeID);
            }
            return rooms;
        }

        public List<RoomType> GetAllRoomTypes() => _roomTypeRepo.GetAll();

        public RoomInformation? GetRoomById(int id) => _roomRepo.GetById(id);

        public void AddRoom(RoomInformation room)
        {
            if (string.IsNullOrWhiteSpace(room.RoomNumber))
                throw new Exception("Số phòng không được để trống");

            if (_roomRepo.RoomNumberExists(room.RoomNumber))
                throw new Exception("Số phòng đã tồn tại");

            room.RoomStatus = 1;
            _roomRepo.Add(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            if (string.IsNullOrWhiteSpace(room.RoomNumber))
                throw new Exception("Số phòng không được để trống");

            if (_roomRepo.RoomNumberExists(room.RoomNumber, room.RoomID))
                throw new Exception("Số phòng đã tồn tại");

            _roomRepo.Update(room);
        }

        public void DeleteRoom(int id) => _roomRepo.Delete(id);

        public List<RoomInformation> SearchRooms(string keyword)
        {
            var rooms = GetAllRooms();
            if (string.IsNullOrWhiteSpace(keyword))
                return rooms;

            keyword = keyword.ToLower();
            return rooms.Where(r =>
                r.RoomNumber.ToLower().Contains(keyword) ||
                (r.RoomDescription != null && r.RoomDescription.ToLower().Contains(keyword)) ||
                (r.RoomType != null && r.RoomType.RoomTypeName.ToLower().Contains(keyword))
            ).ToList();
        }
    }
}




