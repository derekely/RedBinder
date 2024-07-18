using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext.Configurations;

public class RecipeJoinConfiguration : IEntityTypeConfiguration<RecipeJoin>
{
    public void Configure(EntityTypeBuilder<RecipeJoin> builder)
    {
        builder.ToTable("RecipeJoins");

        builder.HasKey(rj => rj.Id);

        builder.HasOne(rj => rj.Measurement)
            .WithMany(measurement => measurement.RecipeJoins)
            .HasForeignKey(rj => rj.MeasurementId);
        
        builder.HasOne(rj => rj.Ingredient)
            .WithMany(ingredient => ingredient.RecipeJoins)
            .HasForeignKey(rj => rj.IngredientId);
        
        builder.HasOne(rj => rj.RecipeOverview)
            .WithMany(recipeOverview => recipeOverview.RecipeJoins)
            .HasForeignKey(rj => rj.RecipeOverviewId);
    }
}