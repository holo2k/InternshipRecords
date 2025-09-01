using InternshipRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipRecords.Infrastructure.Persistence.Configurations;

public class InternConfiguration : IEntityTypeConfiguration<Intern>
{
    public void Configure(EntityTypeBuilder<Intern> builder)
    {
        builder.HasIndex(i => i.Email).IsUnique();
        builder.HasIndex(i => i.Phone).IsUnique();

        builder.Property(i => i.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(i => i.LastName).IsRequired().HasMaxLength(100);
    }
}