using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.DataAccess.Interfaces;
using FUMiniHotelSystem.DataAccess.Repositories;
using FUMiniHotelSystem.DataAccess.Database;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public static class ServiceFactory
    {
        private static bool _useSqlServer = true; // Set to true for SQL Server, false for In-Memory

        public static IAuthenticationService CreateAuthenticationService()
        {
            if (_useSqlServer)
            {
                var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=FUMiniHotelSystem;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true";
                var customerRepository = new SqlCustomerRepository(connectionString);
                return new AuthenticationService(customerRepository);
            }
            else
            {
                var customerRepository = new CustomerRepository();
                return new AuthenticationService(customerRepository);
            }
        }

        public static ICustomerService CreateCustomerService()
        {
            if (_useSqlServer)
            {
                var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=FUMiniHotelSystem;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true";
                var customerRepository = new SqlCustomerRepository(connectionString);
                return new CustomerService(customerRepository);
            }
            else
            {
                var customerRepository = new CustomerRepository();
                return new CustomerService(customerRepository);
            }
        }

        public static IRoomService CreateRoomService()
        {
            if (_useSqlServer)
            {
                // For now, use In-Memory for rooms (you can create SQL repositories for rooms later)
                var roomRepository = new RoomRepository();
                var roomTypeRepository = new RoomTypeRepository();
                return new RoomService(roomRepository, roomTypeRepository);
            }
            else
            {
                var roomRepository = new RoomRepository();
                var roomTypeRepository = new RoomTypeRepository();
                return new RoomService(roomRepository, roomTypeRepository);
            }
        }

        public static IRoomTypeService CreateRoomTypeService()
        {
            if (_useSqlServer)
            {
                // For now, use In-Memory for room types
                var roomTypeRepository = new RoomTypeRepository();
                return new RoomTypeService(roomTypeRepository);
            }
            else
            {
                var roomTypeRepository = new RoomTypeRepository();
                return new RoomTypeService(roomTypeRepository);
            }
        }

        public static IBookingService CreateBookingService()
        {
            if (_useSqlServer)
            {
                // For now, use In-Memory for bookings
                var bookingRepository = new BookingRepository();
                var customerRepository = new CustomerRepository();
                var roomRepository = new RoomRepository();
                return new BookingService(bookingRepository, roomRepository, customerRepository);
            }
            else
            {
                var bookingRepository = new BookingRepository();
                var customerRepository = new CustomerRepository();
                var roomRepository = new RoomRepository();
                return new BookingService(bookingRepository, roomRepository, customerRepository);
            }
        }
    }
}
