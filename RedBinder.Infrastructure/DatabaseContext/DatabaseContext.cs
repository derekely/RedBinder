using Microsoft.EntityFrameworkCore;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext;

public class DatabaseContext : DbContext
{
    public DbSet<RecipeJoin> RecipeJoins { get; set; }
    public DbSet<RecipeDetails> RecipeDetails { get; set; }
    // public DbSet<Measurement> Measurements { get; set; } // Do I need these?
    // public DbSet<Ingredient> Ingredients { get; set; } // Do I need these?
}