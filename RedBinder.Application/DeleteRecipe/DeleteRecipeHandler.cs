using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;

namespace RedBinder.Application.DeleteRecipe;

public record DeleteRecipeCommand(int Id) : IRequest<Result>;

public class DeleteRecipeHandler(IRepositoryService RepositoryService) : IRequestHandler<DeleteRecipeCommand, Result>
{
    private readonly IRepositoryService _repositoryService = RepositoryService;
    public async Task<Result> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        return await _repositoryService.DeleteRecipeAsync(request.Id);
    }
}