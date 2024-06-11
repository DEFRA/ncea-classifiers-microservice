using Microsoft.EntityFrameworkCore;

namespace Ncea.Classifier.Microservice.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

}
