namespace Ncea.Classifier.Microservice.Models
{
    public class Classifier
    {        
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public List<Classifier> Classifiers { get; set; } = [];

        public string? ParentCode { get; set; }
        public string? ParentName { get; set; }
    }
}
