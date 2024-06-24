using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Ncea.Classifier.Microservice.Data.Configurations;

[ExcludeFromCodeCoverage]
public abstract class ClassifierBaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : ClassifierBaseEntity
{
    void IEntityTypeConfiguration<T>.Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasKey(c => c.Code);

        builder
            .Property(b => b.Id)
            .UseIdentityColumn()
            .HasColumnName("Id")
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder
            .Property(b => b.Code)
            .HasColumnType("varchar(10)")
            .HasColumnOrder(2);

        builder
            .Property(b => b.Name)
            .HasColumnType("varchar(500)")
            .HasColumnOrder(3)
            .IsRequired();

        builder
            .Property(b => b.Definition)
            .HasColumnType("text")
            .HasColumnOrder(4)
            .IsRequired(false);        

        Configure(builder);
    }

    protected abstract void Configure(EntityTypeBuilder<T> builder);
}
