namespace Ncea.Classifier.Microservice.Models.Response
{
    public class GuidedSearchClassifier
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Definition { get; set; }
        public List<GuidedSearchClassifier>? Classifiers { get; set; }
    }
}
