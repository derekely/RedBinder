using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public record Measurement
{
    private Measurement(string name, double quantity)
    {
        Name = name;
        Quantity = quantity;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
    
    // Used for EF Core
    public Measurement() { }
    
    public static Result<Measurement> Create(string name, double quantity) => 
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Ensure(() => quantity > 0, "Quantity must be greater than 0")
            .Map(() => new Measurement(name, quantity));
    
    
}