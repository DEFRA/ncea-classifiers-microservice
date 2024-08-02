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
using Ncea.Classifier.Microservice.Extensions;
using Ncea.Classifier.Microservice.Mappers;
using Ncea.Classifier.Microservice.ExceptionHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
    options => { builder.Configuration.Bind("AzureAd", options); });

var Configuration = builder.Configuration;

var dbConnectionStringFromAppSettings = Configuration.GetConnectionString("DefaultConnection");

ConfigureKeyVault(builder);
ConfigureLogging(builder);
ConfigureDataServices(builder, Configuration, dbConnectionStringFromAppSettings);
ConfigureServices(builder);
builder.Services.ConfigureHealthChecks(Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddValidatorsFromAssemblyContaining<FilterCriteriaValidator>();
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

app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api/isAlive"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeyAuthMiddleware>();
});

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

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

void ConfigureDataServices(WebApplicationBuilder builder, ConfigurationManager Configuration, string? dbConnectionStringFromAppSettings)
{
    var dbConnectionString = Configuration.GetConnectionString("DefaultConnection");
    if (builder.Environment.IsDevelopment() && dbConnectionStringFromAppSettings != null)
    {
        dbConnectionString = dbConnectionStringFromAppSettings;
    }
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(dbConnectionString);
    SetUpDataSourceBuilderConfig(dataSourceBuilder);
    
    var datasource = dataSourceBuilder.Build();
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(datasource);
    });

    builder.Services.AddNpgsqlDataSource(dbConnectionString!, (Action<NpgsqlDataSourceBuilder>)(dataSourceBuilder =>
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

    builder.Services.AddScoped<IValidator<FilterCriteria>, FilterCriteriaValidator>();
    builder.Services.AddTransient<IApiKeyValidationService, ApiKeyValidationService>();
    builder.Services.AddScoped<IClassifierService, ClassifierService>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
}

[ExcludeFromCodeCoverage]
public static partial class Program { }