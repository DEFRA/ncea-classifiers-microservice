using Microsoft.AspNetCore.Mvc;
using Ncea.Classifier.Microservice.Models;
using Ncea.Classifier.Microservice.Models.Response;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using AutoMapper;
using FluentValidation;

namespace Ncea.Classifier.Microservice.Controllers;

[ApiController]
[Route("api")]
public class ClassifiersController : ControllerBase
{
    private readonly IValidator<FilterCriteria> _validator;
    private readonly IClassifierService _classifierService;
    private readonly IMapper _mapper;

    public ClassifiersController(IClassifierService classifierService, IValidator<FilterCriteria> validator, IMapper mapper)
    {
        _classifierService = classifierService;
        _validator = validator;
        _mapper = mapper;
    }

    [HttpGet("vocabulary")]
    [ProducesResponseType<IEnumerable<ClassifierInfo>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    public async Task<IActionResult> GetClassifiersByLevel([FromQuery] FilterCriteria filterCriteria, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(filterCriteria);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var parentCodes = (filterCriteria.Parents != null) ? filterCriteria.Parents.Split(',').Select(x => x.Trim()).ToArray() : [];

        var result = await _classifierService.GetGuidedSearchClassifiersByLevelAndParentCodes((Domain.Enums.Level)filterCriteria.Level, parentCodes, cancellationToken);

        var classifiers = _mapper.Map<List<GuidedSearchClassifiersWithPageContent>>(result);

        return Ok(classifiers);
    }
}
