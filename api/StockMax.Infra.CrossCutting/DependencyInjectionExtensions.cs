using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockMax.Application.Services;
using StockMax.Domain.Interfaces.Repositories;
using StockMax.Domain.Interfaces.Services;
using StockMax.Infra.Data.Repositories;

namespace StockMax.Infra.CrossCutting
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Repositories(services);
            Services(services);

            return services;
        }

        private static void Services(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
        }

        private static void Repositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}