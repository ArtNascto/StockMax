using Microsoft.EntityFrameworkCore;
using StockMax.Domain.Interfaces.Repositories;
using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;
using StockMax.Domain.Models.View.Helpers;
using System.Linq.Dynamic.Core;

namespace StockMax.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StockMaxDbContext _context;

        public UserRepository(StockMaxDbContext context)
        {
            _context = context;
        }

        public async Task Delete(string id)
        {
            try
            {
                User user = await Get(id);
                if (user != null)
                    await _context.Users.Where(u => u.Id == user.Id).ExecuteDeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> Get(string id)
        {
            try
            {
                return await _context.Users.Where(u => u.Id == id && !u.DeletionTime.HasValue).FirstOrDefaultAsync();
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
                return await _context.Users.CountAsync(u => !u.DeletionTime.HasValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<User>> GetAll(QueryParameters queryParameters)
        {
            try
            {
                IQueryable<User> _allUsers = _context.Users.OrderBy(queryParameters.OrderBy, queryParameters.IsDescending()).Where(x => !x.DeletionTime.HasValue);
                if (queryParameters.HasQuery())
                {
                    _allUsers = _allUsers.Where(x => x.Email.ToString()
                    .Contains(queryParameters.Query.ToLower()) || x.Name
                    .Contains(queryParameters.Query.ToLower()));
                }
                return await _allUsers.Skip(queryParameters.PageCount * (queryParameters.Page - 1)).Take(queryParameters.PageCount).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}