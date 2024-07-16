using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Application.ShoppingList;

public record GetShoppingListQuery(List<int> SelectedRecipes) : IRequest<Result<ShoppingCart>>;

public class GetShoppingListHandler(IRepositoryService repositoryService) : IRequestHandler<GetShoppingListQuery, Result<ShoppingCart>>
{
    public async Task<Result<ShoppingCart>> Handle(GetShoppingListQuery request, CancellationToken cancellationToken) => await repositoryService.GetSelectedRecipesAsync(request.SelectedRecipes);
}