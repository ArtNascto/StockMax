using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;

namespace StockMax.Domain.Interfaces.Services
{
    public interface IUserService
    {
        public Task<User> Get(string id);

        public Task<List<User>> GetAll(QueryParameters queryParameters);

        public Task<User> Create(User user, string password);

        public Task Delete(string id);

        public Task<User> Update(User user, string? password);

        public Task<User> Update(UserDto user, string? password);

        public Task<User?> FindByEmailAsync(string email);

        public Task<bool> CheckPasswordAsync(User user, string password);

        public Task<int> Count();
    }
}