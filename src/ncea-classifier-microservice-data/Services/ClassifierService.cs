using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using Ncea.Classifier.Microservice.Domain.Models;
using Ncea.Classifier.Microservice.Domain.Enums;
using Ncea.Classifier.Microservice.Data.Entities;
using Ncea.Classifier.Microservice.Data.Enums;

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

        var themes = await _dbContext.Themes
            .Include(x => x.Categories)
            .ThenInclude(x => x.SubCategories)
            .ToListAsync(cancellationToken);

        foreach(var theme in themes)
        {
            BuildThemeClassifier(theme, classifiers);
        }

        return classifiers;
    }    

    public async Task<IEnumerable<GuidedSearchClassifiersWithPageContent>> GetGuidedSearchClassifiersByLevelAndParentCodes(Level level, string[] parentCodes, CancellationToken cancellationToken)
    {
        var classifiers = await GetGuidedSearchClassifierInfo(level, parentCodes, cancellationToken);

        var distinctThemeCodes = classifiers.Select(x => x.ThemeCode).Distinct();

        var pageContentBlocks = await _dbContext.SearchPageContentBlocks
            .Where(x => x.Step == (SearchStep)level && (distinctThemeCodes != null || distinctThemeCodes!.Contains(x.ThemeCode)))
            .ToListAsync(cancellationToken);

        var classifierGroups = classifiers.GroupBy(x => new { x.ThemeCode, x.ThemeName, x.Level })
            .Select(grp => new GuidedSearchClassifiersWithPageContent
            {
                ThemeCode = grp.Key.ThemeCode,
                ThemeName = grp.Key.ThemeName,
                SectionTitle = GetPageContentByTheme(grp.Key.ThemeCode, PageContentKey.SectionTitle, pageContentBlocks),
                SectionIntroduction = GetPageContentByTheme(grp.Key.ThemeCode, PageContentKey.SectionIntroduction, pageContentBlocks),
                Level = grp.Key.Level,
                Classifiers = (grp.Key.Level != Level.Theme ) ? grp.Select(x => x).ToList(): null
            })
            .OrderBy(x => x.ThemeCode);

        return classifierGroups;
    }

    private async Task<IEnumerable<GuidedSearchClassifierInfo>> GetGuidedSearchClassifierInfo(Level level, string[] parentCodes, CancellationToken cancellationToken)
    {
        if (level == Level.Category)
        {
            return await getCategoryClassifiers(parentCodes, cancellationToken);
        }
        else if (level == Level.SubCategory)
        {
            return await GetSubCategoryClassifiers(parentCodes, cancellationToken);
        }
        else
        {
            return await _dbContext.Themes
                .Select(x => new GuidedSearchClassifierInfo(x.Code, x.Name, Level.Theme, x.Definition, x.Code, x.Name, string.Empty, string.Empty, null))
                .ToListAsync(cancellationToken);
        }
    }

    private async Task<IEnumerable<GuidedSearchClassifierInfo>> GetSubCategoryClassifiers(string[] parentCodes, CancellationToken cancellationToken)
    {
        var query = _dbContext.SubCategories.AsQueryable();
        if (parentCodes.Length != 0)
        {
            query = query.Where(x => parentCodes.Contains(x.CategoryCode));
        }
        return await query
            .Include(x => x.Category)
            .Select(x => new GuidedSearchClassifierInfo(x.Code, x.Name, Level.SubCategory, x.Definition, x.Category.Theme.Code, x.Category.Theme.Name, x.CategoryCode, x.Category.Name, null))
            .ToListAsync(cancellationToken);
    }

    private async Task<IEnumerable<GuidedSearchClassifierInfo>> getCategoryClassifiers(string[] parentCodes, CancellationToken cancellationToken)
    {
        var query = _dbContext.Categories.AsQueryable();
        if (parentCodes.Length != 0)
        {
            query = query.Where(x => parentCodes.Contains(x.ThemeCode));
        }
        return await query
            .Include(x => x.Theme)
            .Select(x => new GuidedSearchClassifierInfo(x.Code, x.Name, Level.Category, x.Definition, x.ThemeCode, x.Theme.Name, x.ThemeCode, x.Theme.Name, null))
            .ToListAsync(cancellationToken);
    }

    private static void BuildThemeClassifier(Theme theme, List<ClassifierInfo> classifiers)
    {
        var hasChildren = theme.Categories.Count != 0;

        var children = hasChildren ? new List<ClassifierInfo>() : null;

        var classifierTheme = new ClassifierInfo(theme.Code, theme.Name, Level.Theme, theme.Definition, children);

        classifiers.Add(classifierTheme);

        if (theme.Categories != null && hasChildren)
        {
            foreach (var category in theme.Categories)
            {
                BuildCategoryClassifier(category, children!);
            }
        }
    }

    private static void BuildCategoryClassifier(Category category, List<ClassifierInfo> classifiers)
    {
        var hasChildren = category.SubCategories.Count != 0;

        var children = hasChildren ? new List<ClassifierInfo>() : null;

        var classifierCategory = new ClassifierInfo(category.Code, category.Name, Level.Category, category.Definition, children);

        classifiers.Add(classifierCategory);

        if (category.SubCategories != null && hasChildren)
        {
            foreach (var subCategory in category.SubCategories)
            {
                BuildSubCategoryClassifier(subCategory, children!);
            }
        }
    }

    private static void BuildSubCategoryClassifier(SubCategory subCategory, List<ClassifierInfo> classifiers)
    {
        var classifierSubCategory = new ClassifierInfo(subCategory.Code, subCategory.Name, Level.SubCategory, subCategory.Definition, null);

        classifiers.Add(classifierSubCategory);
    }

    private static string GetPageContentByTheme(string themeCode, PageContentKey contentKey ,List<SearchPageContent> pageContentBlocks)
    {
        var result = string.Empty;

        var contentBlocks = pageContentBlocks.Where(x => x.ThemeCode == themeCode && x.Key == contentKey);

        if (contentBlocks.Any())
        {
            result = contentBlocks.FirstOrDefault()!.Value;
        }

        return result;
    }
}
