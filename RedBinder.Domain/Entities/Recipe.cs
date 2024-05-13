using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class Recipe // TODO: IRecipe??
{
    private Recipe(List<string> ingredients, List<string> directions, Photo photo)
    {
        Ingredients = ingredients;
        Directions = directions;
        Photo = photo;
    }

    // Used for EF Core
    public Recipe() { }

    public List<string> Ingredients { get; set; } = null; // TODO: Ingredients clas!
    public List<string> Directions { get; set; } = null; // TODO: think about making this into a class. EF core guards against SQL injection, but result of these?
    public Photo Photo { get; }

    // TODO: Fill this out!
    public Result<Recipe> Create() => Result.Success(new Recipe());

}