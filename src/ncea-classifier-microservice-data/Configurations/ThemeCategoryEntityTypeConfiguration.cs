using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class ThemeCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ThemeCategory>
{
    public void Configure(EntityTypeBuilder<ThemeCategory> builder)
    {
        builder
            .Property(b => b.Id)
            .UseIdentityColumn()
            .HasColumnName("Id")
            .HasColumnType("int")
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(b => b.ThemeCode)
            .HasColumnOrder(2);

        builder
            .Property(b => b.CategoryCode)
            .HasColumnOrder(3);

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(4);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(5);
    }
}

