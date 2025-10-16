using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class BookingRepository
    {
        private readonly InMemoryDatabase _db = InMemoryDatabase.Instance;

        public List<Booking> GetAll() => _db.Bookings.OrderByDescending(b => b.CreatedDate).ToList();

        public Booking? GetById(int id) => _db.Bookings.FirstOrDefault(b => b.BookingID == id);

        public List<Booking> GetByCustomerId(int customerId) => _db.Bookings.Where(b => b.CustomerID == customerId).OrderByDescending(b => b.CreatedDate).ToList();

        public List<Booking> GetByDateRange(DateTime startDate, DateTime endDate) => _db.Bookings.Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate).OrderByDescending(b => b.TotalAmount).ToList();

        public void Add(Booking booking)
        {
            booking.BookingID = _db.GetNextBookingId();
            booking.CreatedDate = DateTime.Now;
            _db.Bookings.Add(booking);
        }

        public void Update(Booking booking)
        {
            var existing = _db.Bookings.FirstOrDefault(b => b.BookingID == booking.BookingID);
            if (existing != null)
            {
                var index = _db.Bookings.IndexOf(existing);
                _db.Bookings[index] = booking;
            }
        }

        public void Delete(int id)
        {
            var booking = _db.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (booking != null)
            {
                booking.BookingStatus = 3; // Cancelled
            }
        }
    }
}




