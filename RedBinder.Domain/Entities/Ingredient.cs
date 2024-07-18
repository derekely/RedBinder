using System;
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
    
    public int Id { get; private set; }
    public string Name { get; }
    public ICollection<RecipeJoin> RecipeJoins { get; set; }
    
    // Used for EF Core
    public Ingredient() { }
    
    public static Result<Ingredient> Create(string name) =>
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Map(() => new Ingredient(name));
    
    public static Ingredient ToIngredientFromDto(IngredientDto ingredientDto) => new(ingredientDto.Name);
}

public class IngredientEqualityComparer : IEqualityComparer<Ingredient>
{
    public bool Equals(Ingredient? x, Ingredient? y) => x?.Name == y?.Name;
    public int GetHashCode(Ingredient obj) => obj.Name.GetHashCode();
}