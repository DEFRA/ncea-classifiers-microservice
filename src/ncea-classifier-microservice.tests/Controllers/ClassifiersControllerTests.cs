using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ncea.Classifier.Microservice.Controllers;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using Ncea.Classifier.Microservice.Domain.Enums;
using Ncea.Classifier.Microservice.Mappers;

namespace Ncea.Classifier.Microservice.Tests.Controllers;

public class ClassifiersControllerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ClassifiersController>> loggerMock;

    public ClassifiersControllerTests()
    {
        loggerMock = new Mock<ILogger<ClassifiersController>>();

        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public async Task GetAllClassifiers_ReturnsBadRequestResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetAllClassifiers(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.ClassifierInfo>());

        var controller = new ClassifiersController(classifierServiceMock.Object, _mapper, loggerMock.Object);
        controller.ModelState.AddModelError("LevelId", "Required");

        // Act
        var result = await controller.GetAllClassifiers(It.IsAny<CancellationToken>());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task GetAllClassifiers_Returns200OkResult_WhenModelStateIsValid()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetAllClassifiers(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.ClassifierInfo>()
            {
                new Domain.Models.ClassifierInfo("theme-code-1", "theme-name-1", Domain.Enums.Level.Theme,"theme-def-1", null)
            });

        var controller = new ClassifiersController(classifierServiceMock.Object, _mapper, loggerMock.Object);

        // Act
        var result = await controller.GetAllClassifiers(It.IsAny<CancellationToken>());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Models.Response.ClassifierInfo>>(okResult.Value);
        var classifier = returnValue.FirstOrDefault();
        Assert.Equal("theme-name-1", classifier!.Name);
    }

    [Fact]
    public async Task GetClassifiersByLevel_ReturnsBadRequestResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetGuidedSearchClassifiersByLevelAndParentCodes(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.GuidedSearchClassifiersWithPageContent>());

        var controller = new ClassifiersController(classifierServiceMock.Object, _mapper, loggerMock.Object);
        controller.ModelState.AddModelError("LevelId", "Required");

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria(), It.IsAny<CancellationToken>());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task GetClassifiersByLevel_Return200OkResult_WhenModelStateValid()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetGuidedSearchClassifiersByLevelAndParentCodes(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.GuidedSearchClassifiersWithPageContent>()
            {
                new Domain.Models.GuidedSearchClassifiersWithPageContent()
                {
                    ThemeCode = "test-theme-code"
                }
            });

        var controller = new ClassifiersController(classifierServiceMock.Object, _mapper, loggerMock.Object);

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria(), It.IsAny<CancellationToken>());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Models.Response.GuidedSearchClassifiersWithPageContent>>(okResult.Value);
        var classifier = returnValue.FirstOrDefault();
        Assert.Equal("test-theme-code", classifier!.ThemeCode);
    }
}
