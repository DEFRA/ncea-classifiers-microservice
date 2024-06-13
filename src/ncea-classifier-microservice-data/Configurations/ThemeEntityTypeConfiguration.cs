using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class ThemeEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<Theme>
{
    protected override void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder
            .HasMany(x => x.Categories)
            .WithMany(x => x.Themes)
            .UsingEntity<ThemeCategory>(
                l => l.HasOne<Category>().WithMany().HasForeignKey(e => e.CategoryCode),
                r => r.HasOne<Theme>().WithMany().HasForeignKey(e => e.ThemeCode));

        builder
            .HasMany(x => x.SearchPageContentBlocks)
            .WithOne(x => x.Theme)
            .HasForeignKey(x => x.ThemeCode)
            .IsRequired(false);
    }
}
