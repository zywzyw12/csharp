using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            // Setup dependency injection (Scoped: HTTP リクエストごとにインスタンスが作成され、同じリクエスト内では同じインスタンスが使用されます。)
            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .BuildServiceProvider();

            using var context = serviceProvider.GetService<ApplicationDbContext>() ?? throw new Exception();

            context.Configs.ToList().ForEach(x => Console.WriteLine(x.ConfigKey + ":" + x.ConfigValue));

            Console.ReadKey();

            Console.WriteLine("-----------------------");
            using var context2 = new ApplicationDbContext2() ;

            Console.WriteLine(context.Configs.Count());
        }
    }
}
