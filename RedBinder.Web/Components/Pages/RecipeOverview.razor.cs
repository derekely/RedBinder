using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;

namespace TestingBlazorPages.Components.Pages;

public partial class RecipeOverview
{
    public List<Recipe>? Recipes { get; set; } = default;
}