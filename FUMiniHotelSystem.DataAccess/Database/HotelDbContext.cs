using Microsoft.Data.SqlClient;
using System.Data;
using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.DataAccess.Database
{
    public class HotelDbContext
    {
        private readonly string _connectionString;

        public HotelDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = await GetConnectionAsync();
            
            // Create tables if not exist
            await CreateTablesAsync(connection);
            
            // Insert sample data if not exist
            await InsertSampleDataAsync(connection);
        }

        private async Task CreateTablesAsync(IDbConnection connection)
        {
            var createTablesScript = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RoomTypes' AND xtype='U')
                CREATE TABLE RoomTypes (
                    RoomTypeID int IDENTITY(1,1) PRIMARY KEY,
                    RoomTypeName nvarchar(50) NOT NULL,
                    TypeDescription nvarchar(250),
                    TypeNote nvarchar(250)
                );

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Customers' AND xtype='U')
                CREATE TABLE Customers (
                    CustomerID int IDENTITY(1,1) PRIMARY KEY,
                    CustomerFullName nvarchar(50) NOT NULL,
                    Telephone nvarchar(12) NOT NULL,
                    EmailAddress nvarchar(50) NOT NULL UNIQUE,
                    CustomerBirthday date NOT NULL,
                    CustomerStatus int NOT NULL DEFAULT 1,
                    Password nvarchar(50) NOT NULL
                );

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RoomInformations' AND xtype='U')
                CREATE TABLE RoomInformations (
                    RoomID int IDENTITY(1,1) PRIMARY KEY,
                    RoomNumber nvarchar(50) NOT NULL UNIQUE,
                    RoomDescription nvarchar(220),
                    RoomMaxCapacity int NOT NULL,
                    RoomStatus int NOT NULL DEFAULT 1,
                    RoomPricePerDate decimal(18,2) NOT NULL,
                    RoomTypeID int NOT NULL,
                    FOREIGN KEY (RoomTypeID) REFERENCES RoomTypes(RoomTypeID)
                );

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Bookings' AND xtype='U')
                CREATE TABLE Bookings (
                    BookingID int IDENTITY(1,1) PRIMARY KEY,
                    CustomerID int NOT NULL,
                    RoomID int NOT NULL,
                    BookingDate datetime2 NOT NULL,
                    CheckInDate datetime2 NOT NULL,
                    CheckOutDate datetime2 NOT NULL,
                    TotalAmount decimal(18,2) NOT NULL,
                    BookingStatus int NOT NULL DEFAULT 1,
                    CreatedDate datetime2 NOT NULL DEFAULT GETDATE(),
                    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
                    FOREIGN KEY (RoomID) REFERENCES RoomInformations(RoomID)
                );
            ";

            using var command = connection.CreateCommand();
            command.CommandText = createTablesScript;
            command.ExecuteNonQuery();
        }

        private async Task InsertSampleDataAsync(IDbConnection connection)
        {
            // Check if data already exists
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT COUNT(*) FROM Customers WHERE EmailAddress = 'admin@FUMiniHotelSystem.com'";
            var count = checkCommand.ExecuteScalar();
            
            if (Convert.ToInt32(count) > 0) return; // Data already exists

            var insertDataScript = @"
                -- Insert Room Types
                INSERT INTO RoomTypes (RoomTypeName, TypeDescription, TypeNote) VALUES
                ('Standard', 'Standard room with basic amenities', 'Single or double occupancy'),
                ('Deluxe', 'Deluxe room with premium amenities', 'Enhanced comfort and services'),
                ('Suite', 'Luxury suite with full amenities', 'Premium accommodation'),
                ('Family', 'Family room for multiple guests', 'Suitable for families with children');

                -- Insert Admin Customer
                INSERT INTO Customers (CustomerFullName, Telephone, EmailAddress, CustomerBirthday, CustomerStatus, Password) VALUES
                ('System Administrator', '0123456789', 'admin@FUMiniHotelSystem.com', '1990-01-01', 1, '@@abc123@@');

                -- Insert Sample Customers
                INSERT INTO Customers (CustomerFullName, Telephone, EmailAddress, CustomerBirthday, CustomerStatus, Password) VALUES
                ('Nguyen Van A', '0987654321', 'nguyenvana@email.com', '1985-05-15', 1, 'password123'),
                ('Tran Thi B', '0912345678', 'tranthib@email.com', '1992-08-20', 1, 'password456');

                -- Insert Room Informations
                INSERT INTO RoomInformations (RoomNumber, RoomDescription, RoomMaxCapacity, RoomStatus, RoomPricePerDate, RoomTypeID) VALUES
                ('101', 'Standard room with city view', 2, 1, 100000, 1),
                ('102', 'Standard room with garden view', 2, 1, 120000, 1),
                ('201', 'Deluxe room with balcony', 3, 1, 200000, 2),
                ('202', 'Deluxe room with ocean view', 3, 1, 250000, 2),
                ('301', 'Luxury suite with living area', 4, 1, 400000, 3),
                ('401', 'Family room with connecting rooms', 6, 1, 300000, 4);
            ";

            using var command = connection.CreateCommand();
            command.CommandText = insertDataScript;
            command.ExecuteNonQuery();
        }
    }
}
