using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Setup dependency injection (Scoped: HTTP リクエストごとにインスタンスが作成され、同じリクエスト内では同じインスタンスが使用されます。)
            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors()
                    )
                .BuildServiceProvider();

            using var context = serviceProvider.GetService<ApplicationDbContext>() ?? throw new Exception();

            context.Configs.ToList().ForEach(x => Console.WriteLine(x.ConfigKey + ":" + x.ConfigValue));

            Console.ReadKey();

            Console.WriteLine("-----------------------");
            using var context2 = new ApplicationDbContext2();

            Console.WriteLine(context.Configs.Count());
        }
    }
}
