using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Ncea.Classifier.Microservice.Data.Services;
using Ncea.Classifier.Microservice.Data.Services.Contracts;

namespace Ncea.Classifier.Microservice.Data.Tests.Services;

public class ClassifierServiceTests : IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly IClassifierService _classifierService;

    public ClassifierServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "PostgresSqlDbInMemory2")
            .Options;

        _dbContext = new AppDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        _classifierService = new ClassifierService(_dbContext);
    }

    [Fact]
    public async Task GetAllClassifiers_WhenClassifierVocabularyExists_ReturnClassifierVocabularyWithThemeCategorySubCategory()
    {
        // Arrange
        SeedInitialData();

        // Act
        var result = await _classifierService.GetAllClassifiers(default);

        // Assert
        result.Count().Should().Be(2); // Theme
        result.First().Classifiers!.Count().Should().Be(1); // Category
        result.First().Classifiers!.First().Classifiers!.Count().Should().Be(1); // SubCategory
        result.First().Classifiers!.First().Classifiers!.First().Classifiers.Should().BeNull();
    }

    [Fact]
    public async Task GetAllClassifiers_WhenClassifierVocabularyNotExists_ReturnEmptyArray()
    {
        // Act
        var result = await _classifierService.GetAllClassifiers(default);

        // Assert
        result.Count().Should().Be(0);
    }

    [Fact]
    public async Task GetGuidedSearchClassifiersByLevelAndParentCodes_WhenLevelOneIsRequested_ReturnClassifiersForLevelOne()
    {
        // Arrange
        SeedInitialData();

        // Act
        var result = await _classifierService.GetGuidedSearchClassifiersByLevelAndParentCodes(Domain.Enums.Level.Theme, [], default);

        // Assert
        result.Count().Should().Be(2);
        result.First().ThemeCode.Should().Be("test-theme-1");
        result.First().ThemeName.Should().Be("test-theme-name-1");
        result.First().SectionTitle.Should().Be("<html>section-title-1</html>");
        result.First().SectionIntroduction.Should().Be("<html>section-introduction-1</html>");
        result.First().Classifiers.Should().BeNull();
    }

    [Fact]
    public async Task GetGuidedSearchClassifiersByLevelAndParentCodes_WhenLevelTwoIsRequestedWithoutParent_ReturnClassifiersForLevelTwo()
    {
        // Arrange
        SeedInitialData();

        // Act
        var result = await _classifierService.GetGuidedSearchClassifiersByLevelAndParentCodes(Domain.Enums.Level.Category, [], default);

        // Assert
        result.Count().Should().Be(2);
        result.First().ThemeCode.Should().Be("test-theme-1");
        result.First().ThemeName.Should().Be("test-theme-name-1");
        result.First().SectionTitle.Should().Be("<html>section-title-2</html>");
        result.First().SectionIntroduction.Should().Be("<html>section-introduction-2</html>");
        result.First().Classifiers!.Count().Should().Be(1);
        result.First().Classifiers![0].Code.Should().Be("test-category-1");

        result.Last().ThemeCode.Should().Be("test-theme-2");
        result.Last().ThemeName.Should().Be("test-theme-name-2");
        result.Last().SectionTitle.Should().BeEmpty();
        result.Last().SectionIntroduction.Should().BeEmpty();
        result.Last().Classifiers!.Count().Should().Be(1);
        result.Last().Classifiers![0].Code.Should().Be("test-category-2");
    }

    [Fact]
    public async Task GetGuidedSearchClassifiersByLevelAndParentCodes_WhenLevelTwoIsRequestedWithParent_ReturnClassifiersForLevelTwo()
    {
        // Arrange
        SeedInitialData();

        // Act
        var result = await _classifierService.GetGuidedSearchClassifiersByLevelAndParentCodes(Domain.Enums.Level.Category, ["test-theme-1"], default);

        // Assert
        result.Count().Should().Be(1);
        result.First().ThemeCode.Should().Be("test-theme-1");
        result.First().ThemeName.Should().Be("test-theme-name-1");
        result.First().SectionTitle.Should().Be("<html>section-title-2</html>");
        result.First().SectionIntroduction.Should().Be("<html>section-introduction-2</html>");
        result.First().Classifiers!.Count().Should().Be(1);
        result.First().Classifiers![0].Code.Should().Be("test-category-1");
    }

    [Fact]
    public async Task GetGuidedSearchClassifiersByLevelAndParentCodes_WhenLevelThreeIsRequestedWithoutParent_ReturnClassifiersForLevelThree()
    {
        // Arrange
        SeedInitialData();

        // Act
        var result = await _classifierService.GetGuidedSearchClassifiersByLevelAndParentCodes(Domain.Enums.Level.SubCategory, [], default);

        // Assert
        result.Count().Should().Be(2);
        result.First().ThemeCode.Should().Be("test-theme-1");
        result.First().ThemeName.Should().Be("test-theme-name-1");
        result.First().SectionTitle.Should().Be("<html>section-title-3</html>");
        result.First().SectionIntroduction.Should().Be("<html>section-introduction-3</html>");
        result.First().Classifiers!.Count().Should().Be(1);
        result.First().Classifiers![0].Code.Should().Be("test-subcategory-1");

        result.Last().ThemeCode.Should().Be("test-theme-2");
        result.Last().ThemeName.Should().Be("test-theme-name-2");
        result.Last().SectionTitle.Should().BeEmpty();
        result.Last().SectionIntroduction.Should().BeEmpty();
        result.Last().Classifiers!.Count().Should().Be(1);
        result.Last().Classifiers![0].Code.Should().Be("test-subcategory-2");
    }

    [Fact]
    public async Task GetGuidedSearchClassifiersByLevelAndParentCodes_WhenLevelThreeIsRequestedWithParent_ReturnClassifiersForLevelThree()
    {
        // Arrange
        SeedInitialData();

        // Act
        var result = await _classifierService.GetGuidedSearchClassifiersByLevelAndParentCodes(Domain.Enums.Level.SubCategory, ["test-category-1"], default);

        // Assert
        result.Count().Should().Be(1);
        result.First().ThemeCode.Should().Be("test-theme-1");
        result.First().ThemeName.Should().Be("test-theme-name-1");
        result.First().SectionTitle.Should().Be("<html>section-title-3</html>");
        result.First().SectionIntroduction.Should().Be("<html>section-introduction-3</html>");
        result.First().Classifiers!.Count().Should().Be(1);
        result.First().Classifiers![0].Code.Should().Be("test-subcategory-1");
    }

    private void SeedInitialData()
    {
        _dbContext.Themes.Add(new Entities.Theme() { Code = "test-theme-1", Name = "test-theme-name-1", Definition = "test-theme-def-1" });
        _dbContext.Themes.Add(new Entities.Theme() { Code = "test-theme-2", Name = "test-theme-name-2", Definition = "test-theme-def-2" });

        _dbContext.Categories.Add(new Entities.Category() { Code = "test-category-1", Name = "test-category-name-1", Definition = "test-category-def-1", ThemeCode = "test-theme-1" });
        _dbContext.Categories.Add(new Entities.Category() { Code = "test-category-2", Name = "test-category-name-2", Definition = "test-category-def-2", ThemeCode = "test-theme-2" });

        _dbContext.SubCategories.Add(new Entities.SubCategory() { Code = "test-subcategory-1", Name = "test-subcategory-name-1", Definition = "test-subcategory-def-1", CategoryCode = "test-category-1" });
        _dbContext.SubCategories.Add(new Entities.SubCategory() { Code = "test-subcategory-2", Name = "test-subcategory-name-2", Definition = "test-subcategory-def-2", CategoryCode = "test-category-2" });

        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionTitle, Value = "<html>section-title-1</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.One });
        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionIntroduction, Value = "<html>section-introduction-1</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.One });

        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionTitle, Value = "<html>section-title-2</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.Two });
        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionIntroduction, Value = "<html>section-introduction-2</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.Two });

        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionTitle, Value = "<html>section-title-3</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.Three });
        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionIntroduction, Value = "<html>section-introduction-3</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.Three });

        _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
