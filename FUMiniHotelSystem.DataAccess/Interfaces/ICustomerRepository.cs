using FUMiniHotelSystem.BusinessObjects.Models;

namespace FUMiniHotelSystem.DataAccess.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync();
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
    }
}

