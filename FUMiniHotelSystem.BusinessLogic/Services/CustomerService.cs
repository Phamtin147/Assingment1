using FUMiniHotelSystem.BusinessObjects.Models;
using FUMiniHotelSystem.BusinessLogic.Interfaces;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.BusinessLogic.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetActiveCustomersAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _customerRepository.GetByEmailAsync(email);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            // Validate email uniqueness
            if (await _customerRepository.IsEmailExistsAsync(customer.EmailAddress))
            {
                throw new InvalidOperationException("Email đã tồn tại trong hệ thống.");
            }

            customer.CustomerStatus = 1; // Active
            return await _customerRepository.AddAsync(customer);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customer.CustomerID);
            if (existingCustomer == null)
            {
                throw new InvalidOperationException("Khách hàng không tồn tại.");
            }

            // Validate email uniqueness (excluding current customer)
            if (await _customerRepository.IsEmailExistsAsync(customer.EmailAddress, customer.CustomerID))
            {
                throw new InvalidOperationException("Email đã tồn tại trong hệ thống.");
            }

            await _customerRepository.UpdateAsync(customer);
            return customer;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                throw new InvalidOperationException("Khách hàng không tồn tại.");
            }

            await _customerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            var allCustomers = await _customerRepository.GetActiveCustomersAsync();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return allCustomers;
            }

            return allCustomers.Where(c => 
                c.CustomerFullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.EmailAddress.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Telephone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            return await _customerRepository.IsEmailExistsAsync(email, excludeId);
        }
    }
}

