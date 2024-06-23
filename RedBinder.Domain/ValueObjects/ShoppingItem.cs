using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain.ValueObjects;

public record ShoppingItem(Ingredient Ingredient, List<Measurement> Measurements)
{
    public static Result<ShoppingItem> Create(Ingredient ingredient, List<Measurement> measurements) =>
        Result.SuccessIf(measurements.Count > 0, "Must have at least one measurement")
            .Map(() => new ShoppingItem(ingredient, measurements));

    public Result<ShoppingItem> Combine(Ingredient ingredient2, Measurement measurement2) =>
        Result.SuccessIf(string.Equals(Ingredient.Name, ingredient2.Name, StringComparison.CurrentCultureIgnoreCase), "Ingredients must be the same")
            .Bind(() =>
            {
                Measurement? existingMeasurement = Measurements.FirstOrDefault(measurement =>
                    string.Equals(measurement.Name, measurement2.Name, StringComparison.OrdinalIgnoreCase));

                if (existingMeasurement == null)
                    return Create(Ingredient, [..Measurements, measurement2]); // Not Existing Measurement

                return existingMeasurement.AddSameMeasurement(measurement2).Bind(updatedMeasurements =>
                    Create(Ingredient, [..Measurements.Where(m => m != existingMeasurement), updatedMeasurements]));
            });
    
    public string GetMeasurementString() =>
        Measurements.Count == 1 
            ? $"{Measurements.First().Quantity} {Measurements.First().Name} of {Ingredient.Name}" 
            : $"{string.Join(", ", Measurements.Select(measurement => $"{measurement.Quantity} {measurement.Name}"))} of {Ingredient.Name}";
}