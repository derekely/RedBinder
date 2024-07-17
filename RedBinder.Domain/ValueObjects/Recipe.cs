using System.Collections.Generic;
using System.Linq;
using RedBinder.Domain.DTOs;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain.ValueObjects;

public record Recipe(RecipeDetails RecipeDetails, List<ShoppingItem> ShoppingItems)
{
    public static Recipe ToRecipeFromDto(RecipeDto recipeDto) => new(RecipeDetails.ToRecipeDetailsFromDto(recipeDto.RecipeDetails), recipeDto.ShoppingItems.Select(ShoppingItem.ToShoppingItemFromDto).ToList());
}