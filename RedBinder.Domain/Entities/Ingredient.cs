using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class Ingredient
{
    private Ingredient(string name)
    {
        Name = name;
    }
    
    public int Id { get; }
    public string Name { get; }
    
    // Used for EF Core
    public Ingredient() { }
    
    public static Result<Ingredient> Create(string name) =>
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Map(() => new Ingredient(name));
}