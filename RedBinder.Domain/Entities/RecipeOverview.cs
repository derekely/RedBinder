namespace RedBinder.Domain.Entities;

public record RecipeOverview
{
    public int Id { get; set; }
    public int DetailsId { get; set; }
    public int IngredientId { get; set; }
    public int MeasurementId { get; set; }
}