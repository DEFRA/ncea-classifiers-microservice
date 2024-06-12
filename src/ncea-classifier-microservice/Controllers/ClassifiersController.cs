using Microsoft.AspNetCore.Mvc;
using Ncea.Classifier.Microservice.Models;

namespace Ncea.Classifier.Microservice.Controllers;

[ApiController]
[Route("api/classifiers")]
public class ClassifiersController : ControllerBase
{
    private readonly ILogger<ClassifiersController> _logger;

    public ClassifiersController(ILogger<ClassifiersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string GetAllClassifiers()
    {
        return "success";
    }
    
    [HttpGet("{levelId}")]
    public string GetClassifiersByLevel(int levelId, [FromQuery] ClassifierCriteria classifierCriteria)
    {
        return "success";
    }
}
