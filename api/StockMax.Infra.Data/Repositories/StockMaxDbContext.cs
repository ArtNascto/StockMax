using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StockMax.Domain.Models.Entity;

namespace StockMax.Infra.Data.Repositories
{
    public class StockMaxDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Color>? Colors { get; set; }

        public StockMaxDbContext(DbContextOptions<StockMaxDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=stockmax;User Id=sa;Password=19J<SPQB4Ic;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public class StockMaxDbContextFactory : IDesignTimeDbContextFactory<StockMaxDbContext>
    {
        public StockMaxDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockMaxDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=stockmax;User Id=sa;Password=19J<SPQB4Ic;TrustServerCertificate=True");
            return new StockMaxDbContext(optionsBuilder.Options);
        }
    }
}