using Ncea.Classifier.Microservice.Domain.Enums;
using Ncea.Classifier.Microservice.Domain.Models;

namespace Ncea.Classifier.Microservice.Data.Services.Contracts;

public interface IClassifierService
{
    Task<IEnumerable<ClassifierInfo>> GetAllClassifiers(CancellationToken cancellationToken);

    Task<ClassifierInfo> GetClassifierByLevelAndCode(Level level, string code, CancellationToken cancellationToken);
    Task<ClassifierInfo> GetClassifierByLevelAndCodeWithChildren(Level level, string code, CancellationToken cancellationToken);

    Task<IEnumerable<ClassifierInfo>> GetClassifiersByLevel(Level level, CancellationToken cancellationToken);
    Task<IEnumerable<ClassifierInfo>> GetClassifiersByLevelWithChildren(Level level, CancellationToken cancellationToken);
}
