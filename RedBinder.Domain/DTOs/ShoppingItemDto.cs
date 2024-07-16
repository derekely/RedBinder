using System.Collections.Generic;
using System.Linq;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Domain.DTOs;

public class ShoppingItemDto(IngredientDto IngredientDto, List<MeasurementDto> MeasurementsDto)
{
    public ShoppingItemDto(ShoppingItem shoppingItem) : this(new IngredientDto(shoppingItem.Ingredient),
        shoppingItem.Measurements.Select(measurement => new MeasurementDto(measurement)).ToList()) { }

    public string GetMeasurementString() =>
        MeasurementsDto.Count == 1 
            ? $"{MeasurementsDto.First().Quantity} {MeasurementsDto.First().Name} of {IngredientDto.Name}" 
            : $"{string.Join(", ", MeasurementsDto.Select(measurement => $"{measurement.Quantity} {measurement.Name}"))} of {IngredientDto.Name}";
}