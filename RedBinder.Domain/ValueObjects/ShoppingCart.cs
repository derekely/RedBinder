using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain.ValueObjects;

public record ShoppingCart(ImmutableList<ShoppingItem> ShoppingItems)
{
    // Methods
    public static Result<ShoppingCart> Create(List<ShoppingItem> shoppingItems)
    {
        var combinedList = new ShoppingCart(ImmutableList<ShoppingItem>.Empty);

        return Result.SuccessIf(shoppingItems.Count > 0, "Shopping cart must have at least one item")
            .Tap(() => shoppingItems.ForEach(si =>
            {
                combinedList = combinedList.ShoppingItems.AddItem(si.Ingredient, si.Measurements.First()).Value;
            }))
            .Map(() => combinedList);
    }
}