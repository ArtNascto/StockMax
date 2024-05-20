using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockMax.Domain.Interfaces.Database;
using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;

namespace StockMax.Infra.Data.Repositories
{
    public class Seed : ISeed
    {
        private readonly IConfiguration _config;

        private readonly StockMaxDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        private List<CreateUserDto> users = new()
        {
            new() { Email = "arthurgnascto@gmail.com", Password = "123qwe", Name = "Arthur Nascimento", Uin = "123.123.123-87" },
            new() { Email = "nandactorres.fcp@gmail.com", Password = "123qwe", Name = "Fernanda Pereira", Uin = "123.123.123-87" },
            new() { Email = "etgc3023.life@gmail.com", Password = "123qwe", Name = "Enzo Torres", Uin = "123.123.123-87" },
        };

        private List<Color> colors = new() {
            new() { Value = "Vermelho" } ,
            new() { Value = "Verde" },
            new() { Value = "Azul" },
            new() { Value = "Amarelo" },
            new() { Value = "Ciano" },
            new() { Value = "Laranja" },
            new() { Value = "Laranja" },
            new() { Value = "Verde-limão" },
            new() { Value = "Turquesa" },
            new() { Value = "Violeta" },
            new() { Value = "Preto" },
            new() { Value = "Cinza escuro" },
            new() { Value = "Cinza médio" },
            new() { Value = "Cinza claro" },
            new() { Value = "Branco" },
            new() { Value = "Marrom" },
            new() { Value = "Rosa" },
            new() { Value = "Dourado" },
            new() { Value = "Prata" },
            new() { Value = "Bege" },
            new() { Value = "Azul celeste" },
            new() { Value = "Azul marinho" },
            new() { Value = "Azul royal" },
            new() { Value = "Azul turquesa" },
            new() { Value = "Azul cobalto" },
            new() { Value = "Verde oliva" },
            new() { Value = "Verde esmeralda" },
            new() { Value = "Verde menta" },
            new() { Value = "Verde floresta" },
            new() { Value = "Verde musgo" },
            new() { Value = "Vermelho escarlate" },
            new() { Value = "Vermelho borgonha" },
            new() { Value = "Vermelho carmim" },
            new() { Value = "Vermelho coral" },
            new() { Value = "Vermelho cereja" },
            new() { Value = "Amarelo mostarda" },
            new() { Value = "Amarelo canário" },
            new() { Value = "Amarelo ouro" },
            new() { Value = "Amarelo âmbar" },
            new() { Value = "Amarelo creme" },
            new() { Value = "Roxo lavanda" },
            new() { Value = "Roxo lilás" },
            new() { Value = "Roxo ametista" },
            new() { Value = "Roxo berinjela" },
            new() { Value = "Roxo orquídea" },
            new() { Value = "Salmão" },
            new() { Value = "Pêssego" },
            new() { Value = "Coral" },
            new() { Value = "Marfim" },
            new() { Value = "Caqui" },
        };

        public Seed(UserManager<User> userManager, IConfiguration config, StockMaxDbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedColors()
        {
            foreach (var color in colors)
            {
                if (!_dbContext.Colors.Any(c => c.Value == color.Value))
                {
                    _dbContext.Add(color);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task SeedInitialUsers()
        {
            foreach (var user in users)
            {
                var hasUser = await _dbContext.Users?.AnyAsync(u => u.Email == user.Email && !u.DeletionTime.HasValue);
                if (!hasUser)
                {
                    var newUser = new User()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Uin = user.Uin,
                        UserName = user.Email,
                        EmailConfirmed = true,
                        CreationTime = DateTime.Now,
                    };
                    try
                    {
                        var result = await _userManager.CreateAsync(newUser, user.Password);
                        if (result.Errors != null && result.Errors.Count() > 0)
                        {
                            throw new Exception(result.Errors.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            };
        }
    }
}