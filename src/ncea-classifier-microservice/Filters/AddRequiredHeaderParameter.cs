using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ncea.Classifier.Microservice.Filters;

public class AddRequiredHeaderParameter : IOperationFilter   // as a nested class in script config file.
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {            
            Name = Constants.ApiKeyHeaderName,
            In = ParameterLocation.Header,
            Description = "Provide Api Key",
            Required = true
        });
    }
}
