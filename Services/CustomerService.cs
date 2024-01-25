using dotnetcore7_webapi_authentication.Data;
using dotnetcore7_webapi_authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore7_webapi_authentication.Services
{
    public interface ICustomerService
    {
        public Task<List<Customer>> GetAll();

    }
    public class CustomerService : ICustomerService
    {
        private readonly Dotnetcore7WebapiAuthenticationDbContext _context;
        public CustomerService(Dotnetcore7WebapiAuthenticationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Customer>> GetAll()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }
    }
}