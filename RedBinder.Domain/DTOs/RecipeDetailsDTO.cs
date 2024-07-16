using RedBinder.Domain.Entities;

namespace RedBinder.Domain.DTOs;

public record RecipeDetailsDto(int Id, string Name, string Directions, string Description)
{
    public RecipeDetailsDto(RecipeDetails recipeDetails) : this(recipeDetails.Id, recipeDetails.Name, recipeDetails.Directions, recipeDetails.Description)
    {
    }
}