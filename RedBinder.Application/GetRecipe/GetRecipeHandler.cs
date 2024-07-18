using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.DTOs;

namespace RedBinder.Application.GetRecipe;
public record GetRecipeQuery(int RecipeId) : IRequest<Result<RecipeDto>>; 

public class GetRecipeHandler(IRepositoryService repositoryService) : IRequestHandler<GetRecipeQuery, Result<RecipeDto>>
{
    ShoppingItemDto _shoppingItemDto = new ShoppingItemDto(new IngredientDto(1, "Name"),
        [new MeasurementDto(1, "Name", 12)]);
    
    public async Task<Result<RecipeDto>> Handle(GetRecipeQuery request, CancellationToken cancellationToken) => 
    // await Task.FromResult(Result.Success(new RecipeDto(new RecipeOverviewDto(1, "Name", "Directions", "Description"), [_shoppingItemDto])));    
    await repositoryService.GetRecipeAsync(request.RecipeId)
            .Map(recipe => new RecipeDto(new RecipeOverviewDto(recipe.RecipeOverview), recipe.ShoppingItems.Select(item => new ShoppingItemDto(item)).ToList()));
}