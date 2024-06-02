using CSharpFunctionalExtensions;

namespace RedBinder.Domain.Entities;

public class Measurement
{
    private Measurement(string name)
    {
        Name = name;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Used for EF Core
    public Measurement() { }
    
    public static Result<Measurement> Create(string name) => 
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
        .Map(() => new Measurement(name));
}