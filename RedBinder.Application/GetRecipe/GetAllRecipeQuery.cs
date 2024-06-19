using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;

namespace RedBinder.Application.GetRecipe;

public record GetAllRecipeQuery : IRequest<Result<List<Recipe>>>;

public class GetAllRecipeHandler(IRepositoryService repositoryService) : IRequestHandler<GetAllRecipeQuery, Result<List<Recipe>>>
{
    public async Task<Result<List<Recipe>>> Handle(GetAllRecipeQuery request, CancellationToken cancellationToken) => await repositoryService.GetRecipesAsync();
}