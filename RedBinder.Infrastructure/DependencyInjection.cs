using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RedBinder.Application.ServiceInterface;
using RedBinder.Infrastructure.DatabaseContext;
using RedBinder.Infrastructure.Repository;

namespace RedBinder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContextRedBinder>(options =>
            options.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=RedBinder;Trusted_Connection=True;TrustServerCertificate=True;User Id=dbo;"
                , b => b.MigrationsAssembly(typeof(DatabaseContextRedBinder).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", DatabaseContextRedBinder.SchemaName)));

        services.AddScoped<IRepositoryService, RepositoryService>();
        
        return services;
    }
}