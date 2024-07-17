using System.Collections.Generic;
using System.Linq;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Domain.DTOs;

public class ShoppingItemDto(IngredientDto ingredientDto, List<MeasurementDto> measurementsDto)
{
    public IngredientDto IngredientDto => ingredientDto;
    public List<MeasurementDto> MeasurementsDto => measurementsDto;
    
    public ShoppingItemDto(ShoppingItem shoppingItem) : this(new IngredientDto(shoppingItem.Ingredient),
        shoppingItem.Measurements.Select(measurement => new MeasurementDto(measurement)).ToList()) { }

    public string GetMeasurementString() =>
        measurementsDto.Count == 1 
            ? $"{measurementsDto.First().Quantity} {measurementsDto.First().Name} of {ingredientDto.Name}" 
            : $"{string.Join(", ", measurementsDto.Select(measurement => $"{measurement.Quantity} {measurement.Name}"))} of {ingredientDto.Name}";
}