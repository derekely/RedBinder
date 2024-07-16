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
            options.UseSqlServer(configuration.GetConnectionString("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;")));

        services.AddScoped<IRepositoryService, RepositoryService>();
        
        return services;
    }
}