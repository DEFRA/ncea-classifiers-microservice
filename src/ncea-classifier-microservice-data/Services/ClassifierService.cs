using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using Ncea.Classifier.Microservice.Domain.Models;
using Ncea.Classifier.Microservice.Domain.Enums;
using Ncea.Classifier.Microservice.Data.Entities;
using Ncea.Classifier.Microservice.Data.Models;

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
            CreateThemeClassifier(theme, classifiers);
        }

        return classifiers;
    }    

    public async Task<IEnumerable<GuidedSearchClassifiersWithPageContent>> GetGuidedSearchClassifiersByLevelAndParentCodes(Level level, string[] parentCodes, CancellationToken cancellationToken)
    {
        var classifiers = await GetGuidedSearchClassifierInfo(level, parentCodes, cancellationToken);

        var distinctThemeCodes = classifiers.Select(x => x.ThemeCode).Distinct();

        var pageContentBlocks = await _dbContext.SearchPageContentBlocks
            .Where(x => x.Step == (Enums.SearchStep)level && (distinctThemeCodes != null || distinctThemeCodes!.Contains(x.ThemeCode)))
            .ToListAsync(cancellationToken);

        var classifierGroups = classifiers.GroupBy(x => new { x.ThemeCode, x.ThemeName, x.Level })
            .Select(grp => new GuidedSearchClassifiersWithPageContent
            {
                ThemeCode = grp.Key.ThemeCode,
                ThemeName = grp.Key.ThemeName,
                SectionTitle = GetSectionTitle(grp.Key.ThemeCode, pageContentBlocks),
                SectionIntroduction = GetSectionIntroduction(grp.Key.ThemeCode, pageContentBlocks),
                Level = grp.Key.Level,
                Classifiers = grp.Select(x => x).ToList()
            })
            .OrderBy(x => x.ThemeCode);

        return classifierGroups;
    }    

    private static GuidedSearchClassifiersWithPageContent CreateGuidedSearchClassifiersWithPageContent(IGrouping<ClassifierGroupCode, GuidedSearchClassifierInfo> grp, List<SearchPageContent> pageContentBlocks)
    {
        var sectionTitle = string.Empty;
        var sectionIntroduction = string.Empty;

        if(pageContentBlocks.Any(x => x.ThemeCode == grp.Key.ThemeCode))
        {
            var pageContent = pageContentBlocks.FirstOrDefault(x => x.ThemeCode == grp.Key.ThemeCode)!;
            sectionTitle = pageContent.SectionTitle;
            sectionIntroduction = pageContent.SectionIntroduction;
        }

        return new GuidedSearchClassifiersWithPageContent
        {
            ThemeCode = grp.Key.ThemeCode,
            ThemeName = grp.Key.ThemeName,
            SectionTitle = sectionTitle,
            SectionIntroduction = sectionIntroduction,
            Level = grp.Key.Level,
            Classifiers = grp.Select(x => x).ToList()
        };
    }

    private async Task<IEnumerable<GuidedSearchClassifierInfo>> GetGuidedSearchClassifierInfo(Level level, string[] parentCodes, CancellationToken cancellationToken)
    {
        var classifiers = new List<GuidedSearchClassifierInfo>();

        if (level == Level.Category)
        {
            classifiers = await _dbContext.Categories
                .Include(x => x.Theme)
                .Where(x => !parentCodes.Any() || parentCodes.Contains(x.ThemeCode))
                .Select(x => new GuidedSearchClassifierInfo(x.Code, x.Name, Level.Category, x.Definition, x.ThemeCode, x.Theme.Name, x.ThemeCode, x.Theme.Name, null))
            .ToListAsync(cancellationToken);
        }
        else if (level == Level.SubCategory)
        {
            classifiers = await _dbContext.SubCategories
                .Include(x => x.Category)
                .Where(x => !parentCodes.Any() || parentCodes.Contains(x.CategoryCode))
                .Select(x => new GuidedSearchClassifierInfo(x.Code, x.Name, Level.SubCategory, x.Definition, x.Category.Theme.Code, x.Category.Theme.Name, x.CategoryCode, x.Category.Name, null))
                .ToListAsync(cancellationToken);
        }
        else
        {
            classifiers = await _dbContext.Themes
                .Select(x => new GuidedSearchClassifierInfo(x.Code, x.Name, Level.Theme, x.Definition, x.Code, x.Name, string.Empty, string.Empty, null))
            .ToListAsync(cancellationToken);
        }

        return classifiers;
    }

    private void CreateThemeClassifier(Theme theme, List<ClassifierInfo> classifiers)
    {
        var children = new List<ClassifierInfo>();

        var classifierTheme = new ClassifierInfo(theme.Code, theme.Name, Level.Theme, theme.Definition, children);

        classifiers.Add(classifierTheme);

        if (theme.Categories != null && theme.Categories.Any())
        {
            foreach (var category in theme.Categories)
            {
                CreateCategoryClassifier(category, children);
            }
        }
    }

    private void CreateCategoryClassifier(Category category, List<ClassifierInfo> classifiers)
    {
        var children = new List<ClassifierInfo>();

        var classifierCategory = new ClassifierInfo(category.Code, category.Name, Level.Category, category.Definition, children);

        classifiers.Add(classifierCategory);

        if (category.SubCategories != null && category.SubCategories.Any())
        {
            foreach (var subCategory in category.SubCategories)
            {
                CreateSubCategoryClassifier(subCategory, children);
            }
        }
    }

    private void CreateSubCategoryClassifier(SubCategory subCategory, List<ClassifierInfo> classifiers)
    {
        var children = new List<ClassifierInfo>();

        var classifierSubCategory = new ClassifierInfo(subCategory.Code, subCategory.Name, Level.SubCategory, subCategory.Definition, children);

        classifiers.Add(classifierSubCategory);
    }

    private static string GetSectionTitle(string themeCode, List<SearchPageContent> pageContentBlocks)
    {
        var result = string.Empty;

        if (pageContentBlocks.Any(x => x.ThemeCode == themeCode))
        {
            var pageContent = pageContentBlocks.FirstOrDefault(x => x.ThemeCode == themeCode)!;
            result = pageContent.SectionTitle;
        }

        return result;
    }

    private static string GetSectionIntroduction(string themeCode, List<SearchPageContent> pageContentBlocks)
    {
        var result = string.Empty;

        if (pageContentBlocks.Any(x => x.ThemeCode == themeCode))
        {
            var pageContent = pageContentBlocks.FirstOrDefault(x => x.ThemeCode == themeCode)!;
            result = pageContent.SectionIntroduction;
        }

        return result;
    }
}
