using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.DTOs;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Application.UpdateRecipe;

public record UpdateRecipeCommand(RecipeDto Recipe) : IRequest<Result>;

public class UpdateRecipeHandler(IRepositoryService repositoryService) : IRequestHandler<UpdateRecipeCommand, Result>
{
    public async Task<Result> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken) => await repositoryService.UpdateRecipeAsync(Recipe.ToRecipeFromDto(request.Recipe));
}