using Microsoft.EntityFrameworkCore;
using StockMax.Domain.Interfaces.Repositories;
using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;
using StockMax.Domain.Models.View.Helpers;
using System.Linq.Dynamic.Core;

namespace StockMax.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StockMaxDbContext _context;

        public ProductRepository(StockMaxDbContext context)
        {
            _context = context;
        }

        public async Task<List<Color>> GetColors()
        {
            try
            {
                return await _context.Colors.Where(c => !c.DeletionTime.HasValue).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> Create(Product product)
        {
            try
            {
                var colorsText = product.Colors;
                var colors = product.Colors.Split(",");

                foreach (var color in colors)
                {
                    if (!_context.Colors.Any(c => c.Value == color))
                    {
                        var c = new Color()
                        {
                            Value = color,
                            CreationTime = DateTime.Now,
                            LastUpdate = DateTime.Now
                        };
                        _context.Colors.Add(c);
                        await _context.SaveChangesAsync();
                    }
                }

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product;
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
                var product = await Get(id);
                if (product != null)
                    await _context.Products.Where(u => u.Id == product.Id).ExecuteDeleteAsync();
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
                return await _context.Products.Where(u => u.Id == id && !u.DeletionTime.HasValue).FirstOrDefaultAsync();
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
                return await _context.Products.CountAsync(x => !x.DeletionTime.HasValue && !string.IsNullOrEmpty(x.Name));
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
                IQueryable<Product> _allProducts = _context.Products.OrderBy(queryParameters.OrderBy, queryParameters.IsDescending()).Where(x => !x.DeletionTime.HasValue && !string.IsNullOrEmpty(x.Name));
                if (queryParameters.HasQuery())
                {
                    _allProducts = _allProducts.Where(x => x.Code.ToString().Contains(queryParameters.Query.ToLower()) || x.Name.Contains(queryParameters.Query.ToLower()));
                }
                return await _allProducts.Skip(queryParameters.PageCount * (queryParameters.Page - 1)).Take(queryParameters.PageCount).ToListAsync();
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
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}