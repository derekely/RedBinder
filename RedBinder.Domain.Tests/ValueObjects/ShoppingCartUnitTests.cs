using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using FluentAssertions;
using RedBinder.Domain.Entities;
using RedBinder.Domain.ValueObjects;

namespace RedBinder.Domain.Tests.ValueObjects;

public class ShoppingCartUnitTests
{
    private static readonly Ingredient Tomato = Ingredient.Create("Tomato").Value;
    private static readonly Ingredient Cheese = Ingredient.Create("Cheese").Value;
    private static readonly Ingredient Lettuce = Ingredient.Create("Lettuce").Value;
    private static ShoppingItem TomatoSI(double amount, string measurementName) => new(Tomato, [Measurement.Create(measurementName, amount).Value]);
    private static ShoppingItem CheeseSI(double amount, string measurementName) => new(Cheese, [Measurement.Create(measurementName, amount).Value]);
    private static ShoppingItem LettuceSI(double amount, string measurementName) => new(Lettuce, [Measurement.Create(measurementName, amount).Value]);
    
    [Fact]
    public void Create_ExpectedCreation_DistinctItems()
    {
        // Arrange
        List<ShoppingItem> givenShoppingItems = [TomatoSI(1, "kg"), CheeseSI(3, "lbs"), LettuceSI(2, "kg")];

        ImmutableList<ShoppingItem> expectedShoppingItems = [TomatoSI(1, "kg"), CheeseSI(3, "lbs"), LettuceSI(2, "kg")];
        
        // Act
        Result<ShoppingCart> shoppingCart = ShoppingCart.Create(givenShoppingItems);
        
        // Assert
        shoppingCart.Should().Succeed();
        shoppingCart.Value.ShoppingItems.Should().BeEquivalentTo(expectedShoppingItems);
    }

    [Fact]
    public void AddItem_Passes_WithDifferentItems()
    {
        // Arrange
        ShoppingCart existingShoppingCart = ShoppingCart.Create([TomatoSI(1, "kg")]).Value;
        
        ImmutableList<ShoppingItem> expectedShoppingItems = [TomatoSI(1, "kg"), CheeseSI(3, "lbs")];
        
        // Act
        Result<ShoppingCart> newShoppingCart = existingShoppingCart.ShoppingItems.AddItem(Cheese, Measurement.Create("lbs", 3).Value);
        
        // Assert
        newShoppingCart.Should().Succeed();
        newShoppingCart.Value.ShoppingItems.Should().BeEquivalentTo(expectedShoppingItems);
    }

    [Fact]
    public void AddItem_Passes_WithSameItems_DifferentMeasurements()
    {
        // Arrange
        ShoppingCart existingCart = ShoppingCart.Create([TomatoSI(1, "kg")]).Value;
        ImmutableList<ShoppingItem> expectedShoppingItems = [new ShoppingItem(Tomato, [Measurement.Create("kg", 1).Value, Measurement.Create("lbs", 2).Value])];
        
        // Act
        Result<ShoppingCart> newShoppingCart = existingCart.ShoppingItems.AddItem(Tomato, Measurement.Create("lbs", 2).Value);
        
        // Assert
        newShoppingCart.Should().Succeed();
        newShoppingCart.Value.ShoppingItems.Should().BeEquivalentTo(expectedShoppingItems);
    }

    [Fact]
    public void AddItem_Passes_WithSameItems_SameMeasurements()
    {
        // Arrange
        ShoppingCart existingCart = ShoppingCart.Create([TomatoSI(1, "kg")]).Value;
        
        ImmutableList<ShoppingItem> expectedShoppingItems = [TomatoSI(3, "kg")];
        
        // Act
        Result<ShoppingCart> newShoppingCart = existingCart.ShoppingItems.AddItem(Tomato, Measurement.Create("kg", 2).Value);
        
        // Assert
        newShoppingCart.Should().Succeed();
        newShoppingCart.Value.ShoppingItems.Should().BeEquivalentTo(expectedShoppingItems);
    }
}