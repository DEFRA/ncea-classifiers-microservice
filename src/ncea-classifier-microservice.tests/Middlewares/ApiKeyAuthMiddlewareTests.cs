using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ncea.Classifier.Microservice.Middlewares;
using Ncea.Classifier.Microservice.Services;

namespace Ncea.Classifier.Microservice.Tests.Middlewares;

public class ApiKeyAuthMiddlewareTests
{
    public readonly IConfiguration _configuration;

    public ApiKeyAuthMiddlewareTests()
    {
        var configApiKey = new Dictionary<string, string>
        {
            {"nceaClassifierMicroServiceApiKey", "api-key-xx"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configApiKey!)
            .Build();
    }

    [Fact]
    public async Task GivenInvokeAsync_WhenValidApiKeyInRequestHeader_ThenResponseStatusCodeSetTo200()
    {
        // Arrange
        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Headers[Constants.ApiKeyHeaderName] = "api-key-xx";
        RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
        var apiAuthService = new ApiKeyValidationService(_configuration);
        var authMiddleware = new ApiKeyAuthMiddleware(next, apiAuthService);

        // Act
        await authMiddleware.InvokeAsync(ctx);

        // Assert
        var result = ctx.Response.StatusCode;
        result.Should().Be(200);
    }

    [Fact]
    public async Task GivenInvokeAsync_WhenEmptyApiKeyInRequestHeader_ThenResponseStatusCodeSetTo400()
    {
        // Arrange
        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Headers[Constants.ApiKeyHeaderName] = string.Empty;
        RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
        var apiAuthService = new ApiKeyValidationService(_configuration);
        var authMiddleware = new ApiKeyAuthMiddleware(next, apiAuthService);

        // Act
        await authMiddleware.InvokeAsync(ctx);

        // Assert
        var result = ctx.Response.StatusCode;
        result.Should().Be(400);
    }

    [Fact]
    public async Task GivenInvokeAsync_WhenInvalidApiKeyInRequestHeader_ThenResponseStatusCodeSetTo401()
    {
        // Arrange
        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Headers[Constants.ApiKeyHeaderName] = "api-key-invalid";
        RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
        var apiAuthService = new ApiKeyValidationService(_configuration);
        var authMiddleware = new ApiKeyAuthMiddleware(next, apiAuthService);

        // Act
        await authMiddleware.InvokeAsync(ctx);

        // Assert
        var result = ctx.Response.StatusCode;
        result.Should().Be(401);
    }
}
