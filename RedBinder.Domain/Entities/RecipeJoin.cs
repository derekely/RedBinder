namespace RedBinder.Domain.Entities;

public record RecipeJoin
{
    // Used for EF Core
    public int Id { get; set; }
    
    // Properties
    public int RecipeDetailsId { get; set; }
    public int IngredientId { get; set; }
    public int MeasurementId { get; set; }
    
    // Navigation properties
    public RecipeDetails RecipeDetails { get; set; }
    public Ingredient Ingredient { get; set; }
    public Measurement Measurement { get; set; }
    
    // Constructors
    public RecipeJoin() { }
}