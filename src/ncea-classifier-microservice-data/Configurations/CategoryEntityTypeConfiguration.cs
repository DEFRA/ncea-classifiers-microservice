using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class CategoryEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<Category>
{
    protected override void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasMany(x => x.SubCategories)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryCode)
            .IsRequired(true);

        builder
            .Property(b => b.ThemeCode)
            .HasColumnOrder(5);

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(6);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(7);
    }
}
