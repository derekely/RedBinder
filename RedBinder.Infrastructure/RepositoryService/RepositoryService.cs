using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;
using RedBinder.Infrastructure.DatabaseContext;

namespace RedBinder.Infrastructure.RepositoryService;

public class RepositoryService(DatabaseContext.DatabaseContext databaseContext) : IRepositoryService
{
    // Done
    public async Task<Result<List<RecipeDetails>>> GetRecipesAsync() =>
        await GetFromDatabaseAsync(context => context.RecipeDetails
                .ToListAsync(), e => e.ToString())
            .Bind(maybeRecipeDetails => maybeRecipeDetails.ToResult("No recipes found"));

    // Done
    public async Task<Result<ShoppingCart>> GetSelectedRecipesAsync(List<int> recipeIds) =>
        await GetFromDatabaseAsync(context => context.RecipeJoins
                .Include(recipe => recipe.Measurement)
                .Include(recipe => recipe.Ingredient)
                .Where(rj => recipeIds.Contains(rj.RecipeDetailsId))
                .ToListAsync(), e => e.ToString())
            .Bind(maybeRecipeJoins => maybeRecipeJoins.ToResult("No recipes found"))
            .Bind(TranslateToRecipesFromRecipeJoin)
            .Map(recipes => recipes.SelectMany(recipe => recipe.ShoppingItems).ToList())
            .Bind(ShoppingCart.Create);

    // Done
    public async Task<Result<Recipe>> GetRecipeAsync(int recipeId) =>
        await GetFromDatabaseAsync(context => context.RecipeJoins
                    .Include(recipe => recipe.Measurement)
                    .Include(recipe => recipe.Ingredient)
                    .Where(recipe => recipe.Id == recipeId).ToListAsync()
                , e => e)
            .Bind(maybeRecipeJoins => maybeRecipeJoins.ToResult("No recipe found"))
            .Bind(rjs => rjs.Select(rj => ShoppingItem.Create(rj.Ingredient, [rj.Measurement])).Combine()
                .Map(shoppingItems => new Recipe(rjs.First().RecipeDetails, shoppingItems.ToList())));

    // Done
    public async Task<Result> CreateRecipeAsync(Recipe recipe) =>
        await EnsureRecipeDetailsAsync(recipe.RecipeDetails)
            .Bind(recipeDetails => recipe.ShoppingItems.Select(SaveShoppingItem).Combine()
                .Bind(iAndMs => 
                    SaveToDatabaseAsync(async context => await context.RecipeJoins.AddRangeAsync(iAndMs.Select(ingAndMeas => new RecipeJoin
                    {
                        RecipeDetails = recipeDetails,
                        Ingredient = ingAndMeas.ingredient,
                        Measurement = ingAndMeas.measurement
                    }).ToList()), e => e.ToString())));
    // Done
    public async Task<Result> UpdateRecipeAsync(Recipe recipe) =>
        await EnsureRecipeDetailsAsync(recipe.RecipeDetails)
            .Bind(recipeDetails => recipe.ShoppingItems.Select(SaveShoppingItem).Combine()
                .Bind(iAndMs => SaveToDatabaseAsync(async context =>
                {
                    context.RecipeJoins.RemoveRange(context.RecipeJoins.Where(rj => rj.RecipeDetailsId == recipeDetails.Id));
                    await context.RecipeJoins.AddRangeAsync(iAndMs.Select(ingAndMeas => new RecipeJoin
                    {
                        RecipeDetails = recipeDetails,
                        Ingredient = ingAndMeas.ingredient,
                        Measurement = ingAndMeas.measurement
                    }));
                }, e => e.ToString())));
    // Done
    public async Task<Result> DeleteRecipeAsync(int recipeId) =>
        await SaveToDatabaseAsync(context =>
        {
            context.RecipeJoins.RemoveRange(context.RecipeJoins.Where(rj => rj.RecipeDetailsId == recipeId));
            context.RecipeDetails.RemoveRange(context.RecipeDetails.Where(rd => rd.Id == recipeId));
            return Task.CompletedTask;
        }, e => e.ToString());


    #region Private Methods
    
