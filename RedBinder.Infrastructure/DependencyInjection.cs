using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedBinder.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace RedBinder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext.DatabaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IRepositoryService, RepositoryService.RepositoryService>();

        return services;
    }
}