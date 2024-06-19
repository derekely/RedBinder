using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSharpFunctionalExtensions;

namespace RedBinder.Domain.ValueObjects;

public record ShoppingCart(ImmutableList<ShoppingItem> shoppingItems)
{
    // Properties
    public ImmutableList<ShoppingItem> ShoppingItems { get; private set; } = shoppingItems;

    // Methods
    public static Result<ShoppingCart> Create(List<ShoppingItem> shoppingItems) =>
        Result.SuccessIf(shoppingItems.Count > 0, "Shopping cart must have at least one item")
            .Map(shoppingItems.ToImmutableList)
            .Map(immutableShoppingList => new ShoppingCart(immutableShoppingList));
    
    public ShoppingCart AddItem(ShoppingItem shoppingItem)
    {
        // See if we already have that item in the cart,
        // if so, combine the measurements
        // If we don't simply add it to the list in the shopping cart
        
        if (ShoppingItems.Any(shoppingItem => shoppingItem.Ingredient.Name == shoppingItem.Ingredient.Name))
        {
            ShoppingItem existingItem = ShoppingItems.First(shopItem => shopItem.Ingredient.Name == shoppingItem.Ingredient.Name);
            // existingItem.Combine(shoppingItem.Ingredient, shoppingItem.Measurements.First());
        }
        
        return new ShoppingCart(ShoppingItems.Add(shoppingItem)); // not this
    }
}