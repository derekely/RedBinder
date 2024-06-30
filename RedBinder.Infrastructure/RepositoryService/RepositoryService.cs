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
    
    // Done
    public async Task<Result<List<RecipeDetails>>> GetRecipesAsync() =>
        await GetFromDatabase(context => context.RecipeDetails
            .ToListAsync(), e => e.ToString());

    // Done
    public async Task<Result<ShoppingCart>> GetSelectedRecipesAsync(List<int> recipeIds) =>
        await GetFromDatabase(context => context.RecipeJoins
                .Include(recipe => recipe.Measurement)
                .Include(recipe => recipe.Ingredient)
                .Where(rj => recipeIds.Contains(rj.RecipeDetailsId))
                .ToListAsync(), e => e.ToString())
            .Bind(TranslateToRecipesFromRecipeJoin)
            .Map(recipes => recipes.SelectMany(recipe => recipe.ShoppingItems).ToList())
            .Bind(ShoppingCart.Create);

    // Done
    public async Task<Result<Recipe>> GetRecipeAsync(int recipeId) =>
        await GetFromDatabase(context => context.RecipeJoins
                    .Include(recipe => recipe.Measurement)
                    .Include(recipe => recipe.Ingredient)
                    .Where(recipe => recipe.Id == recipeId).ToListAsync()
                , e => e)
            .Bind(rjs => rjs.Select(rj => ShoppingItem.Create(rj.Ingredient, [rj.Measurement])).Combine()
                .Map(shoppingItems => new Recipe(rjs.First().RecipeDetails, shoppingItems.ToList())));

    public async Task<Result> CreateRecipeAsync(Recipe recipe)
    {
        throw new NotImplementedException(); // TODO: Figure out how to save to database
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
    
    private static Result<List<Recipe>> TranslateToRecipesFromRecipeJoin(List<RecipeJoin> recipeJoins) =>
        recipeJoins.Select(rj => ShoppingItem.Create(rj.Ingredient, [rj.Measurement])
                .Map(shoppingItems => new Recipe(rj.RecipeDetails, [shoppingItems])))
            .Combine()
            .Map(iEnum => iEnum.ToList());
    
    private static Result<Recipe> TranslateToRecipeFromRecipeJoin(RecipeJoin recipeJoin) =>
        ShoppingItem.Create(recipeJoin.Ingredient, [recipeJoin.Measurement])
            .Map(shoppingItems => new Recipe(recipeJoin.RecipeDetails, [shoppingItems]));
    
    private static DatabaseContext.DatabaseContext GetContext() => new();

    private static async Task<Result> SaveToDatabase(Func<DatabaseContext.DatabaseContext, Task> query, Func<string, string> errorFormatting)
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