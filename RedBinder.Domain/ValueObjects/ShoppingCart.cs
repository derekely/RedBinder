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
    public static Result<ShoppingCart> Create(List<ShoppingItem> shoppingItems) =>
        Result.SuccessIf(shoppingItems.Count > 0, "Shopping cart must have at least one item")
            .Map(shoppingItems.ToImmutableList) // TODO: have this join the items together
            .Map(immutableShoppingList => new ShoppingCart(immutableShoppingList));
    
    public Result<ShoppingCart> AddItem(Ingredient ingredient, Measurement measurement)
    {
        ShoppingItem? existingItem = ShoppingItems.FirstOrDefault(shopItem => string.Equals(shopItem.Ingredient.Name, ingredient.Name, StringComparison.CurrentCultureIgnoreCase));
    
        List<ShoppingItem> newShoppingList = [..ShoppingItems];
    
        return existingItem != null
            ? existingItem.Combine(ingredient, measurement).Bind(combinedItem => Create([..newShoppingList.Where(item => item != existingItem), combinedItem])) // Same Ingredient
            : ShoppingItem.Create(ingredient, [measurement]).Bind(shoppingItem => Create([..newShoppingList, shoppingItem])); // Different Ingredient
    }
}