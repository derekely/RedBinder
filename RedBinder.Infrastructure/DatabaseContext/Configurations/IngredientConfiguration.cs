using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable("Ingredients");

        builder.HasKey(ingredient => ingredient.Id);
        
        builder.Property(ingredient => ingredient.Name)
            .IsRequired()
            .HasDefaultValue(null)
            .HasMaxLength(100);

        builder.HasMany(ingredient => ingredient.RecipeJoins)
            .WithOne(recipeJoin => recipeJoin.Ingredient)
            .HasForeignKey(rj => rj.IngredientId);
    }
}