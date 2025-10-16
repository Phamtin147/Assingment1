using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class RoomTypeRepository
    {
        private readonly InMemoryDatabase _db = InMemoryDatabase.Instance;

        public List<RoomType> GetAll() => _db.RoomTypes.ToList();

        public RoomType? GetById(int id) => _db.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == id);
    }
}




