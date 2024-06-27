using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Ncea.Classifier.Microservice.Services;

namespace Ncea.Classifier.Microservice.Tests.Services;

public class ApiKeyValidationServiceTests
{
    private readonly ApiKeyValidationService _apiKeyValidationService;

    public ApiKeyValidationServiceTests()
    {
        var configApiKey = new Dictionary<string, string>
        {
            {"nceaClassifierMicroServiceApiKey", "api-key-xx"}
        };
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configApiKey!)
            .Build();

        _apiKeyValidationService = new ApiKeyValidationService(configuration);
    }

    [Fact]
    public void GivenIsValidApiKey_WhenApiKeyIsValid_ThenReturnTrue()
    {
        // Arrange
        var apiKey = "api-key-xx";

        // Act
        var result = _apiKeyValidationService.IsValidApiKey(apiKey);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenIsValidApiKey_WhenApiKeyIsNotValid_ThenReturnFalse()
    {
        // Arrange
        var apiKey = "api-key-xx-invalid";

        // Act
        var result = _apiKeyValidationService.IsValidApiKey(apiKey);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenIsValidApiKey_WhenApiKeyIsEmpty_ThenReturnFalse()
    {
        // Arrange
        var apiKey = string.Empty;

        // Act
        var result = _apiKeyValidationService.IsValidApiKey(apiKey);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenIsValidApiKey_WhenApiKeyIsNull_ThenReturnFalse()
    {
        // Arrange
        var apiKey = "";

        // Act
        var result = _apiKeyValidationService.IsValidApiKey(apiKey);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenIsValidApiKey_WhenApiKeyConfigurationNotAvailable_ThenReturnFalse()
    {
        // Arrange
        var apiKey = "api-key-xx";

        // Act
        var configApiKey = new Dictionary<string, string>
        {
            {"nceaClassifierMicroServiceApiKey", ""}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configApiKey!)
            .Build();

        var apiKeyValidationService = new ApiKeyValidationService(configuration);
        var result = apiKeyValidationService.IsValidApiKey(apiKey);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenIsValidApiKey_WhenApiKeyConfigurationNotExists_ThenReturnFalse()
    {
        // Arrange
        var apiKey = "api-key-xx";

        // Act
        var configApiKey = new Dictionary<string, string>();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configApiKey!)
            .Build();

        var apiKeyValidationService = new ApiKeyValidationService(configuration);
        var result = apiKeyValidationService.IsValidApiKey(apiKey);

        // Assert
        result.Should().BeFalse();
    }
}
