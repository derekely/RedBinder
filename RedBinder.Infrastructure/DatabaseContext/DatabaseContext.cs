using Microsoft.EntityFrameworkCore;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext;

public class DatabaseContext : DbContext
{
    public DbSet<RecipeJoin> RecipeOverviews { get; set; }
    public DbSet<RecipeDetails> RecipeDetails { get; set; }
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
}