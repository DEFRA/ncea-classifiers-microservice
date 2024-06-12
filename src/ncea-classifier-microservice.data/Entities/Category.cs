namespace Ncea.Classifier.Microservice.Data.Entities;

public class Category : ClassifierBaseEntity
{
    public List<Theme> Themes { get; set; } = [];
    public List<SubCategory> SubCategories { get; set; } = [];
}
