using System.Linq.Expressions;
using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        protected override IEnumerable<Customer> GetAll()
        {
            return _database.Customers.ToList();
        }

        protected override Customer? GetById(int id)
        {
            return _database.Customers.FirstOrDefault(c => c.CustomerID == id);
        }

        protected override IEnumerable<Customer> Find(Expression<Func<Customer, bool>> predicate)
        {
            return _database.Customers.AsQueryable().Where(predicate).ToList();
        }

        protected override Customer Add(Customer entity)
        {
            entity.CustomerID = _database.GetNextCustomerId();
            _database.Customers.Add(entity);
            return entity;
        }

        protected override void Update(Customer entity)
        {
            var existingCustomer = _database.Customers.FirstOrDefault(c => c.CustomerID == entity.CustomerID);
            if (existingCustomer != null)
            {
                var index = _database.Customers.IndexOf(existingCustomer);
                _database.Customers[index] = entity;
            }
        }

        protected override void Delete(int id)
        {
            var customer = _database.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer != null)
            {
                customer.CustomerStatus = 2; // Mark as deleted
            }
        }

        protected override bool Exists(int id)
        {
            return _database.Customers.Any(c => c.CustomerID == id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await Task.FromResult(_database.Customers.FirstOrDefault(c => c.EmailAddress == email));
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            return await Task.FromResult(_database.Customers.Where(c => c.CustomerStatus == 1).ToList());
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _database.Customers.Where(c => c.EmailAddress == email);
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.CustomerID != excludeId.Value);
            }
            return await Task.FromResult(query.Any());
        }
    }
}

