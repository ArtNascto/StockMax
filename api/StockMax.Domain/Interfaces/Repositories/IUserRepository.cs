using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;

namespace StockMax.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<User> Get(string id);

        public Task<List<User>> GetAll(QueryParameters queryParameters);

        public Task Delete(string id);

        public Task<int> Count();
    }
}