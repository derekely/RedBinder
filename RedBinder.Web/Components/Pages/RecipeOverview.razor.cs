using RedBinder.Domain;

namespace TestingBlazorPages.Components.Pages;

public partial class RecipeOverview
{
    public List<Recipe>? Recipes { get; set; } = default;
}