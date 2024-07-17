using CSharpFunctionalExtensions;
using RedBinder.Domain.DTOs;

namespace RedBinder.Web.Models;

public static class TranslationExtensions
{
    public static RecipeDto ToDtoFromCreationModel(this RecipeModel model, List<ShoppingItemModel> shoppingItemsModel)
    {
        RecipeDetailsDto recipeDetailsDto = new(0 ,model.Name, model.Directions, model.Description);
        
        List<ShoppingItemDto> shoppingItemsDto = shoppingItemsModel.Select(x =>
        {
            var ingredientDto = new IngredientDto(0, x.Name);
            var measurementDto = new MeasurementDto(0, x.Measurement, x.Quantity);
            return new ShoppingItemDto(ingredientDto, [measurementDto]);
        }).ToList();

        return new RecipeDto(recipeDetailsDto, shoppingItemsDto);
    }
}