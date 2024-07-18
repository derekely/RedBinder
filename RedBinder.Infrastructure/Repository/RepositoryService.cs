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

namespace RedBinder.Infrastructure.Repository;

public class RepositoryService(DbContextOptions options) : IRepositoryService
{
    private readonly DbContextOptions _options = options;
    
    //Confirmed works
    public async Task<Result<List<RecipeOverview>>> GetRecipesAsync() =>
        await GetFromDatabaseAsync(context => context.RecipeOverviews
                .ToListAsync(), e => e.ToString())
            .Bind(maybeRecipeDetails => maybeRecipeDetails.ToResult("No recipes found"));
    
    public async Task<Result<ShoppingCart>> GetSelectedRecipesAsync(List<int> recipeIds) =>
        await GetFromDatabaseAsync(context => context.RecipeJoins
                .Include(recipe => recipe.Measurement)
                .Include(recipe => recipe.Ingredient)
                .Where(rj => recipeIds.Contains(rj.RecipeOverviewId))
                .ToListAsync(), e => e.ToString())
            .Bind(maybeRecipeJoins => maybeRecipeJoins.ToResult("No recipes found"))
            .Bind(TranslateToRecipesFromRecipeJoin)
            .Map(recipes => recipes.SelectMany(recipe => recipe.ShoppingItems).ToList())
            .Bind(ShoppingCart.Create);
    
    //Confirmed works
    public async Task<Result<Recipe>> GetRecipeAsync(int recipeId) =>
        await GetFromDatabaseAsync(context => context.RecipeJoins
                    .Include(recipe => recipe.Measurement)
                    .Include(recipe => recipe.Ingredient)
                    .Include(recipe => recipe.RecipeOverview)
                    .Where(recipeJoin => recipeJoin.RecipeOverviewId == recipeId).ToListAsync()
                , e => e)
            .Bind(maybeRecipeJoins => maybeRecipeJoins.ToResult("No recipe found"))
            .Bind(rjs => rjs.Select(rj => ShoppingItem.Create(rj.Ingredient, [rj.Measurement])).Combine()
                .Map(shoppingItems => new Recipe(rjs.First().RecipeOverview, shoppingItems.ToList())));
    
    //Confirmed works
    public async Task<Result> CreateRecipeAsync(Recipe recipe)
    {
        return await Result.Try(async () =>
        {
            var context = new DatabaseContextRedBinder(_options);
            await using(context)
            {
                var recipeJoinsResult = await EnsureEntityExistsByPropertyAsync(context.RecipeOverviews, recipe.RecipeOverview, nameof(RecipeOverview.Name), recipe.RecipeOverview.Name)
                    .Bind(recipeOverview => recipe.ShoppingItems.Select(async item =>
                    {
                         return await EnsureEntityExistsByPropertyAsync(context.Ingredients, item.Ingredient, nameof(Ingredient.Name), item.Ingredient.Name)
                            .Bind(ingredient => EnsureEntityExistsByPropertyAsync(context.Measurements, item.Measurements.First(), nameof(Measurement.Name), item.Measurements.First().Name)
                                .Map(measurement => new RecipeJoin
                                {
                                    RecipeOverview = recipeOverview,
                                    Ingredient = ingredient,
                                    Measurement = measurement
                                }));
                    }).Combine("|"))
                    .Tap(recipeJoins => context.RecipeJoins.AddRangeAsync(recipeJoins));
                await context.SaveChangesAsync();
                return recipeJoinsResult;
            }      
        });
    }

    public async Task<Result> UpdateRecipeAsync(Recipe recipe) =>
        await EnsureRecipeDetailsAsync(recipe.RecipeOverview)
            .Bind(recipeDetails => recipe.ShoppingItems.Select(SaveShoppingItem).Combine()
                .Bind(iAndMs => SaveToDatabaseAsync(async context =>
                {
                    context.RecipeJoins.RemoveRange(context.RecipeJoins.Where(rj => rj.RecipeOverviewId == recipeDetails.Id));
                    await context.RecipeJoins.AddRangeAsync(iAndMs.Select(ingAndMeas => new RecipeJoin
                    {
                        RecipeOverview = recipeDetails,
                        Ingredient = ingAndMeas.ingredient,
                        Measurement = ingAndMeas.measurement
                    }));
                }, e => e.ToString())));

    public async Task<Result> DeleteRecipeAsync(int recipeId) =>
        await SaveToDatabaseAsync(context =>
        {
            context.RecipeJoins.RemoveRange(context.RecipeJoins.Where(rj => rj.RecipeOverviewId == recipeId));
            context.RecipeOverviews.RemoveRange(context.RecipeOverviews.Where(rd => rd.Id == recipeId));
            return Task.CompletedTask;
        }, e => e.ToString());


    #region Private Methods
    
