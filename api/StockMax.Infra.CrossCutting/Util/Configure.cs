using Microsoft.Extensions.Configuration;

namespace StockMax.Infra.CrossCutting.Util
{
    public class Configure
    {
        public static string configureApp(string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            return configuration[key].ToString();
        }
    }
}