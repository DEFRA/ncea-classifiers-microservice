using Ncea.Classifier.Microservice.Services.Contracts;

namespace Ncea.Classifier.Microservice.Services;

public class ApiKeyValidationService : IApiKeyValidationService
{
    private readonly IConfiguration _configuration;

    public ApiKeyValidationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public bool IsValidApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return false;
        
        string? nceaClassifierApiKey = _configuration.GetValue<string>(Constants.ApiKeyName);
        if (nceaClassifierApiKey == null || nceaClassifierApiKey != apiKey)
            return false;

        return true;
    }
}
