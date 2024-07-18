using RedBinder.Domain.DTOs;

namespace RedBinder.Web.Models;

public class ShoppingItemModel
{
    public string Name { get; set; } = string.Empty;
    public string Measurement { get; set; } = string.Empty;
    public double Quantity { get; set; }
    
    internal static ShoppingItemModel FromDto(ShoppingItemDto shoppingItemDto)
    {
        return new ShoppingItemModel
        {
            Name = shoppingItemDto.IngredientDto.Name,
            Measurement = shoppingItemDto.MeasurementsDto.First().Name,
            Quantity = shoppingItemDto.MeasurementsDto.First().Quantity
        };
    }
}