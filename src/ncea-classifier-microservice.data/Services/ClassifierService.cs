using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using Ncea.Classifier.Microservice.Domain.Models;
using Ncea.Classifier.Microservice.Domain.Enums;

namespace Ncea.Classifier.Microservice.Data.Services;

public class ClassifierService : IClassifierService
{
    private readonly AppDbContext _dbContext;

    public ClassifierService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ClassifierInfo>> GetAllClassifiers(CancellationToken cancellationToken)
    {
        var classifiers = new List<ClassifierInfo>();

        var result = await _dbContext.Themes
            .Include(x => x.Categories)
            .ThenInclude(x => x.SubCategories)
            .ToListAsync(cancellationToken);

        return classifiers;
    }

    public async Task<ClassifierInfo> GetClassifierByLevelAndCode(Level level, string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ClassifierInfo> GetClassifierByLevelAndCodeWithChildren(Level level, string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ClassifierInfo>> GetClassifiersByLevel(Level level, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ClassifierInfo>> GetClassifiersByLevelWithChildren(Level level, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
