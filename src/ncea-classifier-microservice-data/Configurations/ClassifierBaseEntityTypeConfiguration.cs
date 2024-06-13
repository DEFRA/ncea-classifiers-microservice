using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;


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
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(b => b.Code)
            .HasColumnType("varchar(10)")
            .HasColumnOrder(2)
            .IsRequired();

        builder
            .Property(b => b.Name)
            .HasColumnType("varchar(50)")
            .HasColumnOrder(3)
            .IsRequired();

        builder
            .Property(b => b.Definition)
            .HasColumnType("text")
            .HasColumnOrder(4)
            .IsRequired();

        builder
            .Property(b => b.CreatedAt)
            .HasColumnOrder(5);

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnOrder(6);

        Configure(builder);
    }

    protected abstract void Configure(EntityTypeBuilder<T> builder);
}
