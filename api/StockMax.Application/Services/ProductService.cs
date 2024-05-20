using StockMax.Domain.Interfaces.Repositories;
using StockMax.Domain.Interfaces.Services;
using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;

namespace StockMax.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Create(Product product)
        {
            try
            {
                return await _repository.Create(product);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Count()
        {
            try
            {
                return await _repository.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                await _repository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Color>> GetColors()
        {
            try
            {
                return await _repository.GetColors();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> Get(Guid id)
        {
            try
            {
                return await _repository.Get(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Product>> GetAll(QueryParameters queryParameters)
        {
            try
            {
                return await _repository.GetAll(queryParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> Update(Product product)
        {
            try
            {
                return await _repository.Update(product);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}