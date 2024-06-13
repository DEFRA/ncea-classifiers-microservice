using Microsoft.AspNetCore.Mvc;

namespace Ncea.Classifier.Microservice.Models;

public class FilterCriteria
{
    [FromRoute]
    public int LevelId { get; set; }

    [FromQuery]
    public string? Parents { get; set; }
}
