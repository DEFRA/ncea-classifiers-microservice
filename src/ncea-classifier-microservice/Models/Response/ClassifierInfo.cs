namespace Ncea.Classifier.Microservice.Models
{
    public class ClassifierInfo
    {        
        public string Code { get; set; } = null!;
        public string? ParentCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Level { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<ClassifierInfo> Classifiers { get; set; } = [];
    }
}
