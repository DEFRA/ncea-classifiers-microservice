using Ncea.Classifier.Microservice.Domain.Enums;
using Ncea.Classifier.Microservice.Domain.Models;

namespace Ncea.Classifier.Microservice.Data.Services.Contracts;

public interface IClassifierService
{
    Task<IEnumerable<ClassifierInfo>> GetAllClassifiers(CancellationToken cancellationToken);

    Task<bool> AreParentCodesValid(Level level, string[] parentCodes, CancellationToken cancellationToken);

    Task<IEnumerable<GuidedSearchClassifiersWithPageContent>> GetGuidedSearchClassifiersByLevelAndParentCodes(Level level, string[] parentCodes, CancellationToken cancellationToken);
}
