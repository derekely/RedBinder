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
        return await SaveToDatabase(async context =>
        {
            //Details
            RecipeDetails recipeDetails = recipe.RecipeDetails;
            await context.RecipeDetails.AddAsync(recipeDetails);
            await context.SaveChangesAsync();
        
            // Shopping Items
            foreach (ShoppingItem shoppingItem in recipe.ShoppingItems)
            {
                // We will need to ensure that stuff isn't being added that shouldn't be added.... hmmmm
                Ingredient ingredient = shoppingItem.Ingredient;
                await context.Ingredients.AddAsync(ingredient);
                await context.SaveChangesAsync();
        
                Measurement measurement = new Measurement
                {
                    Name = shoppingItem.Measurements.First().Name,
                    Quantity = shoppingItem.Measurements.First().Quantity
                };
                await context.Measurements.AddAsync(measurement);
                await context.SaveChangesAsync();
        
                RecipeJoin recipeJoin = new RecipeJoin
                {
                    RecipeDetails = recipeDetails,
                    Ingredient = ingredient,
                    Measurement = measurement
                };
                await context.RecipeJoins.AddAsync(recipeJoin);
                await context.SaveChangesAsync();
            }
        }, e => e.ToString());
    }

    public async Task<Result<Recipe>> UpdateRecipeAsync(Recipe recipe)
    {
        throw new NotImplementedException(); // TODO: Figure out how to save to database
        // return await GetFromDatabase(context => context.RecipeJoins
        //             .Include(rj => rj.Measurement)
        //             .Include(rj => rj.Ingredient)
        //             .Where(rj => rj.Id == recipe.RecipeDetails.Id).ToListAsync()
        //        , e => e.ToString()) // TODO: Figure this out
            //.Bind();
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

    private async Task<Result<(int ingredientId, int measurementId)>> SaveShoppingItem(ShoppingItem shoppingItem)
    {
       // Check for duplicates for the ingredient and measurement, if they are there then return the ids of both

       Maybe<int> maybeIngredientId = await GetFromDatabase(context => context.Ingredients
               .Where(ingredient => ingredient.Name == shoppingItem.Ingredient.Name)
               .FirstOrDefaultAsync()
           , e => e.ToString());

       if (maybeIngredientId.HasNoValue)
       {
           // save it to the database. then get the id of that saved item and have it ready to be returned with the measurement id
              var ingredientId = await SaveToDatabase(context => context.Ingredients.AddAsync(shoppingItem.Ingredient),
                     e => e.ToString())
                .Map(ingredient => ingredient.Id);
       }

       var measurementId = await GetFromDatabase(context => context.Measurements
                   .Where(measurement => measurement.Name == shoppingItem.Measurements.First().Name)
                   .FirstOrDefaultAsync()
               , e => e.ToString())
           .Bind(measurement =>
           {
               if (measurement is not null) return Result.Success(measurement.Id);
               return SaveToDatabase(context => context.Measurements.AddAsync(shoppingItem.Measurements.First()),
                       e => e.ToString())
                   .Map(measurement => measurement.Id);
           });

       return (ingredientId, measurementId);
    }
    
    #endregion
}