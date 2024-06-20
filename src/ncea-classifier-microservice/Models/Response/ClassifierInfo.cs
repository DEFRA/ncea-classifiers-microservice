namespace Ncea.Classifier.Microservice.Models.Response
{
    public class ClassifierInfo
    {        
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Level { get; set; }        
        public List<ClassifierInfo> Classifiers { get; set; } = [];
    }
}
