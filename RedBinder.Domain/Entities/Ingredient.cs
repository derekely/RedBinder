using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class Ingredient
{
    private Ingredient(string name, double quantity, Measurement measurement)
    {
        Name = name;
        Quantity = quantity;
        Measurement = measurement;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
    public Measurement Measurement { get; set; }
    
    public Ingredient() { }
    
    public static Result<Ingredient> Create(string name, double quantity, Measurement measurement) =>
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Map(() => new Ingredient(name, quantity, measurement));
}