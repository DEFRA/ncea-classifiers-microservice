using Microsoft.AspNetCore.Mvc;
using Ncea.Classifier.Microservice.Models;
using Ncea.Classifier.Microservice.Models.Response;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using AutoMapper;

namespace Ncea.Classifier.Microservice.Controllers;

[ApiController]
[Route("api")]
public class ClassifiersController : ControllerBase
{
    private readonly IClassifierService _classifierService;
    private readonly IMapper _mapper;
    private readonly ILogger<ClassifiersController> _logger;

    public ClassifiersController(IClassifierService classifierService, IMapper mapper, ILogger<ClassifiersController> logger)
    {
        _classifierService = classifierService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("vocabulary")]
    [ProducesResponseType<IEnumerable<ClassifierInfo>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllClassifiers(CancellationToken cancellationToken)
    {
        var result = await _classifierService.GetAllClassifiers(cancellationToken);

        var classifiers = _mapper.Map<List<ClassifierInfo>>(result);

        return Ok(classifiers);
    }
    
    [HttpGet("classifiers")]
    [ProducesResponseType<IEnumerable<GuidedSearchClassifiersWithPageContent>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassifiersByLevel([FromQuery] FilterCriteria filterCriteria, CancellationToken cancellationToken)
    {
        var parentCodes = (filterCriteria.Parents != null) ? filterCriteria.Parents.Split(',').Select(x => x.Trim()).ToArray() : [];

        var result = await _classifierService.GetGuidedSearchClassifiersByLevelAndParentCodes((Domain.Enums.Level)filterCriteria.Level, parentCodes, cancellationToken);

        var classifiers = _mapper.Map<List<GuidedSearchClassifiersWithPageContent>>(result);

        return Ok(classifiers);
    }
}
