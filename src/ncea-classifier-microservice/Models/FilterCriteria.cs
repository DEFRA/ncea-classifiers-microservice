using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ncea.Classifier.Microservice.Models;

public class FilterCriteria
{    
    [BindRequired]
    public int Level { get; set; }

    public string? Parents { get; set; }
}
