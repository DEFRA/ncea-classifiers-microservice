using Microsoft.AspNetCore.Mvc;
using Ncea.Classifier.Microservice.Models;
using Ncea.Classifier.Microservice.Models.Response;

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
    [ProducesResponseType<IEnumerable<ClassifierInfo>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAllClassifiers()
    {
        return Ok(new List<ClassifierInfo>());
    }

    
    [HttpGet("level/{LevelId}")]
    [ProducesResponseType<IEnumerable<GuidedSearchClassifiers>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetClassifiersByLevel(FilterCriteria filetrCriteria)
    {
        return Ok(new List<GuidedSearchClassifiers>());
    }
}
