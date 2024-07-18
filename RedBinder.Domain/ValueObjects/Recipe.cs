using System.Collections.Generic;
using System.Linq;
using RedBinder.Domain.DTOs;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain.ValueObjects;

public record Recipe(RecipeOverview RecipeOverview, List<ShoppingItem> ShoppingItems)
{
    public static Recipe ToRecipeFromDto(RecipeDto recipeDto) => new(RecipeOverview.ToRecipeDetailsFromDto(recipeDto.RecipeOverview), recipeDto.ShoppingItems.Select(ShoppingItem.ToShoppingItemFromDto).ToList());
}