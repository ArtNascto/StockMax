namespace StockMax.Domain.Interfaces.Database
{
    public interface ISeed
    {
        Task SeedInitialUsers();

        Task SeedColors();
    }
}