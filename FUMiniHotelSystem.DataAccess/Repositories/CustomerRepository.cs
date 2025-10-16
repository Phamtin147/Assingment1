using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Database;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public class CustomerRepository
    {
        private readonly InMemoryDatabase _db = InMemoryDatabase.Instance;

        public List<Customer> GetAll() => _db.Customers.Where(c => c.CustomerStatus == 1).ToList();

        public Customer? GetById(int id) => _db.Customers.FirstOrDefault(c => c.CustomerID == id);

        public Customer? GetByEmail(string email) => _db.Customers.FirstOrDefault(c => c.EmailAddress == email);

        public void Add(Customer customer)
        {
            customer.CustomerID = _db.GetNextCustomerId();
            _db.Customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            var existing = _db.Customers.FirstOrDefault(c => c.CustomerID == customer.CustomerID);
            if (existing != null)
            {
                var index = _db.Customers.IndexOf(existing);
                _db.Customers[index] = customer;
            }
        }

        public void Delete(int id)
        {
            var customer = _db.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer != null)
            {
                customer.CustomerStatus = 2; // Soft delete
            }
        }

        public bool EmailExists(string email, int? excludeId = null)
        {
            return _db.Customers.Any(c => c.EmailAddress == email && (!excludeId.HasValue || c.CustomerID != excludeId.Value));
        }
    }
}




