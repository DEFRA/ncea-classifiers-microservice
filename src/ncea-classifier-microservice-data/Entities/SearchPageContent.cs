using Ncea.Classifier.Microservice.Data.Enums;

namespace Ncea.Classifier.Microservice.Data.Entities;

public class SearchPageContent : BaseEntity
{
    public PageContentKey Key { get; set; }
    public string Value { get; set; } = string.Empty;

    public string? ThemeCode { get; set; }
    public Theme? Theme { get; set; }

    public SearchStep Step { get; set; }
}
