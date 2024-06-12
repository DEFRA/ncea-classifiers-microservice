using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Entities;
using Ncea.Classifier.Microservice.Data.Enums;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class PageContentEntityTypeConfiguration : IEntityTypeConfiguration<PageContent>
{
    public void Configure(EntityTypeBuilder<PageContent> builder)
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
            .Property(b => b.Level)
            .IsRequired()
            .HasColumnOrder(4)
            .HasConversion(v => v.ToString(), v => (Level)Enum.Parse(typeof(Level), v));

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(5);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(6);
    }
}