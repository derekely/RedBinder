namespace RedBinder.Domain.Entities;

public record RecipeJoin
{
    // Used for EF Core
    public int Id { get; init; }
    
    // Properties
    public int RecipeOverviewId { get; init; }
    public int IngredientId { get; init; }
    public int MeasurementId { get; init; }
    
    // Navigation properties
    public RecipeOverview RecipeOverview { get; init; }
    public Ingredient Ingredient { get; init; }
    public Measurement Measurement { get; init; }
    
    // Constructors - I think it is used by EF Core
    public RecipeJoin() { }
}