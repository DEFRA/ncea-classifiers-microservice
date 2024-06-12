namespace Ncea.Classifier.Microservice.Data.Entities;

public class SubCategory : ClassifierBaseEntity
{
    public List<Category> Categories { get; set; } = [];
}
