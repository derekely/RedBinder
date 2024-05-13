using RedBinder.Domain.Entities;

namespace TestingBlazorPages.Components.Pages;

public partial class RecipeOverview
{
    public List<Recipe>? Recipes { get; set; } = default;
}