namespace Ncea.Classifier.Microservice.Data.Entities;

public abstract class ClassifierBaseEntity : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Definition { get; set; } = null!;
}
