using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class Recipe
{
    private Recipe(string name, string directions, string description)
    {
        Name = name;
        Directions = directions;
        Description = description;
    }

    // Used for EF Core
    public Recipe() { }

    public string Name { get; set; } = null!;
    public string Directions { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public Result<Recipe> Create(string? nameString, string directions, string description) =>
        Result.SuccessIf(nameString != null, "Name cannot be null")
            .Map(() => new Recipe(nameString!, directions, description));
}