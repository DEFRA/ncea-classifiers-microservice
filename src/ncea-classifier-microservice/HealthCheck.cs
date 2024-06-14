using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ncea.Classifier.Microservice;

public static class HealthCheck
{
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(healthQuery: "select 1", name: "Postgre", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Ncea Classifiers", "Database" })
            .AddApplicationInsightsPublisher();
    }
}
