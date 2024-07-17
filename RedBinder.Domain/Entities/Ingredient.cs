using System.Collections.Generic;
using CSharpFunctionalExtensions;
using RedBinder.Domain.DTOs;

namespace RedBinder.Domain.Entities;

public class Ingredient
{
    private Ingredient(string name)
    {
        Name = name;
    }
    
    public int Id { get; }
    public string Name { get; }
    public ICollection<RecipeJoin> RecipeJoins { get; set; }
    
    // Used for EF Core
    public Ingredient() { }
    
    public static Result<Ingredient> Create(string name) =>
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Map(() => new Ingredient(name));
    
    public Ingredient ToIngredientFromDto(IngredientDto ingredientDto) => new(ingredientDto.Name);
}