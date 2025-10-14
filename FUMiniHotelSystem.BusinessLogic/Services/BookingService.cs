using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessObjects.DTOs;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICustomerRepository _customerRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, ICustomerRepository customerRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(booking.CustomerID);
            if (customer == null)
            {
                throw new InvalidOperationException("Khách hàng không tồn tại.");
            }

            // Validate room exists and is available
            var room = await _roomRepository.GetByIdAsync(booking.RoomID);
            if (room == null)
            {
                throw new InvalidOperationException("Phòng không tồn tại.");
            }

            if (room.RoomStatus != 1)
            {
                throw new InvalidOperationException("Phòng không khả dụng.");
            }

            // Check room availability
            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(booking.CheckInDate, booking.CheckOutDate);
            if (!availableRooms.Any(r => r.RoomID == booking.RoomID))
            {
                throw new InvalidOperationException("Phòng không khả dụng trong khoảng thời gian đã chọn.");
            }

            // Validate dates
            if (booking.CheckInDate >= booking.CheckOutDate)
            {
                throw new InvalidOperationException("Ngày check-in phải nhỏ hơn ngày check-out.");
            }

            if (booking.CheckInDate < DateTime.Today)
            {
                throw new InvalidOperationException("Ngày check-in không thể là ngày trong quá khứ.");
            }

            // Calculate total amount
            booking.TotalAmount = await CalculateTotalAmountAsync(booking.RoomID, booking.CheckInDate, booking.CheckOutDate);
            booking.BookingStatus = 1; // Pending
            booking.CreatedDate = DateTime.Now;

            return await _bookingRepository.AddAsync(booking);
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking)
        {
            var existingBooking = await _bookingRepository.GetByIdAsync(booking.BookingID);
            if (existingBooking == null)
            {
                throw new InvalidOperationException("Đặt phòng không tồn tại.");
            }

            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(booking.CustomerID);
            if (customer == null)
            {
                throw new InvalidOperationException("Khách hàng không tồn tại.");
            }

            // Validate room exists
            var room = await _roomRepository.GetByIdAsync(booking.RoomID);
            if (room == null)
            {
                throw new InvalidOperationException("Phòng không tồn tại.");
            }

            // Validate dates
            if (booking.CheckInDate >= booking.CheckOutDate)
            {
                throw new InvalidOperationException("Ngày check-in phải nhỏ hơn ngày check-out.");
            }

            // Recalculate total amount
            booking.TotalAmount = await CalculateTotalAmountAsync(booking.RoomID, booking.CheckInDate, booking.CheckOutDate);

            await _bookingRepository.UpdateAsync(booking);
            return booking;
        }

        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new InvalidOperationException("Đặt phòng không tồn tại.");
            }

            await _bookingRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(int customerId)
        {
            return await _bookingRepository.GetBookingsByCustomerAsync(customerId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(ReportRequest request)
        {
            return await _bookingRepository.GetBookingsByDateRangeAsync(request.StartDate, request.EndDate);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRoomAsync(int roomId)
        {
            return await _bookingRepository.GetBookingsByRoomAsync(roomId);
        }

        public async Task<decimal> CalculateTotalAmountAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new InvalidOperationException("Phòng không tồn tại.");
            }

            var numberOfDays = (checkOut - checkIn).Days;
            return room.RoomPricePerDate * numberOfDays;
        }
    }
}

