using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class CategorySubCategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategorySubCategory>
{
    public void Configure(EntityTypeBuilder<CategorySubCategory> builder)
    {
        builder
            .Property(b => b.Id)
            .UseIdentityColumn()
            .HasColumnName("Id")
            .HasColumnType("int")
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(b => b.CategoryCode)
            .HasColumnOrder(2);

        builder
            .Property(b => b.SubCategoryCode)
            .HasColumnOrder(3);

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(4);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(5);
    }
}
