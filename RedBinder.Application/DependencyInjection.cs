using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using RedBinder.Application.CreateRecipe;
using RedBinder.Application.DeleteRecipe;
using RedBinder.Application.GetRecipe;
using RedBinder.Application.ShoppingList;

namespace RedBinder.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            cfg.RegisterServicesFromAssemblies(typeof(RecipeCreationRequest).Assembly);
            cfg.RegisterServicesFromAssemblies(typeof(DeleteRecipeCommand).Assembly);
            cfg.RegisterServicesFromAssemblies(typeof(GetAllRecipeQuery).Assembly);
            cfg.RegisterServicesFromAssemblies(typeof(GetRecipeQuery).Assembly);
            cfg.RegisterServicesFromAssemblies(typeof(GetShoppingListQuery).Assembly);
        });
        
        // services.GetRequiredService<IMediator>();
        
        return services;
    }
}