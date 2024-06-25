using Moq;
using Microsoft.Extensions.Logging;
using Ncea.Classifier.Microservice.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using System.Text.Json;

using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ncea.Classifier.Microservice.Tests.ExceptionHandlers;

public class GlobalExceptionHandlerTests
{
    private readonly Mock<ILogger<GlobalExceptionHandler>> _logger;
    private readonly GlobalExceptionHandler _globalExceptionHandler;

    public GlobalExceptionHandlerTests()
    {
        _logger = new Mock<ILogger<GlobalExceptionHandler>>();
        _globalExceptionHandler = new GlobalExceptionHandler(_logger.Object);
    }

    [Fact]
    public async Task TryHandleAsync_ArgumentNullException()
    {
        HttpContext ctx = new DefaultHttpContext();

        var result = await _globalExceptionHandler.TryHandleAsync(ctx, new ArgumentNullException(), default);

        result.Should().BeTrue();        
    }

    [Fact]
    public async Task TryHandleAsync_ValidationException()
    {
        HttpContext ctx = new DefaultHttpContext();

        var result = await _globalExceptionHandler.TryHandleAsync(ctx, new ValidationException("err-msg"), default);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task TryHandleAsync_UnauthorizedAccessException()
    {
        HttpContext ctx = new DefaultHttpContext();

        var result = await _globalExceptionHandler.TryHandleAsync(ctx, new UnauthorizedAccessException("err-msg"), default);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task TryHandleAsync_UnhandledException()
    {
        HttpContext ctx = new DefaultHttpContext();

        var result = await _globalExceptionHandler.TryHandleAsync(ctx, new ApplicationException("err-msg"), default);

        result.Should().BeTrue();
    }
}
