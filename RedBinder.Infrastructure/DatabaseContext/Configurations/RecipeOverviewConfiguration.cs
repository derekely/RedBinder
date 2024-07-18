using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext.Configurations;

public class RecipeOverviewConfiguration : IEntityTypeConfiguration<RecipeOverview>
{
    public void Configure(EntityTypeBuilder<RecipeOverview> builder)
    {
        builder.ToTable("RecipeOverviews");

        builder.HasKey(recipeOverview => recipeOverview.Id);
        
        builder.Property(recipeOverview => recipeOverview.Name)
            .IsRequired()
            .HasDefaultValue(null)
            .HasMaxLength(100);

        builder.Property(recipeOverview => recipeOverview.Directions)
            .IsRequired()
            .HasDefaultValue(null)
            .HasMaxLength(100);
        
        builder.Property(recipeOverview => recipeOverview.Description)
            .IsRequired()
            .HasDefaultValue(null)
            .HasMaxLength(100);
        
        builder.HasMany(recipeOverview => recipeOverview.RecipeJoins)
            .WithOne(recipeJoin => recipeJoin.RecipeOverview)
            .HasForeignKey(rj => rj.RecipeOverviewId);
    }
}