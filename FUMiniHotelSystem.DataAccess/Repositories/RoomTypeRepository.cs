using System.Linq.Expressions;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class RoomTypeRepository : BaseRepository<RoomType>, IRoomTypeRepository
    {
        protected override IEnumerable<RoomType> GetAll()
        {
            return _database.RoomTypes.ToList();
        }

        protected override RoomType? GetById(int id)
        {
            return _database.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == id);
        }

        protected override IEnumerable<RoomType> Find(Expression<Func<RoomType, bool>> predicate)
        {
            return _database.RoomTypes.AsQueryable().Where(predicate).ToList();
        }

        protected override RoomType Add(RoomType entity)
        {
            entity.RoomTypeID = _database.GetNextRoomTypeId();
            _database.RoomTypes.Add(entity);
            return entity;
        }

        protected override void Update(RoomType entity)
        {
            var existingRoomType = _database.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == entity.RoomTypeID);
            if (existingRoomType != null)
            {
                var index = _database.RoomTypes.IndexOf(existingRoomType);
                _database.RoomTypes[index] = entity;
            }
        }

        protected override void Delete(int id)
        {
            var roomType = _database.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == id);
            if (roomType != null)
            {
                _database.RoomTypes.Remove(roomType);
            }
        }

        protected override bool Exists(int id)
        {
            return _database.RoomTypes.Any(rt => rt.RoomTypeID == id);
        }

        public async Task<IEnumerable<RoomType>> GetActiveRoomTypesAsync()
        {
            return await Task.FromResult(_database.RoomTypes.ToList()); // All room types are considered active
        }

        public async Task<bool> IsRoomTypeNameExistsAsync(string roomTypeName, int? excludeId = null)
        {
            var query = _database.RoomTypes.Where(rt => rt.RoomTypeName == roomTypeName);
            if (excludeId.HasValue)
            {
                query = query.Where(rt => rt.RoomTypeID != excludeId.Value);
            }
            return await Task.FromResult(query.Any());
        }
    }
}

