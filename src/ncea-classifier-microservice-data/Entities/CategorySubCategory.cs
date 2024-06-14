namespace Ncea.Classifier.Microservice.Data.Entities;

public class CategorySubCategory : BaseEntity
{
    public string CategoryCode { get; set; } = null!;
    public string SubCategoryCode { get; set; } = null!;
}
