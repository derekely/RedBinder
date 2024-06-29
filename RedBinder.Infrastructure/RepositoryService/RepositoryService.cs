using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;
using RedBinder.Infrastructure.DatabaseContext;

namespace RedBinder.Infrastructure.RepositoryService;

public class RepositoryService(DatabaseContext.DatabaseContext databaseContext) : IRepositoryService
{
    private readonly DatabaseContext.DatabaseContext _databaseContext = databaseContext;
    
    public Task<Result<List<Recipe>>> GetRecipesAsync()
    {
        return Task.FromResult(Result.Success(new List<Recipe>()));
    }

    public Task<Result<List<Recipe>>> GetSelectedRecipesAsync(List<int> recipeIds)
    {
        throw new System.NotImplementedException();
    }

    public async Task<Result<Recipe>> GetRecipeAsync(int recipeId) =>
        await GetFromDatabase(context => context.RecipeOverviews
                    .Include(recipe => recipe.Measurement)
                    .Include(recipe => recipe.Ingredient)
                    .Where(recipe => recipe.Id == recipeId).ToListAsync()
                , e => e)
            .Bind(rjs => rjs.Select(rj => ShoppingItem.Create(rj.Ingredient, [rj.Measurement])).Combine()
                .Map(shoppingItems => new Recipe(rjs.First().RecipeDetails, shoppingItems.ToList())));

    public Task<Result> CreateRecipeAsync(Recipe recipe)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Recipe>> UpdateRecipeAsync(Recipe recipe)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteRecipeAsync(int recipeId)
    {
        throw new NotImplementedException();
    }

    
    #region Private Methods
    private DatabaseContext.DatabaseContext GetContext()
    {
        return new();
    }
    
    private async Task<Result> SaveToDatabase(Func<DatabaseContext.DatabaseContext, Task> query, Func<string, string> errorFormatting)
    {
        DatabaseContext.DatabaseContext context = GetContext();
        
        Result<int> queryResult = await Result.Try(async () =>
        {
            await query(context);
            return await context.SaveChangesAsync();
        }, e => errorFormatting(e.ToString())); // TODO: do ToErrorString()

        await context.DisposeAsync();

        return queryResult;
    }

    private async Task<Result<T>> GetFromDatabase<T>(Func<DatabaseContext.DatabaseContext, Task<T>> query,
        Func<string, string> errorFormatting)
    {
        var context = GetContext();

        var queryResult = await Result.Try(async () => await query(context), e => errorFormatting(e.ToString()));

        await context.DisposeAsync();

        return queryResult;
    }
    #endregion
}