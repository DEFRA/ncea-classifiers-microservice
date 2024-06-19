
namespace Ncea.Classifier.Microservice.Models.Response
{
    public class GuidedSearchClassifiersWithPageContent
    {
        public string ThemeCode { get; set; } = null!;
        public string ThemeName { get; set; } = null!;
        public string? SectionTitle { get; set; }
        public string? SectionIntroduction { get; set; }
        public int Level { get; set; }
        public List<GuidedSearchClassifier> Classifiers { get; set; } = [];
    }
}
