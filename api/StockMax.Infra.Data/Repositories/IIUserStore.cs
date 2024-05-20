using Microsoft.AspNetCore.Identity;
using StockMax.Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMax.Infra.Data.Repositories
{
    public interface IIUserStore : IUserStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>
    {
    }

    public class UserStore : IIUserStore
    {
        private StockMaxDbContext _context;

        public UserStore(StockMaxDbContext context, IdentityErrorDescriber describer = null)
        {
            _context = context;
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            return Task<IdentityResult>.Run(() =>
            {
                IdentityResult result = IdentityResult.Failed();
                var createResult = _context.Users.Add(user);
                _context.SaveChanges();
                if (createResult != null)
                {
                    result = IdentityResult.Success;
                }

                return result;
            });
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            return Task<IdentityResult>.Run(() =>
            {
                IdentityResult result = IdentityResult.Failed();
                user.DeletionTime = DateTime.Now;
                var deleteResult = _context.Users.Update(user);
                _context.SaveChanges();
                if (deleteResult != null)
                {
                    result = IdentityResult.Success;
                }

                return result;
            });
        }

        public void Dispose()
        {
        }

        public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task<User>.Run(() =>
            {
                return _context.Users?
                .FirstOrDefault(u => u.Id == userId);
            });
        }

        public Task<User?> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            return Task<User>.Run(() =>
            {
                return _context.Users?
                .FirstOrDefault(u => u.Name == name);
            });
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.NormalizedUserName;
            });
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.Id;
            });
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.UserName;
            });
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.NormalizedUserName = normalizedName;
            });
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.UserName = userName;
            });
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return Task<IdentityResult>.Run(() =>
            {
                IdentityResult result = IdentityResult.Failed();
                var updateResult = _context.Users.Update(user);
                _context.SaveChanges();

                if (updateResult != null)
                {
                    result = IdentityResult.Success;
                }

                return result;
            });
        }

        public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<bool>(!String.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.Email = email;
            });
        }

        public Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<bool>(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.EmailConfirmed = confirmed;
            });
        }

        public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Task<User>.Run(() =>
            {
                return _context.Users?
                  .FirstOrDefault(u => u.Email == email);
            });
        }

        public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.NormalizedEmail = normalizedEmail;
            });
        }
    }
}