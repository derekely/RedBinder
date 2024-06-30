using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Application.GetRecipe;

public record GetAllRecipeQuery : IRequest<Result<List<RecipeDetails>>>;

public class GetAllRecipeHandler(IRepositoryService repositoryService) : IRequestHandler<GetAllRecipeQuery, Result<List<RecipeDetails>>>
{
    public async Task<Result<List<RecipeDetails>>> Handle(GetAllRecipeQuery request, CancellationToken cancellationToken) => await repositoryService.GetRecipesAsync();
}