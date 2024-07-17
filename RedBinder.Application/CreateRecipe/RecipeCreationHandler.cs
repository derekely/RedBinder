using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.DTOs;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Application.CreateRecipe;

public record RecipeCreationRequest(RecipeDto Recipe) : IRequest<Result>;

public class RecipeCreationHandler(IRepositoryService repositoryService) : IRequestHandler<RecipeCreationRequest, Result>
{
    public async Task<Result> Handle(RecipeCreationRequest request, CancellationToken cancellationToken)
    {
        var recipe = Recipe.ToRecipeFromDto(request.Recipe);
        return await repositoryService.CreateRecipeAsync(recipe);
    }
} 