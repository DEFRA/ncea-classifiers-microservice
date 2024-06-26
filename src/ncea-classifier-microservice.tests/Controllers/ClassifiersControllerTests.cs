using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Moq;
using Ncea.Classifier.Microservice.Controllers;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using Ncea.Classifier.Microservice.Domain.Enums;
using Ncea.Classifier.Microservice.Mappers;
using Ncea.Classifier.Microservice.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ncea.Classifier.Microservice.Validations;

namespace Ncea.Classifier.Microservice.Tests.Controllers;

public class ClassifiersControllerTests
{
    private readonly IMapper _mapper;
    private readonly IValidator<FilterCriteria> _validator;

    public ClassifiersControllerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IValidator<FilterCriteria>, FilterCriteriaValidator>();        
        // Create the ServiceProvider
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        _validator = serviceProvider.GetRequiredService<IValidator<FilterCriteria>>();

        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(configuration);
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

        var controller = new ClassifiersController(classifierServiceMock.Object, _validator, _mapper);

        // Act
        var result = await controller.GetAllClassifiers(It.IsAny<CancellationToken>());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Models.Response.ClassifierInfo>>(okResult.Value);
        var classifier = returnValue.FirstOrDefault();
        Assert.Equal("theme-name-1", classifier!.Name);
    }

    [Fact]
    public async Task GetClassifiersByLevel_ReturnsBadRequestResult_WhenLevelIdIsNotGiven()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetGuidedSearchClassifiersByLevelAndParentCodes(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.GuidedSearchClassifiersWithPageContent>());
        classifierServiceMock.Setup(x => x.AreParentCodesValid(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new ClassifiersController(classifierServiceMock.Object, _validator, _mapper);

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria(), It.IsAny<CancellationToken>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetClassifiersByLevel_ReturnsBadRequestResult_WhenLevelIdOutsideRange()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetGuidedSearchClassifiersByLevelAndParentCodes(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.GuidedSearchClassifiersWithPageContent>());
        classifierServiceMock.Setup(x => x.AreParentCodesValid(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new ClassifiersController(classifierServiceMock.Object, _validator, _mapper);

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria() { Level = 5 }, It.IsAny<CancellationToken>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetClassifiersByLevel_ReturnsBadRequestResult_WhenLevelIdZero()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetGuidedSearchClassifiersByLevelAndParentCodes(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.GuidedSearchClassifiersWithPageContent>());
        classifierServiceMock.Setup(x => x.AreParentCodesValid(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new ClassifiersController(classifierServiceMock.Object, _validator, _mapper);

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria() { Level = 0 }, It.IsAny<CancellationToken>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetClassifiersByLevel_ReturnsBadRequestResult_WhenGivenParentCodesAreNotInTheSystem()
    {
        // Arrange
        var classifierServiceMock = new Mock<IClassifierService>();
        classifierServiceMock.Setup(x => x.GetGuidedSearchClassifiersByLevelAndParentCodes(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.GuidedSearchClassifiersWithPageContent>());
        classifierServiceMock.Setup(x => x.AreParentCodesValid(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var controller = new ClassifiersController(classifierServiceMock.Object, _validator, _mapper);

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria() { Level = 0 }, It.IsAny<CancellationToken>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetClassifiersByLevel_Return200OkResult_WhenLevelIdWithInRange()
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
        classifierServiceMock.Setup(x => x.AreParentCodesValid(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new ClassifiersController(classifierServiceMock.Object, _validator, _mapper);

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria() { Level = 2 }, It.IsAny<CancellationToken>());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Models.Response.GuidedSearchClassifiersWithPageContent>>(okResult.Value);
        var classifier = returnValue.FirstOrDefault();
        Assert.Equal("test-theme-code", classifier!.ThemeCode);
    }

    [Fact]
    public async Task GetClassifiersByLevel_Return200OkResult_WhenLevelIdAndParentCodesGiven()
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
        classifierServiceMock.Setup(x => x.AreParentCodesValid(It.IsAny<Level>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new ClassifiersController(classifierServiceMock.Object, _validator, _mapper);

        // Act
        var result = await controller.GetClassifiersByLevel(new Models.FilterCriteria() { Level = (int)Level.SubCategory, Parents = "a,b,c"}, It.IsAny<CancellationToken>());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Models.Response.GuidedSearchClassifiersWithPageContent>>(okResult.Value);
        var classifier = returnValue.FirstOrDefault();
        Assert.Equal("test-theme-code", classifier!.ThemeCode);
    }
}
