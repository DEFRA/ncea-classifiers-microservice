using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json;
using Ncea.Classifier.Microservice.Filters;
using System.Linq;
using System.Reflection.Metadata;
using FluentAssertions;

namespace Ncea.Classifier.Microservice.Tests.Filters;

public class AddRequiredHeaderParameterTests
{
    private SchemaRepository _schemaRepository;
    private ISchemaGenerator _schemaGenerator;
    private OpenApiOperation _operation;
    private MethodInfo? _methodInfo;

    public AddRequiredHeaderParameterTests()
    {
        _schemaRepository = new SchemaRepository("document-name");
        _operation = new OpenApiOperation();

        ISerializerDataContractResolver dataContractResolver = new JsonSerializerDataContractResolver(new JsonSerializerOptions());
        _schemaGenerator = new SchemaGenerator(new SchemaGeneratorOptions(), dataContractResolver);

        _methodInfo = _schemaRepository.GetType().GetMethod("some-method");
    }

    [Fact]
    public void GivenApplyFilter_AssignsProperties_FromActionAttribute()
    {
        // Arrange
        var operationFiltercontext = new OperationFilterContext(
            new ApiDescription(),
            _schemaGenerator,
            _schemaRepository,
            _methodInfo);

        // Act
        var filter = new AddRequiredHeaderParameter();
        filter.Apply(_operation, operationFiltercontext);

        //Assert
        var result = _operation.Parameters.Any(p => p.Name == Constants.ApiKeyHeaderName);
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenApplyFilter_AssignsPropertiesWhenParametersAreNull_FromActionAttribute()
    {
        // Arrange
        var operationFiltercontext = new OperationFilterContext(
            new ApiDescription(),
            _schemaGenerator,
            _schemaRepository,
            _methodInfo);
        _operation.Parameters = null;

        // Act
        var filter = new AddRequiredHeaderParameter();
        filter.Apply(_operation, operationFiltercontext);

        //Assert
        var result = _operation.Parameters!.Any(p => p.Name == Constants.ApiKeyHeaderName);
        result.Should().BeTrue();
    }
}
