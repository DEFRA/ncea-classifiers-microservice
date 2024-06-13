namespace Ncea.Classifier.Microservice.Models.Response
{
    public class GuidedSearchClassifier
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string? ParentCode { get; set; }
        public string? ParentName { get; set; }
    }
}
