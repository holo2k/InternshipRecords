using InternshipRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipRecords.Infrastructure.Persistence.Configurations;

public class DirectionConfiguration : IEntityTypeConfiguration<Direction>
{
    public void Configure(EntityTypeBuilder<Direction> builder)
    {
        builder.HasIndex(d => d.Name).IsUnique();
        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);

        builder.HasMany(d => d.Interns)
            .WithOne(i => i.Direction)
            .HasForeignKey(i => i.DirectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}