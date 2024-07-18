using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using RedBinder.Domain.DTOs;

namespace RedBinder.Domain.Entities;

public record Measurement
{
    private Measurement(string name, double quantity)
    {
        Name = name;
        Quantity = quantity;
    }
    
    public int Id { get; init; }
    public string Name { get; init; }
    public double Quantity { get; init; }
    
    // Used for EF Core
    public Measurement() { }
    public ICollection<RecipeJoin> RecipeJoins { get; init; }
    
    public static Result<Measurement> Create(string name, double quantity) => 
        Result.SuccessIf(!string.IsNullOrEmpty(name), "Name cannot be null")
            .Ensure(() => quantity > 0, "Quantity must be greater than 0")
            .Map(() => new Measurement(name, quantity));
    
    public Measurement ToMeasurementFromDto(MeasurementDto measurementDto) => new(measurementDto.Name, measurementDto.Quantity);
}

public class MeasurementEqualityComparer : IEqualityComparer<Measurement>
{
    public bool Equals(Measurement? x, Measurement? y) => x?.Name == y?.Name;
    public int GetHashCode(Measurement obj) => obj.Name.GetHashCode();
}