    // Generic method to check if an entity exists and add or update accordingly
    private async Task<Result<T>> EnsureEntityExistsByPropertyAsync<T>(DbSet<T> dbSet, T entity, string propertyName,string entityName) where T : class
    {
        return await Result.Try(async () =>
        {
            var context = new DatabaseContextRedBinder(_options);
            var existingEntity = await dbSet.FirstOrDefaultAsync(dbEntity => EF.Property<string>(dbEntity, propertyName) == entityName);
            if (existingEntity == null)
            {
                dbSet.Add(entity);
            }
            else
            {
                context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            await context.SaveChangesAsync();
            await context.DisposeAsync();
            return entity;
        }, e => e.ToString());
    }
    
    private static Result<List<Recipe>> TranslateToRecipesFromRecipeJoin(List<RecipeJoin> recipeJoins) =>
        recipeJoins.Select(rj => ShoppingItem.Create(rj.Ingredient, [rj.Measurement])
                .Map(shoppingItems => new Recipe(rj.RecipeOverview, [shoppingItems])))
            .Combine()
            .Map(iEnum => iEnum.ToList());

    private async Task<Result> SaveToDatabaseAsync(Func<DatabaseContextRedBinder, Task> query, Func<string, string> errorFormatting)
    {
        return await Result.Try(async () =>
        {
            var context = new DatabaseContextRedBinder(_options);
            await query(context);
            await context.SaveChangesAsync();
            await context.DisposeAsync();
        }, e => errorFormatting(e.ToString()));
    }

    private async Task<Result<Maybe<T>>> GetFromDatabaseAsync<T>(Func<DatabaseContextRedBinder, Task<T>> query, Func<string, string> errorFormatting)
    {
        return await Result.Try(async () =>
        {
            var context = new DatabaseContextRedBinder(_options);
            var queryResult = Maybe.From(await query(context));
            await context.DisposeAsync();
            return queryResult;
        }, e => errorFormatting(e.ToString()));
    }

    private async Task<Result<RecipeOverview>> EnsureRecipeDetailsAsync(RecipeOverview recipeOverview) =>
        await GetFromDatabaseAsync(context => context.RecipeOverviews
                .Where(rd => rd.Name == recipeOverview.Name)
                .FirstOrDefaultAsync(), e => e.ToString())
            .Bind(async foundRecipeDetails =>
                foundRecipeDetails.HasValue
                    ? await Task.FromResult(Result.Success(foundRecipeDetails.Value!))
                    : await SaveToDatabaseAsync(async context => await context.RecipeOverviews.AddAsync(recipeOverview), e => e.ToString())
                        .Bind(() => GetFromDatabaseAsync(context => context.RecipeOverviews
                            .Where(rd => rd.Name == recipeOverview.Name)
                            .FirstOrDefaultAsync(), e => e.ToString()))
                        .Map(maybeNewRecipeDetails => maybeNewRecipeDetails.Value!));

   private async Task<Result<(Ingredient ingredient, Measurement measurement)>> SaveShoppingItem(ShoppingItem shoppingItem)
{
    // Check if the Ingredient already exists and use it, otherwise add a new one.
    var ingredientResult = await GetFromDatabaseAsync(context => context.Ingredients
            .Where(queriedIngredient => queriedIngredient.Name == shoppingItem.Ingredient.Name)
            .FirstOrDefaultAsync(), e => e.ToString())
        .Bind(async foundIngredient =>
            foundIngredient.HasValue
                ? await Task.FromResult(Result.Success(foundIngredient.Value))
                : await SaveToDatabaseAsync(async context => await context.Ingredients.AddAsync(shoppingItem.Ingredient), e => e.ToString())
                    .Bind(() => GetFromDatabaseAsync(context => context.Ingredients
                            .Where(queriedIngredient => queriedIngredient.Name == shoppingItem.Ingredient.Name)
                            .FirstOrDefaultAsync(), e => e.ToString()))
                    .Map(maybeNewIngredient => maybeNewIngredient.Value));

    // Check if the Measurement already exists and use it, otherwise add a new one.
    var measurementResult = await GetFromDatabaseAsync(context => context.Measurements
            .Where(queriedMeasurement => queriedMeasurement.Name == shoppingItem.Measurements.First().Name)
            .FirstOrDefaultAsync(), e => e.ToString())
        .Bind(async foundMeasurement =>
            foundMeasurement.HasValue
                ? await Task.FromResult(Result.Success(foundMeasurement.Value))
                : await SaveToDatabaseAsync(async context => await context.Measurements.AddAsync(shoppingItem.Measurements.First()), e => e.ToString())
                    .Bind(() => GetFromDatabaseAsync(context => context.Measurements
                            .Where(queriedMeasurement => queriedMeasurement.Name == shoppingItem.Measurements.First().Name && queriedMeasurement.Quantity == shoppingItem.Measurements.First().Quantity)
                            .FirstOrDefaultAsync(), e => e.ToString()))
                    .Map(maybeNewMeasurement => maybeNewMeasurement.Value));

    return ingredientResult.Bind(ingredient => measurementResult.Map(measurement => (ingredient, measurement)));
}
    
    #endregion
}