using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ncea.Classifier.Microservice.Data;
using System.Diagnostics.CodeAnalysis;

namespace Ncea.Classifier.Microservice.Extensions;

[ExcludeFromCodeCoverage]
public static class HealthCheck
{
    private static readonly string[] _tags = ["Ncea Classifiers", "Database"];

    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

        services.AddHealthChecks()
            .AddNpgSql(healthQuery: "select 1", name: "Postgre", failureStatus: HealthStatus.Unhealthy, tags: _tags)
            .AddApplicationInsightsPublisher();
    }
}
