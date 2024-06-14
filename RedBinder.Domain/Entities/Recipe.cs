using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class Recipe
{
    private Recipe(string name, string directions, string description, Dictionary<Measurement, List<Ingredient> > ingredientsAndAmount)
    {
        Name = name;
        Directions = directions;
        Description = description;
        IngredientsAndAmount = ingredientsAndAmount;
    }
    
    public string Name { get; set; } = null!;
    public string Directions { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Dictionary<Measurement, List<Ingredient>> IngredientsAndAmount { get; set; } = new();
    
    
    public Result<Recipe> Create(string? nameString, string directions, string description, List<Ingredient> ingredients) => //TODO think about this, with the dictionary
        Result.SuccessIf(nameString != null, "Name cannot be null")
            .Map(() => new Recipe(nameString!, directions, description, new()));
}