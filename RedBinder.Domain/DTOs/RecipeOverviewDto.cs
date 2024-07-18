using RedBinder.Domain.Entities;

namespace RedBinder.Domain.DTOs;

public record RecipeOverviewDto(int Id, string Name, string Directions, string Description)
{
    public RecipeOverviewDto(RecipeOverview recipeOverview) : this(recipeOverview.Id, recipeOverview.Name, recipeOverview.Directions, recipeOverview.Description)
    {
    }
}