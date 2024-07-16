using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class RecipeDetails
{
    private RecipeDetails(string name, string directions, string description)
    {
        Name = name;
        Directions = directions;
        Description = description;
    }
    
    public int Id { get; init; }
    public string Name { get; init; }
    public string Directions { get; init; }
    public string Description { get; init; }
    
    // Used by EF Core
    public ICollection<RecipeJoin> RecipeJoins { get; init; }
    public RecipeDetails() { }
    
    public static Result<RecipeDetails> Create(string name, string directions, string description) =>
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(directions), "Directions cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(description), "Description cannot be null")
            .Map(() => new RecipeDetails(name, directions, description));
}