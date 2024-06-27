using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace Ncea.Classifier.Microservice.Data.Tests;

public class AppDbContextTests : IDisposable
{
    private readonly AppDbContext _dbContext;

    public AppDbContextTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "PostgresSqlDbInMemory1")
            .Options;

        _dbContext = new AppDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public void CheckAppDbContext()
    {
        // Act
        SeedInitialData();

        // Assert
        _dbContext.Themes.Count().Should().Be(2);
        _dbContext.Categories.Count().Should().Be(2);
        _dbContext.SubCategories.Count().Should().Be(2);
        _dbContext.SearchPageContentBlocks.Count().Should().Be(2);
    }

    private void SeedInitialData()
    {
        _dbContext.Themes.Add(new Entities.Theme() { Code = "test-theme-1", Name = "test-theme-name-1", Definition = "test-theme-def-1" });
        _dbContext.Themes.Add(new Entities.Theme() { Code = "test-theme-2", Name = "test-theme-name-2", Definition = "test-theme-def-2" });

        _dbContext.Categories.Add(new Entities.Category() { Code = "test-category-1", Name = "test-category-name-1", Definition = "test-category-def-1", ThemeCode = "test-theme-1" });
        _dbContext.Categories.Add(new Entities.Category() { Code = "test-category-2", Name = "test-category-name-2", Definition = "test-category-def-2", ThemeCode = "test-theme-2" });

        _dbContext.SubCategories.Add(new Entities.SubCategory() { Code = "test-subcategory-1", Name = "test-subcategory-name-1", Definition = "test-subcategory-def-1", CategoryCode = "test-category-1" });
        _dbContext.SubCategories.Add(new Entities.SubCategory() { Code = "test-subcategory-2", Name = "test-subcategory-name-2", Definition = "test-subcategory-def-2", CategoryCode = "test-category-2" });

        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionTitle, Value = "<html>test-string-1</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.One });
        _dbContext.SearchPageContentBlocks.Add(new Entities.SearchPageContent() { Key = Enums.PageContentKey.SectionIntroduction, Value = "<html>test-string-2</html>", ThemeCode = "test-theme-1", Step = Enums.SearchStep.One });

        _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();

        GC.SuppressFinalize(this);
    }
}
