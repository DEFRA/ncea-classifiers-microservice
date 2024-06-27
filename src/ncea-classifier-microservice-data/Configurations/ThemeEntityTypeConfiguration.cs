using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Ncea.Classifier.Microservice.Data.Configurations;

[ExcludeFromCodeCoverage]
public class ThemeEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<Theme>
{
    protected override void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder
            .HasMany(x => x.Categories)
            .WithOne(x => x.Theme)
            .HasForeignKey(x => x.ThemeCode);

        builder
            .HasMany(x => x.SearchPageContentBlocks)
            .WithOne(x => x.Theme)
            .HasForeignKey(x => x.ThemeCode)
            .IsRequired(false);

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(5)
            .IsRequired(false);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(6)
            .IsRequired(false);
    }
}
