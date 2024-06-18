using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class ThemeEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<Theme>
{
    protected override void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder
            .HasMany(x => x.Categories)
            .WithOne(x => x.Theme)
            .HasForeignKey(x => x.ThemeCode)
            .IsRequired(true);

        builder
            .HasMany(x => x.SearchPageContentBlocks)
            .WithOne(x => x.Theme)
            .HasForeignKey(x => x.ThemeCode)
            .IsRequired(false);

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(5);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(6);
    }
}
