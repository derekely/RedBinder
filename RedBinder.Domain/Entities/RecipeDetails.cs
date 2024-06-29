using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class RecipeDetails
{
    private RecipeDetails(int recipeId, string name, string directions, string description)
    {
        RecipeId = recipeId;
        Name = name;
        Directions = directions;
        Description = description;
    }
    
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public string Directions { get; set; }
    public string Description { get; set; }
    
    // Used for EF Core
    public RecipeDetails() { }
    
    public static Result<RecipeDetails> Create(int recipeId, string name, string directions, string description) =>
        Result.SuccessIf(recipeId > 0, "RecipeId must be greater than 0")
            .Ensure(() => !string.IsNullOrEmpty(name), "Name cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(directions), "Directions cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(description), "Description cannot be null")
            .Map(() => new RecipeDetails(recipeId, name, directions, description));
}