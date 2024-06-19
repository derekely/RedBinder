using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain.ValueObjects;

public record ShoppingItem(Ingredient Ingredient, List<Measurement> Measurements)
{
    // public Result<ShoppingItem> Combine(Ingredient ingredient, Measurement measurement) =>
    //     Result.SuccessIf(string.Equals(Ingredient.Name, ingredient.Name, StringComparison.CurrentCultureIgnoreCase), "Ingredients must be the same")
    //         .Map(() => string.Equals(Measurements.Name, measurement.Name, StringComparison.CurrentCultureIgnoreCase) 
    //             ? new ShoppingItem(Ingredient, measurement with { Quantity = Measurements.Quantity + measurement.Quantity }) 
    //             : new ShoppingItem());
    
    // 
    public string GetMeasurementString()
    {
        if (Measurements.Count == 1)
        {
            return $"{Measurements.First().Quantity} {Measurements.First().Name} of {Ingredient.Name}";
        }
        return $"{string.Join(", ", Measurements.Select(measurement => $"{measurement.Quantity} {measurement.Name}"))} of {Ingredient.Name}";
    }
}