using System.Linq.Expressions;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        protected override IEnumerable<Booking> GetAll()
        {
            return _database.Bookings.ToList();
        }

        protected override Booking? GetById(int id)
        {
            return _database.Bookings.FirstOrDefault(b => b.BookingID == id);
        }

        protected override IEnumerable<Booking> Find(Expression<Func<Booking, bool>> predicate)
        {
            return _database.Bookings.AsQueryable().Where(predicate).ToList();
        }

        protected override Booking Add(Booking entity)
        {
            entity.BookingID = _database.GetNextBookingId();
            _database.Bookings.Add(entity);
            return entity;
        }

        protected override void Update(Booking entity)
        {
            var existingBooking = _database.Bookings.FirstOrDefault(b => b.BookingID == entity.BookingID);
            if (existingBooking != null)
            {
                var index = _database.Bookings.IndexOf(existingBooking);
                _database.Bookings[index] = entity;
            }
        }

        protected override void Delete(int id)
        {
            var booking = _database.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (booking != null)
            {
                booking.BookingStatus = 3; // Mark as cancelled
            }
        }

        protected override bool Exists(int id)
        {
            return _database.Bookings.Any(b => b.BookingID == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(int customerId)
        {
            return await Task.FromResult(_database.Bookings.Where(b => b.CustomerID == customerId).OrderByDescending(b => b.CreatedDate).ToList());
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_database.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .OrderByDescending(b => b.BookingDate)
                .ToList());
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRoomAsync(int roomId)
        {
            return await Task.FromResult(_database.Bookings.Where(b => b.RoomID == roomId).OrderByDescending(b => b.CreatedDate).ToList());
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
        {
            return await Task.FromResult(_database.Bookings.Where(b => b.BookingStatus == 1 || b.BookingStatus == 2).ToList());
        }
    }
}

