using System.Diagnostics.CodeAnalysis;

namespace Ncea.Classifier.Microservice.Data.Entities;

[ExcludeFromCodeCoverage]
public abstract class BaseEntity
{
    public int Id { get; set; }    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
