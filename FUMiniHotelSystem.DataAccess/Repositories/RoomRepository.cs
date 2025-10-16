using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class RoomRepository
    {
        private readonly InMemoryDatabase _db = InMemoryDatabase.Instance;

        public List<RoomInformation> GetAll() => _db.RoomInformations.Where(r => r.RoomStatus == 1).ToList();

        public RoomInformation? GetById(int id) => _db.RoomInformations.FirstOrDefault(r => r.RoomID == id);

        public void Add(RoomInformation room)
        {
            room.RoomID = _db.GetNextRoomId();
            _db.RoomInformations.Add(room);
        }

        public void Update(RoomInformation room)
        {
            var existing = _db.RoomInformations.FirstOrDefault(r => r.RoomID == room.RoomID);
            if (existing != null)
            {
                var index = _db.RoomInformations.IndexOf(existing);
                _db.RoomInformations[index] = room;
            }
        }

        public void Delete(int id)
        {
            var room = _db.RoomInformations.FirstOrDefault(r => r.RoomID == id);
            if (room != null)
            {
                room.RoomStatus = 2; // Soft delete
            }
        }

        public bool RoomNumberExists(string roomNumber, int? excludeId = null)
        {
            return _db.RoomInformations.Any(r => r.RoomNumber == roomNumber && (!excludeId.HasValue || r.RoomID != excludeId.Value));
        }
    }
}



