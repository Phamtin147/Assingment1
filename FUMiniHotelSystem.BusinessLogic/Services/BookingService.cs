using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepo = new();
        private readonly CustomerRepository _customerRepo = new();
        private readonly RoomRepository _roomRepo = new();

        public List<Booking> GetAllBookings()
        {
            var bookings = _bookingRepo.GetAll();
            foreach (var booking in bookings)
            {
                booking.Customer = _customerRepo.GetById(booking.CustomerID);
                booking.Room = _roomRepo.GetById(booking.RoomID);
            }
            return bookings;
        }

        public List<Booking> GetBookingsByCustomer(int customerId)
        {
            var bookings = _bookingRepo.GetByCustomerId(customerId);
            foreach (var booking in bookings)
            {
                booking.Room = _roomRepo.GetById(booking.RoomID);
            }
            return bookings;
        }

        public List<Booking> GetBookingsByDateRange(DateTime startDate, DateTime endDate)
        {
            var bookings = _bookingRepo.GetByDateRange(startDate, endDate);
            foreach (var booking in bookings)
            {
                booking.Customer = _customerRepo.GetById(booking.CustomerID);
                booking.Room = _roomRepo.GetById(booking.RoomID);
            }
            return bookings;
        }

        public void AddBooking(Booking booking)
        {
            if (booking.CheckInDate >= booking.CheckOutDate)
                throw new Exception("Ngày check-out phải sau ngày check-in");

            booking.BookingDate = DateTime.Now;
            booking.BookingStatus = 1; // Pending
            _bookingRepo.Add(booking);
        }

        public void UpdateBooking(Booking booking) => _bookingRepo.Update(booking);

        public void DeleteBooking(int id) => _bookingRepo.Delete(id);
    }
}




