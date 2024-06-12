using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class CategoryEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<Category>
{
    protected override void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasMany(x => x.SubCategories)
            .WithMany(x => x.Categories)
            .UsingEntity<CategorySubCategory>(
                l => l.HasOne<SubCategory>().WithMany().HasForeignKey(e => e.SubCategoryCode),
                r => r.HasOne<Category>().WithMany().HasForeignKey(e => e.CategoryCode));
    }
}
