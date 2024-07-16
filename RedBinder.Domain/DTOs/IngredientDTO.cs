using RedBinder.Domain.Entities;

namespace RedBinder.Domain.DTOs;

public record IngredientDto(int Id, string Name)
{
    public IngredientDto(Ingredient ingredient) : this(ingredient.Id, ingredient.Name)
    {
    }
}