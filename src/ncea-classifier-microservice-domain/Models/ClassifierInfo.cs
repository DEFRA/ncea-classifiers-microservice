using Ncea.Classifier.Microservice.Domain.Enums;

namespace Ncea.Classifier.Microservice.Domain.Models;

public class ClassifierInfo
{
    public ClassifierInfo(string code, string name, Level level, string definition, List<ClassifierInfo>? children) 
    {
        Code = code;
        Name = name;
        Level = level;
        Definition = definition;
        Classifiers = children;
    }

    public string Code { get; }
    public string Name { get; }
    public Level Level { get; }
    public string Definition { get; }
    public List<ClassifierInfo>? Classifiers { get; }
}
