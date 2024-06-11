using Microsoft.AspNetCore.Mvc;

namespace Ncea.Classifier.Microservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NceaClassifiersController : ControllerBase
{
    private readonly ILogger<NceaClassifiersController> _logger;

    public NceaClassifiersController(ILogger<NceaClassifiersController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAllClassifiers")]
    public string GetAll()
    {
        return "success";
    }   
}
