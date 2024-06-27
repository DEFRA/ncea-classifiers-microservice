using Ncea.Classifier.Microservice.Services.Contracts;
using System.Net;

namespace Ncea.Classifier.Microservice.Middlewares
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiKeyValidationService _apiKeyValidationService;

        public ApiKeyAuthMiddleware(RequestDelegate next, IApiKeyValidationService apiKeyValidationService)
        {
            _next = next;
            _apiKeyValidationService = apiKeyValidationService;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers[Constants.ApiKeyHeaderName]))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            string? apiKey = context.Request.Headers[Constants.ApiKeyHeaderName];
            if (!_apiKeyValidationService.IsValidApiKey(apiKey!))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await _next(context);
        }
    }
}