    private static Result<List<Recipe>> TranslateToRecipesFromRecipeJoin(List<RecipeJoin> recipeJoins) =>
        recipeJoins.Select(rj => ShoppingItem.Create(rj.Ingredient, [rj.Measurement])
                .Map(shoppingItems => new Recipe(rj.RecipeDetails, [shoppingItems])))
            .Combine()
            .Map(iEnum => iEnum.ToList());
    
    private static DatabaseContext.DatabaseContext GetContext() => new();

    private static async Task<Result> SaveToDatabaseAsync(Func<DatabaseContext.DatabaseContext, Task> query, Func<string, string> errorFormatting)
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

    private static async Task<Result<Maybe<T>>> GetFromDatabaseAsync<T>(Func<DatabaseContext.DatabaseContext, Task<T>> query, Func<string, string> errorFormatting)
    {
        var context = GetContext();

        Result<Maybe<T>> queryResult = await Result.Try(async () => Maybe.From(await query(context)), e => errorFormatting(e.ToString()));

        await context.DisposeAsync();

        return queryResult;
    }

    private static async Task<Result<RecipeDetails>> EnsureRecipeDetailsAsync(RecipeDetails recipeDetails) =>
        await GetFromDatabaseAsync(context => context.RecipeDetails
                .Where(rd => rd.Name == recipeDetails.Name)
                .FirstOrDefaultAsync(), e => e.ToString())
            .Bind(async foundRecipeDetails =>
                foundRecipeDetails.HasValue
                    ? await Task.FromResult(Result.Success(foundRecipeDetails.Value!))
                    : await SaveToDatabaseAsync(async context => await context.RecipeDetails.AddAsync(recipeDetails), e => e.ToString())
                        .Bind(() => GetFromDatabaseAsync(context => context.RecipeDetails
                            .Where(rd => rd.Name == recipeDetails.Name)
                            .FirstOrDefaultAsync(), e => e.ToString()))
                        .Map(maybeNewRecipeDetails => maybeNewRecipeDetails.Value!));

    private static async Task<Result<(Ingredient ingredient, Measurement measurement)>> SaveShoppingItem(ShoppingItem shoppingItem)
    {
       var ingredientResult = await GetFromDatabaseAsync(context => context.Ingredients
               .Where(queriedIngredient => queriedIngredient.Name == shoppingItem.Ingredient.Name)
               .FirstOrDefaultAsync()
           , e => e.ToString())
           .Bind(async foundIngredient => 
               foundIngredient.HasValue 
                ? await Task.FromResult(Result.Success(foundIngredient.Value!))
               : await SaveToDatabaseAsync(async context => await context.Ingredients.AddAsync(shoppingItem.Ingredient), e => e.ToString())
                   .Bind(() => GetFromDatabaseAsync(context => context.Ingredients
                           .Where(queriedIngredient => queriedIngredient.Name == shoppingItem.Ingredient.Name)
                           .FirstOrDefaultAsync()
                       , e => e.ToString()))
           .Map(maybeNewIngredient => maybeNewIngredient.Value!));

       var measurementResult = await GetFromDatabaseAsync(context => context.Measurements
                   .Where(queriedMeasurement => queriedMeasurement.Name == shoppingItem.Measurements.First().Name)
                   .FirstOrDefaultAsync()
               , e => e.ToString())
           .Bind(async foundMeasurement =>
               foundMeasurement.HasValue
                   ? await Task.FromResult(Result.Success(foundMeasurement.Value!))
                   : await SaveToDatabaseAsync(
                           async context => await context.Measurements.AddAsync(shoppingItem.Measurements.First()),
                           e => e.ToString())
                       .Bind(() => GetFromDatabaseAsync(context => context.Measurements
                               .Where(queriedMeasurement =>
                                   queriedMeasurement.Name == shoppingItem.Measurements.First().Name)
                               .FirstOrDefaultAsync()
                           , e => e.ToString()))
                       .Map(maybeNeweasurement => maybeNeweasurement.Value!));
       
         return ingredientResult.Bind(ingredient => measurementResult.Map(measurement => (ingredient, measurement)));
    }
    
    #endregion
}