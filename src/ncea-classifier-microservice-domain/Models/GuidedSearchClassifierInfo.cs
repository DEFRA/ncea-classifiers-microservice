using Ncea.Classifier.Microservice.Domain.Enums;

namespace Ncea.Classifier.Microservice.Domain.Models;

public class GuidedSearchClassifierInfo : ClassifierInfo
{
    public GuidedSearchClassifierInfo(string code, string name, Level level, string? definition, string themeCode, string themeName, string? parentCode, string? parentName, List<ClassifierInfo>? children)
        : base (code, name, level, definition, children)
    {
        ThemeCode = themeCode;
        ThemeName = themeName;
        ParentCode = parentCode ?? string.Empty;
        ParentName = parentName ?? string.Empty;
    }

    public string ThemeCode { get; set; }
    public string ThemeName { get; set; }
    public string ParentCode { get; set; }
    public string ParentName { get; set; }
}
