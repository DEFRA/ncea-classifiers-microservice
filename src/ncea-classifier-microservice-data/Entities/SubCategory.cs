namespace Ncea.Classifier.Microservice.Data.Entities;

public class SubCategory : ClassifierBaseEntity
{
    public string CategoryCode { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
