using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;

namespace StockMax.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task<Product> Get(Guid id);

        public Task<List<Product>> GetAll(QueryParameters queryParameters);

        public Task<Product> Create(Product product);

        public Task<Product> Update(Product product);

        public Task Delete(Guid id);

        public Task<int> Count();

        public Task<List<Color>> GetColors();
    }
}