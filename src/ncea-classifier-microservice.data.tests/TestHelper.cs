using Microsoft.EntityFrameworkCore;

namespace Ncea.Classifier.Microservice.Data.Tests;

public class TestHelper
{
    private readonly AppDbContext appDbContext;

    public TestHelper()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase(databaseName: "PostGreDbInMemory");

        var dbContextOptions = builder.Options;
        appDbContext = new AppDbContext(dbContextOptions);

        // Delete existing db before creating a new one
        appDbContext.Database.EnsureDeleted();
        appDbContext.Database.EnsureCreated();
    }
}
