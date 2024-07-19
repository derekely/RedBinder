using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Domain;

public static class Extensions
{
    public static Result<Measurement> AddSameMeasurement(this Measurement measurement1, Measurement measurement2) =>
        Result.SuccessIf(string.Equals(measurement1.Name, measurement2.Name, StringComparison.CurrentCultureIgnoreCase), "Measurements must be the same")
            .Bind(() => Measurement.Create(measurement1.Name, measurement1.Quantity + measurement2.Quantity));

    public static string ToString(this Exception e) => $"Exception type: '{e.GetType()}' with message: '{e.Message}'";
    
    public static Result<ShoppingCart> AddItem(this ImmutableList<ShoppingItem> shoppingItems, Ingredient ingredient, Measurement measurement)
    {
        ShoppingItem? existingItem = shoppingItems.FirstOrDefault(shopItem => string.Equals(shopItem.Ingredient.Name, ingredient.Name, StringComparison.CurrentCultureIgnoreCase));
    
        List<ShoppingItem> newShoppingList = [..shoppingItems];
    
        return existingItem != null
            ? existingItem.Combine(ingredient, measurement).Map(combinedItem => new ShoppingCart([..newShoppingList.Where(item => item != existingItem), combinedItem])) // Same Ingredient
            : ShoppingItem.Create(ingredient, [measurement]).Map(shoppingItem => new ShoppingCart([..newShoppingList, shoppingItem])); // Different Ingredient
    }
}