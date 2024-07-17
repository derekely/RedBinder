using System.Collections.Generic;

namespace RedBinder.Domain.DTOs;

public record RecipeDto(RecipeDetailsDto RecipeDetails, List<ShoppingItemDto> ShoppingItems);