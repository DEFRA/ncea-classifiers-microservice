namespace Ncea.Classifier.Microservice.Data.Entities;

public abstract class BaseEntity
{
    public int Id { get; private set; }    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
