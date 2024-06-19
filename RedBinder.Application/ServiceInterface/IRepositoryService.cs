using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;

namespace RedBinder.Application.ServiceInterface;

public interface IRepositoryService
{
    public Task<Result<List<Recipe>>> GetRecipesAsync();
    public Task<Result<List<Recipe>>> GetSelectedRecipesAsync(List<int> recipeIds);
    public Task<Result<Recipe>> GetRecipeAsync(int recipeId);
    public Task<Result> CreateRecipeAsync(Recipe recipe);
    public Task<Result<Recipe>> UpdateRecipeAsync(Recipe recipe);
    public Task<Result> DeleteRecipeAsync(int recipeId);
}