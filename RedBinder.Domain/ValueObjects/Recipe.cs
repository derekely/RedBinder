using System.Collections.Generic;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain.ValueObjects;

public record Recipe(RecipeDetails RecipeDetails, List<ShoppingItem> ShoppingItems);