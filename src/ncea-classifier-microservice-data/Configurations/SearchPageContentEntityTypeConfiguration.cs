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
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(b => b.SectionTitle)
            .HasColumnType("text")
            .HasColumnOrder(2);

        builder
            .Property(b => b.SectionIntroduction)
            .HasColumnType("text")
            .HasColumnOrder(3);

        builder
            .Property(b => b.ThemeCode)
            .HasColumnOrder(4);

        builder
            .Property(b => b.Step)
            .IsRequired()
            .HasColumnOrder(5);

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(6);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(7);
    }
}