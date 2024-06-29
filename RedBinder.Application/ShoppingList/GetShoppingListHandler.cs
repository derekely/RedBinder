using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Application.ShoppingList;

public record GetShoppingListQuery(List<int> SelectedRecipes) : IRequest<Result<List<Recipe>>>;

public class GetShoppingListHandler(IRepositoryService repositoryService) : IRequestHandler<GetShoppingListQuery, Result<List<Recipe>>>
{
    public async Task<Result<List<Recipe>>> Handle(GetShoppingListQuery request, CancellationToken cancellationToken) => await repositoryService.GetSelectedRecipesAsync(request.SelectedRecipes);
}