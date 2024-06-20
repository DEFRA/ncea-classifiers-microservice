using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class SearchPageContentEntityTypeConfiguration : IEntityTypeConfiguration<SearchPageContent>
{
    public void Configure(EntityTypeBuilder<SearchPageContent> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .Property(b => b.Id)
            .UseIdentityColumn()
            .HasColumnName("Id")
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder
            .Property(b => b.Key)
            .HasColumnType("varchar(100)")
            .HasConversion<string>()
            .HasColumnOrder(2)
            .IsRequired();

        builder
            .Property(b => b.Value)
            .HasColumnType("text")
            .HasColumnOrder(3)
            .IsRequired(false);

        builder
            .Property(b => b.ThemeCode)
            .HasColumnOrder(4)
            .IsRequired(false);

        builder
            .Property(b => b.Step)            
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