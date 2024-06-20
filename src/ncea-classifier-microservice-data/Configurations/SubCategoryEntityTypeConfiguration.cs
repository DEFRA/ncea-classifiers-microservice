using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class SubCategoryEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<SubCategory>
{
    protected override void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder
            .Property(b => b.CategoryCode)
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
