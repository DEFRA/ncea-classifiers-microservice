
using Ncea.Classifier.Microservice.Domain.Enums;

namespace Ncea.Classifier.Microservice.Data.Models;

public class ClassifierGroupCode
{
    public string ThemeCode { get; set; } = null!;
    public string ThemeName { get; set; } = null!;
    public Level Level { get; set; }
}
