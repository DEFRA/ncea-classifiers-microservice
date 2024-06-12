using Ncea.Classifier.Microservice.Data.Enums;

namespace Ncea.Classifier.Microservice.Data.Entities;

public class PageContent : BaseEntity
{
    public string SectionTitle { get; set; } = string.Empty;
    public string SectionIntroduction { get; set; } = string.Empty;
    public Level Level { get; set; }
    public bool IsActive { get; set; }
}
