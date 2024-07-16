using System.Collections.Generic;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Domain.DTOs;

public record RecipeDto(RecipeDetailsDto RecipeDetails, List<ShoppingItemDto> ShoppingItems);