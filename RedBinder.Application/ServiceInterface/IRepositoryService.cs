using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Application.ServiceInterface;

public interface IRepositoryService
{
    public Task<Result<List<RecipeOverview>>> GetRecipesAsync();
    public Task<Result<ShoppingCart>> GetSelectedRecipesAsync(List<int> recipeIds);
    public Task<Result<Recipe>> GetRecipeAsync(int recipeId);
    public Task<Result> CreateRecipeAsync(Recipe recipe);
    public Task<Result> UpdateRecipeAsync(Recipe sourceRecipe);
    public Task<Result> DeleteRecipeAsync(int recipeId);
}