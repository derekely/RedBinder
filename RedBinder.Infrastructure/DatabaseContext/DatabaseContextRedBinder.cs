using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext;

public class DatabaseContextRedBinder(DbContextOptions options) : DbContext(options)
{
    public const string SchemaName = "dbo";
    public DbSet<RecipeJoin> RecipeJoins { get; set; }
    public DbSet<RecipeOverview> RecipeOverviews { get; set; }
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}