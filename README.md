# RedBinder

RedBinder is an application designed to manage recipes and shopping lists. It provides functionality to store, retrieve, update, and delete recipes, as well as generate shopping carts based on selected recipes. The project leverages .NET technologies with a layered architecture and Entity Framework Core for data access.

## Features

- **Recipe Management:**  
  - Create, read, update, and delete recipes.
  - Each recipe has a name, description, directions, and associated ingredients/measurements.

- **Shopping Cart Generation:**  
  - Select multiple recipes to generate a combined shopping cart of ingredients and required measurements.

- **Persistence:**  
  - Uses Entity Framework Core to interact with a SQL Server database.

## Project Structure

- `RedBinder.Domain`: Core domain entities, value objects, and DTOs (e.g., `RecipeOverview`, `ShoppingItem`, `Ingredient`, `Measurement`).
- `RedBinder.Application`: Application logic, MediatR handlers, and service interfaces for recipes and shopping carts.
- `RedBinder.Infrastructure`: Data access layer using EF Core, dependency injection setup, and repository implementations.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com)
- SQL Server instance (default connection string expects `localhost\SQLEXPRESS01`)
