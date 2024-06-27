using Ncea.Classifier.Microservice.Domain.Enums;

namespace Ncea.Classifier.Microservice.Domain.Models;

public class GuidedSearchClassifiersWithPageContent
{
    public string ThemeCode { get; set; } = null!;
    public string ThemeName { get; set; } = null!;
    public string? SectionTitle { get; set; }
    public string? SectionIntroduction { get; set; }
    public Level Level { get; set; }
    public List<GuidedSearchClassifierInfo>? Classifiers { get; set; }
}
