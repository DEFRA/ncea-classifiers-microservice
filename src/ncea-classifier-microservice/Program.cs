using Microsoft.EntityFrameworkCore;
using Ncea.Classifier.Microservice.Data;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Azure.Core;
using Npgsql;
using FluentValidation;
using Ncea.Classifier.Microservice.Models;
using Ncea.Classifier.Microservice.Validations;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using Ncea.Classifier.Microservice.Data.Services;
using Ncea.Classifier.Microservice.Services.Contracts;
using Ncea.Classifier.Microservice.Services;
using Ncea.Classifier.Microservice.Middlewares;
using Ncea.Classifier.Microservice.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Net.Http.Headers;
using Ncea.Classifier.Microservice.Extensions;
using Ncea.Classifier.Microservice.Mappers;
using Ncea.Classifier.Microservice.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

ConfigureKeyVault(builder);
ConfigureLogging(builder);
ConfigureDataServices(builder, Configuration);
ConfigureServices(builder);
builder.Services.ConfigureHealthChecks(Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(o => o.OperationFilter<AddRequiredHeaderParameter>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/api/isAlive", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/classifiers"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeyAuthMiddleware>();
});

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigrations();

await app.RunAsync();

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

static void ConfigureDataServices(WebApplicationBuilder builder, ConfigurationManager Configuration)
{
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(Configuration.GetConnectionString("DefaultConnection"));
    SetUpDataSourceBuilderConfig(dataSourceBuilder);
    
    var datasource = dataSourceBuilder.Build();
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(datasource);
    });

    builder.Services.AddNpgsqlDataSource(Configuration.GetConnectionString("DefaultConnection")!, (Action<NpgsqlDataSourceBuilder>)(dataSourceBuilder =>
    {
        SetUpDataSourceBuilderConfig(dataSourceBuilder);
    }));    
}

static void SetUpDataSourceBuilderConfig(NpgsqlDataSourceBuilder dataSourceBuilder)
{
    dataSourceBuilder.UsePeriodicPasswordProvider(async (_, ct) =>
    {
        DefaultAzureCredential credential = new();
        TokenRequestContext ctx = new(["https://ossrdbms-aad.database.windows.net/.default"]);
        AccessToken tokenResponse = await credential.GetTokenAsync(ctx, ct);
        return tokenResponse.Token;
    }, TimeSpan.FromHours(4), TimeSpan.FromSeconds(10));
}
void ApplyMigrations()
{

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if(dbContext != null)
        {
            dbContext.Database.Migrate();
        }
    }
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressInferBindingSourcesForParameters = true;
    });

    builder.Services.AddTransient<IApiKeyValidationService, ApiKeyValidationService>();
    builder.Services.AddScoped<IValidator<FilterCriteria>, FilterCriteriaValidator>();
    builder.Services.AddScoped<IClassifierService, ClassifierService>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
}

[ExcludeFromCodeCoverage]
public static partial class Program { }