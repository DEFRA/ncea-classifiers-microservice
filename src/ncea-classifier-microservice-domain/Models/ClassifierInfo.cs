using Ncea.Classifier.Microservice.Domain.Enums;

namespace Ncea.Classifier.Microservice.Domain.Models;

public class ClassifierInfo
{
    public ClassifierInfo(string code, string name, Level level, string description, string? parentCode, string? parentName, Level? parentLevel, List<ClassifierInfo>? children) 
    {
        Code = code;
        Name = name;
        Level = level;
        Description = description;
        ParentCode = parentCode ?? string.Empty;
        ParentName = parentName ?? string.Empty;
        ParentLevel = parentLevel;
        Classifiers = children ?? [];
    }

    public string Code { get; }
    public string Name { get; }
    public Level Level { get; }
    public string Description { get; }
    public List<ClassifierInfo> Classifiers { get; }

    public string ParentCode { get; set; }
    public string ParentName { get; set; }
    public Level? ParentLevel { get; set; }
}
