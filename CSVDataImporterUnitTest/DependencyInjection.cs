using CSVDataImporter.Data;
using CSVDataImporter.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSVDataImporterUnitTest
{
    public static class DependencyInjection
    {
        public static T GetRequiredService<T>() where T : class
        {
            var provider = Provider();

            return provider.GetRequiredService<T>();
        }

        private static IServiceProvider Provider()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();

            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("ConnectionString")));
            services.AddLogging();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            services.AddScoped<EmployeeService>();

            return services.BuildServiceProvider();
        }
    }
}
