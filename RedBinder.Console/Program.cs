using Microsoft.Extensions.DependencyInjection;
using RedBinder.Application;
using RedBinder.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace RedBinder.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Resolve services and call methods
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                // Resolve your services here and call methods on them
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication();
                    services.AddInfrastructure(hostContext.Configuration);
                });
    }
}