using Microsoft.AspNetCore.Identity;
using StockMax.Domain.Interfaces.Repositories;
using StockMax.Domain.Interfaces.Services;
using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;

namespace StockMax.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, IUserRepository repository)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            try
            {
                var valid = _userManager.CheckPasswordAsync(user, password);

                return valid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> Create(User user, string password)
        {
            try
            {
                var exists = await _userManager.FindByEmailAsync(user.Email);
                if (exists != null && !exists.DeletionTime.HasValue)
                {
                    throw new Exception("User already exists");
                };
                var newUser = new User()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Uin = user.Uin,
                    UserName = user.Email,
                    EmailConfirmed = true,
                    CreationTime = DateTime.Now,
                    LastAccess = user.LastAccess,
                    Id = user.Id,
                    ImagePath = user.ImagePath,
                    DeletionTime = user.DeletionTime,
                    Address = user.Address,
                    AddressNumber = user.AddressNumber,
                    BirthDate = user.BirthDate,
                    City = user.City,
                    Complement = user.Complement,
                    Country = user.Country,
                    Gender = user.Gender,
                    Informations = user.Informations,
                    Job = user.Job,
                    LastUpdate = user.LastUpdate,
                    MaritalStatus = user.MaritalStatus,
                    Neighborhood = user.Neighborhood,
                    Phone = user.Phone,
                    State = user.State,
                    ZipCode = user.ZipCode,
                };
                var result = await _userManager.CreateAsync(newUser, password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);

                    throw new Exception(result.Errors.ToString());
                }
                return newUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(string id)
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

        public async Task<User?> FindByEmailAsync(string email)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(email);

                return user;
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
                return await _repository.Get(id);
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
                return await _repository.GetAll(queryParameters);
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

        public async Task<User> Update(User user, string? password)
        {
            try
            {
                var newUser = await _repository.Get(user.Id) ?? throw new Exception("User not found");
                if (newUser != null)
                {
                    newUser.Name = user.Name;
                    newUser.EmailConfirmed = false;
                    newUser.ImagePath = user.ImagePath;
                    newUser.LastUpdate = DateTime.Now;
                    newUser.MaritalStatus = user.MaritalStatus;
                    newUser.Neighborhood = user.Neighborhood;
                    newUser.Phone = user.Phone;
                    newUser.State = user.State;
                    newUser.ZipCode = user.ZipCode;
                    newUser.ImagePath = user.ImagePath;
                    newUser.Address = user.Address;
                    newUser.AddressNumber = user.AddressNumber;
                    newUser.BirthDate = user.BirthDate;
                    newUser.City = user.City;
                    newUser.Complement = user.Complement;
                    newUser.Country = user.Country;
                    newUser.Gender = user.Gender;
                    newUser.Informations = user.Informations;
                    newUser.Job = user.Job;
                    newUser.Name = user.Name;
                    var result = await _userManager.UpdateAsync(newUser);
                    if (!result.Succeeded)
                    {
                        var errors = result.Errors.Select(e => e.Description);

                        throw new Exception(result.Errors.ToString());
                    }
                    return newUser;
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> Update(UserDto user, string? password)
        {
            try
            {
                var newUser = await _repository.Get(user.Id) ?? throw new Exception("User not found");
                if (newUser != null)
                {
                    newUser.Name = user.Name;
                    newUser.EmailConfirmed = false;
                    newUser.ImagePath = user.ImagePath;
                    newUser.LastUpdate = DateTime.Now;
                    newUser.MaritalStatus = user.MaritalStatus;
                    newUser.Neighborhood = user.Neighborhood;
                    newUser.Phone = user.Phone;
                    newUser.State = user.State;
                    newUser.ZipCode = user.ZipCode;
                    newUser.ImagePath = user.ImagePath;
                    newUser.Address = user.Address;
                    newUser.AddressNumber = user.AddressNumber;
                    newUser.BirthDate = user.BirthDate;
                    newUser.City = user.City;
                    newUser.Complement = user.Complement;
                    newUser.Country = user.Country;
                    newUser.Gender = user.Gender;
                    newUser.Informations = user.Informations;
                    newUser.Job = user.Job;
                    newUser.Name = user.Name;
                    var result = await _userManager.UpdateAsync(newUser);
                    if (!result.Succeeded)
                    {
                        var errors = result.Errors.Select(e => e.Description);

                        throw new Exception(result.Errors.ToString());
                    }
                    return newUser;
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}