
namespace Ncea.Classifier.Microservice.Models.Response
{
    public class GuidedSearchClassifiers
    {
        public string Theme { get; set; } = null!;
        public string? SectionTitle { get; set; }
        public string? SectionIntroduction { get; set; }
        public int Level { get; set; }
        public List<GuidedSearchClassifier> Classifiers { get; set; } = [];
    }
}
