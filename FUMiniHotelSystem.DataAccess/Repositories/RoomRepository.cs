using System.Linq.Expressions;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class RoomRepository : BaseRepository<RoomInformation>, IRoomRepository
    {
        protected override IEnumerable<RoomInformation> GetAll()
        {
            return _database.RoomInformations.ToList();
        }

        protected override RoomInformation? GetById(int id)
        {
            return _database.RoomInformations.FirstOrDefault(r => r.RoomID == id);
        }

        protected override IEnumerable<RoomInformation> Find(Expression<Func<RoomInformation, bool>> predicate)
        {
            return _database.RoomInformations.AsQueryable().Where(predicate).ToList();
        }

        protected override RoomInformation Add(RoomInformation entity)
        {
            entity.RoomID = _database.GetNextRoomId();
            _database.RoomInformations.Add(entity);
            return entity;
        }

        protected override void Update(RoomInformation entity)
        {
            var existingRoom = _database.RoomInformations.FirstOrDefault(r => r.RoomID == entity.RoomID);
            if (existingRoom != null)
            {
                var index = _database.RoomInformations.IndexOf(existingRoom);
                _database.RoomInformations[index] = entity;
            }
        }

        protected override void Delete(int id)
        {
            var room = _database.RoomInformations.FirstOrDefault(r => r.RoomID == id);
            if (room != null)
            {
                room.RoomStatus = 2; // Mark as deleted
            }
        }

        protected override bool Exists(int id)
        {
            return _database.RoomInformations.Any(r => r.RoomID == id);
        }

        public async Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomIds = _database.Bookings
                .Where(b => b.BookingStatus == 1 || b.BookingStatus == 2) // Pending or Confirmed
                .Where(b => (b.CheckInDate < checkOut && b.CheckOutDate > checkIn))
                .Select(b => b.RoomID)
                .Distinct();

            var availableRooms = _database.RoomInformations
                .Where(r => r.RoomStatus == 1 && !bookedRoomIds.Contains(r.RoomID))
                .ToList();

            return await Task.FromResult(availableRooms);
        }

        public async Task<IEnumerable<RoomInformation>> GetRoomsByTypeAsync(int roomTypeId)
        {
            return await Task.FromResult(_database.RoomInformations.Where(r => r.RoomTypeID == roomTypeId && r.RoomStatus == 1).ToList());
        }

        public async Task<bool> IsRoomNumberExistsAsync(string roomNumber, int? excludeId = null)
        {
            var query = _database.RoomInformations.Where(r => r.RoomNumber == roomNumber);
            if (excludeId.HasValue)
            {
                query = query.Where(r => r.RoomID != excludeId.Value);
            }
            return await Task.FromResult(query.Any());
        }
    }
}

