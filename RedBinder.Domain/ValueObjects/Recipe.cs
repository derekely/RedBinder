using System.Collections.Generic;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain.ValueObjects;

public class Recipe
{
    private Recipe(string name, string directions, string description, List<ShoppingItem> shoppingItems)
    {
        Name = name;
        Directions = directions;
        Description = description;
        ShoppingItems = shoppingItems;
    }
    
    public Recipe(RecipeDetails recipeDetails, List<ShoppingItem> shoppingItems)
    {
        Name = recipeDetails.Name;
        Directions = recipeDetails.Directions;
        Description = recipeDetails.Description;
        ShoppingItems = shoppingItems;
    }
    
    public string Name { get; }
    public string Directions { get; }
    public string Description { get; }
    public List<ShoppingItem> ShoppingItems { get; }
    
    public static Result<Recipe> Create(string? nameString, string directions, string description, List<ShoppingItem> shoppingItems) =>
        Result.SuccessIf(nameString != null, "Name cannot be null")
            .Ensure(() => shoppingItems.Count > 0, "Recipe must have at least one ingredient")
            .Map(() => new Recipe(nameString!, directions, description, shoppingItems));
}