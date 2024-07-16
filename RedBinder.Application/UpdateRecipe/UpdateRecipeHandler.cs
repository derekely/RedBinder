using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Application.UpdateRecipe;

public record UpdateRecipeCommand(Recipe Recipe) : IRequest<Result>;

public class UpdateRecipeHandler(IRepositoryService repositoryService) : IRequestHandler<UpdateRecipeCommand, Result>
{
    private readonly IRepositoryService _repositoryService = repositoryService;
    public async Task<Result> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken) => await _repositoryService.UpdateRecipeAsync(request.Recipe);
}