namespace Ncea.Classifier.Microservice.Data.Entities;

public class Category : ClassifierBaseEntity
{
    public string ThemeCode { get; set; } = null!;
    public Theme Theme { get; set; } = null!;
    public List<SubCategory> SubCategories { get; set; } = [];
}
