using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;

namespace RedBinder.Application.CreateRecipe;

public record RecipeCreationRequest(Recipe Recipe) : IRequest<Result>;

public class RecipeCreationHandler(IRepositoryService repositoryService) : IRequestHandler<RecipeCreationRequest, Result>
{
    public async Task<Result> Handle(RecipeCreationRequest request, CancellationToken cancellationToken) => await repositoryService.CreateRecipeAsync(request.Recipe);
} 