using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.DataAccess.Repositories;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepo = new();

        public List<Customer> GetAllCustomers() => _customerRepo.GetAll();

        public Customer? GetCustomerById(int id) => _customerRepo.GetById(id);

        public void AddCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerFullName))
                throw new Exception("Tên khách hàng không được để trống");

            if (string.IsNullOrWhiteSpace(customer.EmailAddress))
                throw new Exception("Email không được để trống");

            if (_customerRepo.EmailExists(customer.EmailAddress))
                throw new Exception("Email đã tồn tại");

            customer.CustomerStatus = 1;
            _customerRepo.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerFullName))
                throw new Exception("Tên khách hàng không được để trống");

            if (string.IsNullOrWhiteSpace(customer.EmailAddress))
                throw new Exception("Email không được để trống");

            if (_customerRepo.EmailExists(customer.EmailAddress, customer.CustomerID))
                throw new Exception("Email đã tồn tại");

            _customerRepo.Update(customer);
        }

        public void DeleteCustomer(int id) => _customerRepo.Delete(id);

        public List<Customer> SearchCustomers(string keyword)
        {
            var customers = _customerRepo.GetAll();
            if (string.IsNullOrWhiteSpace(keyword))
                return customers;

            keyword = keyword.ToLower();
            return customers.Where(c =>
                c.CustomerFullName.ToLower().Contains(keyword) ||
                c.EmailAddress.ToLower().Contains(keyword) ||
                c.Telephone.Contains(keyword)
            ).ToList();
        }
    }
}



