using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.DataAccess.Database
{
    public sealed class InMemoryDatabase
    {
        private static readonly Lazy<InMemoryDatabase> _instance = new(() => new InMemoryDatabase());
        public static InMemoryDatabase Instance => _instance.Value;

        private InMemoryDatabase()
        {
            InitializeData();
        }

        public List<Customer> Customers { get; private set; } = new();
        public List<RoomType> RoomTypes { get; private set; } = new();
        public List<RoomInformation> RoomInformations { get; private set; } = new();
        public List<Booking> Bookings { get; private set; } = new();

        private void InitializeData()
        {
            // Initialize Room Types
            RoomTypes.AddRange(new List<RoomType>
            {
                new RoomType { RoomTypeID = 1, RoomTypeName = "Standard", TypeDescription = "Standard room with basic amenities", TypeNote = "Single or double occupancy" },
                new RoomType { RoomTypeID = 2, RoomTypeName = "Deluxe", TypeDescription = "Deluxe room with premium amenities", TypeNote = "Enhanced comfort and services" },
                new RoomType { RoomTypeID = 3, RoomTypeName = "Suite", TypeDescription = "Luxury suite with full amenities", TypeNote = "Premium accommodation" },
                new RoomType { RoomTypeID = 4, RoomTypeName = "Family", TypeDescription = "Family room for multiple guests", TypeNote = "Suitable for families with children" }
            });

            // Initialize Room Informations
            RoomInformations.AddRange(new List<RoomInformation>
            {
                new RoomInformation { RoomID = 1, RoomNumber = "101", RoomDescription = "Standard room with city view", RoomMaxCapacity = 2, RoomStatus = 1, RoomPricePerDate = 100000, RoomTypeID = 1 },
                new RoomInformation { RoomID = 2, RoomNumber = "102", RoomDescription = "Standard room with garden view", RoomMaxCapacity = 2, RoomStatus = 1, RoomPricePerDate = 120000, RoomTypeID = 1 },
                new RoomInformation { RoomID = 3, RoomNumber = "201", RoomDescription = "Deluxe room with balcony", RoomMaxCapacity = 3, RoomStatus = 1, RoomPricePerDate = 200000, RoomTypeID = 2 },
                new RoomInformation { RoomID = 4, RoomNumber = "202", RoomDescription = "Deluxe room with ocean view", RoomMaxCapacity = 3, RoomStatus = 1, RoomPricePerDate = 250000, RoomTypeID = 2 },
                new RoomInformation { RoomID = 5, RoomNumber = "301", RoomDescription = "Luxury suite with living area", RoomMaxCapacity = 4, RoomStatus = 1, RoomPricePerDate = 400000, RoomTypeID = 3 },
                new RoomInformation { RoomID = 6, RoomNumber = "401", RoomDescription = "Family room with connecting rooms", RoomMaxCapacity = 6, RoomStatus = 1, RoomPricePerDate = 300000, RoomTypeID = 4 }
            });

            // Initialize Admin Customer
            Customers.Add(new Customer
            {
                CustomerID = 1,
                CustomerFullName = "System Administrator",
                Telephone = "0123456789",
                EmailAddress = "admin@FUMiniHotelSystem.com",
                CustomerBirthday = new DateTime(1990, 1, 1),
                CustomerStatus = 1,
                Password = "@@abc123@@"
            });

            // Initialize Sample Customers
            Customers.AddRange(new List<Customer>
            {
                new Customer
                {
                    CustomerID = 2,
                    CustomerFullName = "Nguyen Van A",
                    Telephone = "0987654321",
                    EmailAddress = "nguyenvana@email.com",
                    CustomerBirthday = new DateTime(1985, 5, 15),
                    CustomerStatus = 1,
                    Password = "password123"
                },
                new Customer
                {
                    CustomerID = 3,
                    CustomerFullName = "Tran Thi B",
                    Telephone = "0912345678",
                    EmailAddress = "tranthib@email.com",
                    CustomerBirthday = new DateTime(1992, 8, 20),
                    CustomerStatus = 1,
                    Password = "password456"
                }
            });
        }

        public int GetNextCustomerId() => Customers.Count > 0 ? Customers.Max(c => c.CustomerID) + 1 : 1;
        public int GetNextRoomTypeId() => RoomTypes.Count > 0 ? RoomTypes.Max(rt => rt.RoomTypeID) + 1 : 1;
        public int GetNextRoomId() => RoomInformations.Count > 0 ? RoomInformations.Max(r => r.RoomID) + 1 : 1;
        public int GetNextBookingId() => Bookings.Count > 0 ? Bookings.Max(b => b.BookingID) + 1 : 1;
    }
}

