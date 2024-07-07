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
    
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public string Directions { get; set; }
    public string Description { get; set; }
    public ICollection<RecipeJoin> RecipeJoins { get; set; }
    
    // Used for EF Core
    public RecipeDetails() { }
    
    public static Result<RecipeDetails> Create(string name, string directions, string description) =>
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(directions), "Directions cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(description), "Description cannot be null")
            .Map(() => new RecipeDetails(name, directions, description));
}