using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Ncea.Classifier.Microservice.Data.Configurations;

[ExcludeFromCodeCoverage]
public class CategoryEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<Category>
{
    protected override void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasMany(x => x.SubCategories)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryCode);

        builder
            .Property(b => b.ThemeCode)
            .HasColumnOrder(5);

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(6)
            .IsRequired(false);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(7)
            .IsRequired(false);
    }
}
