using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ncea.Classifier.Microservice;

public static class HealthCheck
{
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(healthQuery: "select 1", name: "Postgre", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Ncea Classifiers", "Database" });

        services.AddHealthChecksUI(opt =>
        {
            opt.SetEvaluationTimeInSeconds(10); //time in seconds between check    
            opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
            opt.SetApiMaxActiveRequests(1); //api requests concurrency    
            opt.AddHealthCheckEndpoint("ncea classifiers api", "/api/isAlive"); //map health check api    

        })
            .AddInMemoryStorage();
    }
}
