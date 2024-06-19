using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.RepositoryService;

public class RepositoryService : IRepositoryService
{
    public Task<Result<List<Recipe>>> GetRecipesAsync()
    {
        return Task.FromResult(Result.Success(new List<Recipe>()));
    }

    public Task<Result<List<Recipe>>> GetSelectedRecipesAsync(List<int> recipeIds)
    {
        throw new System.NotImplementedException();
    }

    public Task<Result<Recipe>> GetRecipeAsync(int recipeId)
    {
        return Task.FromResult(Recipe.Create("Test Recipe", "Test Directions", "Test description", new()));
    }

    public Task<Result> CreateRecipeAsync(Recipe recipe)
    {
        throw new System.NotImplementedException();
    }

    public Task<Result<Recipe>> UpdateRecipeAsync(Recipe recipe)
    {
        throw new System.NotImplementedException();
    }

    public Task<Result> DeleteRecipeAsync(int recipeId)
    {
        throw new System.NotImplementedException();
    }
}