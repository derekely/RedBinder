using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using RedBinder.Domain.DTOs;

namespace RedBinder.Domain.Entities;

public class RecipeOverview
{
    private RecipeOverview(string name, string directions, string description)
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
    public RecipeOverview() { }
    public ICollection<RecipeJoin> RecipeJoins { get; init; }
    
    public static Result<RecipeOverview> Create(string name, string directions, string description) =>
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(directions), "Directions cannot be null")
            .Ensure(() => !string.IsNullOrEmpty(description), "Description cannot be null")
            .Map(() => new RecipeOverview(name, directions, description));
    
    public static RecipeOverview ToRecipeDetailsFromDto(RecipeOverviewDto recipeOverviewDto) => new (recipeOverviewDto.Name, recipeOverviewDto.Directions, recipeOverviewDto.Description);
}