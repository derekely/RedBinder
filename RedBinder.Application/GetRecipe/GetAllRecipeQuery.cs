using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.DTOs;

namespace RedBinder.Application.GetRecipe;

public record GetAllRecipeQuery : IRequest<Result<List<RecipeDetailsDto>>>;

public class GetAllRecipeHandler(IRepositoryService repositoryService) : IRequestHandler<GetAllRecipeQuery, Result<List<RecipeDetailsDto>>>
{
    public async Task<Result<List<RecipeDetailsDto>>> Handle(GetAllRecipeQuery request, CancellationToken cancellationToken) =>
        await repositoryService.GetRecipesAsync()
            .Map(recipes => recipes.Select(recipe => new RecipeDetailsDto(recipe)).ToList());
}