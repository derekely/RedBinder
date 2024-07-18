using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedBinder.Domain.Entities;

namespace RedBinder.Infrastructure.DatabaseContext.Configurations;

public class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder.ToTable("Measurements");

        builder.HasKey(measurements => measurements.Id);
        
        builder.Property(measurement => measurement.Name)
            .IsRequired()
            .HasDefaultValue(null)
            .HasMaxLength(100);
        
        builder.Property(measurement => measurement.Quantity)
            .IsRequired()
            .HasDefaultValue(null)
            .HasMaxLength(100);

        builder.HasMany(measurement => measurement.RecipeJoins)
            .WithOne(recipeJoin => recipeJoin.Measurement)
            .HasForeignKey(rj => rj.MeasurementId);
    }
}