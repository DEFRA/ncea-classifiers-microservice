namespace Ncea.Classifier.Microservice.Services.Contracts;

public interface IApiKeyValidationService
{
    bool IsValidApiKey(string apiKey);
}
