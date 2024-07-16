using Microsoft.EntityFrameworkCore;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext;

public class DatabaseContextRedBinder(DbContextOptions<DatabaseContextRedBinder> options) : DbContext(options)
{
    public DbSet<RecipeJoin> RecipeJoins { get; set; }
    public DbSet<RecipeDetails> RecipeDetails { get; set; }
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeJoin>()
            .HasKey(rj => new { rj.RecipeDetailsId, rj.IngredientId, rj.MeasurementId });

        modelBuilder.Entity<RecipeJoin>()
            .HasOne(rj => rj.RecipeDetails)
            .WithMany(rd => rd.RecipeJoins)
            .HasForeignKey(rj => rj.RecipeDetailsId);

        modelBuilder.Entity<RecipeJoin>()
            .HasOne(rj => rj.Ingredient)
            .WithMany(i => i.RecipeJoins)
            .HasForeignKey(rj => rj.IngredientId);
        
        modelBuilder.Entity<RecipeJoin>()
            .HasOne(rj => rj.Measurement)
            .WithMany(m => m.RecipeJoins)
            .HasForeignKey(rj => rj.MeasurementId);
    }
}