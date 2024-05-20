using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMax.API.Handlers;
using StockMax.API.Utils;
using StockMax.Domain.Interfaces.Services;
using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;
using StockMax.Domain.Models.View.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace StockMax.API.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController(JwtHandler jwtHandler, IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly JwtHandler _jwtHandler = jwtHandler;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto model)
        {
            try
            {
                if (model.Email == null || model.Password == null)
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Usuário ou senha inválido" });
                if (_service == null)
                    return StatusCode(500);
                User user = await _service.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Usuário ou senha inválido" });
                }
                var userEntity = await _service.Get(user.Id);
                if (userEntity == null)
                {
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Usuário ou senha inválido" });
                }
                if (user != null)
                {
                    if (user.DeletionTime.HasValue)
                        return Unauthorized(new AuthResponseDto { ErrorMessage = "Usuário desativado" });
                    if (!await _service.CheckPasswordAsync(user, model.Password))
                        return Unauthorized(new AuthResponseDto { ErrorMessage = "Usuário ou senha inválido" });

                    var signingCredentials = _jwtHandler.GetSigningCredentials();
                    IdentityUser u = new()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                    };

                    var claims = _jwtHandler.GetClaims(
                        u
                    );
                    var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    user.LastAccess = DateTime.Now;
                    await _service.Update(user, null);

                    return Ok(
                        new AuthResponseDto
                        {
                            IsAuthSuccessful = true,
                            Token = token,
                            Name = user.Name,
                            Role = "admin",
                            LastAccess = userEntity.LastAccess
                        }
                    );
                }
                else
                {
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _service.Get(id);

            if (user != null && !string.IsNullOrEmpty(user.Id))
            {
                return Ok(new UserDto()
                {
                    Uin = user.Uin,
                    EmailConfirmed = user.EmailConfirmed,
                    LastAccess = user.LastAccess,
                    CreationTime = user.CreationTime,
                    Email = user.Email ?? "",
                    Id = user.Id,
                    Name = user.Name ?? "",
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
                });
            }
            return NotFound(new UserDto());
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters)
        {
            var users = await _service.GetAll(queryParameters);
            var usersDto = new List<UserDto>();
            foreach (var user in users)
            {
                usersDto.Add(new UserDto()
                {
                    Uin = user.Uin,
                    EmailConfirmed = user.EmailConfirmed,
                    LastAccess = user.LastAccess,
                    CreationTime = user.CreationTime,
                    Email = user.Email ?? "",
                    Id = user.Id,
                    Name = user.Name ?? "",
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
                });
            }
            var allUsersCount = await _service.Count();
            var paginationMetadata = new
            {
                totalCount = allUsersCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allUsersCount)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            var toReturn = usersDto.Select(QueryUtil<UserDto>.ExpandSingleItem);

            return Ok(new
            {
                value = toReturn,
                totalCount = allUsersCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allUsersCount)
            });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _service.Delete(id);

            return Ok();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(UserDto user)
        {
            var userID = string.Empty;
            var authToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userToken = new User();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenS = tokenHandler.ReadToken(authToken) as JwtSecurityToken;
            if (tokenS != null)
            {
                var userId = tokenS.Claims.First(claim => claim.Type == "id").Value;
                var userName = tokenS.Claims.First(claim => claim.Type == "userName").Value;
                userToken = await _service.Get(userId);
            }
            try
            {
                var exists = await _service.FindByEmailAsync(user.Email);
                if (exists != null && !exists.DeletionTime.HasValue)
                {
                    throw new Exception("User already exists");
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            try
            {
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
                newUser = await _service.Create(newUser, user.Password);
                userID = newUser.Id;
                user.Id = userID;
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (!string.IsNullOrEmpty(userID))
                    await Delete(userID);
                throw new Exception("Error when create user", ex);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(UserDto user, [FromRoute] string id)
        {
            var authToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var userToken = new User();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenS = tokenHandler.ReadToken(authToken) as JwtSecurityToken;
            if (tokenS != null)
            {
                var userId = tokenS.Claims.First(claim => claim.Type == "id").Value;
                var userName = tokenS.Claims.First(claim => claim.Type == "userName").Value;
                userToken = await _service.Get(userId);
            }
            var newUser = await _service.Update(user, user.Password);
            return Ok(newUser);
        }
    }
}