using Microsoft.Data.SqlClient;
using System.Data;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class SqlCustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public SqlCustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = new List<Customer>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Customers WHERE CustomerStatus = 1";
            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                customers.Add(MapCustomer(reader));
            }

            return customers;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Customers WHERE CustomerID = @id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapCustomer(reader);
            }

            return null;
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Customers WHERE EmailAddress = @email";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapCustomer(reader);
            }

            return null;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Customers (CustomerFullName, Telephone, EmailAddress, CustomerBirthday, CustomerStatus, Password)
                VALUES (@fullName, @telephone, @email, @birthday, @status, @password);
                SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@fullName", entity.CustomerFullName);
            command.Parameters.AddWithValue("@telephone", entity.Telephone);
            command.Parameters.AddWithValue("@email", entity.EmailAddress);
            command.Parameters.AddWithValue("@birthday", entity.CustomerBirthday);
            command.Parameters.AddWithValue("@status", entity.CustomerStatus);
            command.Parameters.AddWithValue("@password", entity.Password);

            var id = await command.ExecuteScalarAsync();
            entity.CustomerID = Convert.ToInt32(id);
            return entity;
        }

        public async Task UpdateAsync(Customer entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE Customers 
                SET CustomerFullName = @fullName, 
                    Telephone = @telephone, 
                    EmailAddress = @email, 
                    CustomerBirthday = @birthday, 
                    CustomerStatus = @status, 
                    Password = @password
                WHERE CustomerID = @id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", entity.CustomerID);
            command.Parameters.AddWithValue("@fullName", entity.CustomerFullName);
            command.Parameters.AddWithValue("@telephone", entity.Telephone);
            command.Parameters.AddWithValue("@email", entity.EmailAddress);
            command.Parameters.AddWithValue("@birthday", entity.CustomerBirthday);
            command.Parameters.AddWithValue("@status", entity.CustomerStatus);
            command.Parameters.AddWithValue("@password", entity.Password);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE Customers SET CustomerStatus = 2 WHERE CustomerID = @id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(*) FROM Customers WHERE CustomerID = @id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            var count = await command.ExecuteScalarAsync();
            return Convert.ToInt32(count) > 0;
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            return await GetAllAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(*) FROM Customers WHERE EmailAddress = @email";
            if (excludeId.HasValue)
            {
                query += " AND CustomerID != @excludeId";
            }

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            if (excludeId.HasValue)
            {
                command.Parameters.AddWithValue("@excludeId", excludeId.Value);
            }

            var count = await command.ExecuteScalarAsync();
            return Convert.ToInt32(count) > 0;
        }

        public async Task<IEnumerable<Customer>> FindAsync(System.Linq.Expressions.Expression<Func<Customer, bool>> predicate)
        {
            // For simplicity, we'll get all customers and filter in memory
            // In a real application, you'd translate the expression to SQL
            var allCustomers = await GetAllAsync();
            return allCustomers.Where(predicate.Compile());
        }

        private Customer MapCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerID = reader.GetInt32("CustomerID"),
                CustomerFullName = reader.GetString("CustomerFullName"),
                Telephone = reader.GetString("Telephone"),
                EmailAddress = reader.GetString("EmailAddress"),
                CustomerBirthday = reader.GetDateTime("CustomerBirthday"),
                CustomerStatus = reader.GetInt32("CustomerStatus"),
                Password = reader.GetString("Password")
            };
        }
    }
}

