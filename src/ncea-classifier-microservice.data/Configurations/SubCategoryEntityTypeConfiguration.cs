using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ncea.Classifier.Microservice.Data.Entities;

namespace Ncea.Classifier.Microservice.Data.Configurations;

public class SubCategoryEntityTypeConfiguration : ClassifierBaseEntityTypeConfiguration<SubCategory>
{
    protected override void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        
    }
}
