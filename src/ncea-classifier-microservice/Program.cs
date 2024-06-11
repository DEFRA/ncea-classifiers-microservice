using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice;
using Ncea.Classifier.Microservice.Data;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Azure.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configuration = builder.Configuration;

builder.Services.AddNpgsqlDataSource(Configuration.GetConnectionString("DefaultConnection")!, dataSourceBuilder =>
{
    dataSourceBuilder.UsePeriodicPasswordProvider(async (_, ct) =>
    {
        DefaultAzureCredential credential = new();
        TokenRequestContext ctx = new(["https://ossrdbms-aad.database.windows.net/.default"]);
        AccessToken tokenResponse = await credential.GetTokenAsync(ctx, ct);
        return tokenResponse.Token;
    }, TimeSpan.FromHours(4), TimeSpan.FromSeconds(10));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql();
    options.AddInterceptors(new AadAuthenticationInterceptor());
});

ConfigureKeyVault(builder);
ConfigureLogging(builder);
builder.Services.ConfigureHealthChecks(Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//HealthCheck Middleware
app.MapHealthChecks("/api/isAlive", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(delegate (Options options)
{
    options.UIPath = "/healthcheck-ui";
    //options.AddCustomStylesheet("./HealthCheck/Custom.css");

});

app.Run();

static void ConfigureKeyVault(WebApplicationBuilder builder)
{
    var keyVaultEndpoint = new Uri(builder.Configuration.GetValue<string>("KeyVaultUri")!);
    var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
    builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
    builder.Services.AddSingleton(secretClient);
}
static void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddApplicationInsights(
            configureTelemetryConfiguration: (config) =>
                config.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString"),
                configureApplicationInsightsLoggerOptions: (options) => { }
            );
        loggingBuilder.AddConsole();
        loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(null, LogLevel.Information);
    });
    builder.Services.AddApplicationInsightsTelemetry(o => o.EnableAdaptiveSampling = false);
    builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
    {
        module.EnableSqlCommandTextInstrumentation = true;
        o.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString");
    });
}
