using System.Collections.Generic;

namespace RedBinder.Domain.DTOs;

public record RecipeDto(RecipeOverviewDto RecipeOverview, List<ShoppingItemDto> ShoppingItems);