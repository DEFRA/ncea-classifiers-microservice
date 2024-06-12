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
using Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using FluentValidation;
using Ncea.Classifier.Microservice.Models;
using Ncea.Classifier.Microservice.Validations;
using Ncea.Classifier.Microservice.Data.Services.Contracts;
using Microsoft.CodeAnalysis.Classification;
using Ncea.Classifier.Microservice.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configuration = builder.Configuration;

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration);

ConfigureKeyVault(builder);
ConfigureLogging(builder);
ConfigureDataServices(builder, Configuration);
ConfigureServices(builder);
builder.Services.ConfigureHealthChecks(Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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

//ApplyMigrations();
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

static NpgsqlDataSourceBuilder SetUpDataSourceBuilderConfig(NpgsqlDataSourceBuilder dataSourceBuilder)
{
    return dataSourceBuilder.UsePeriodicPasswordProvider(async (_, ct) =>
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
        var _Db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_Db != null)
        {
            if (_Db.Database.GetPendingMigrations().Any())
            {
                _Db.Database.Migrate();
            }
        }
    }
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IValidator<ClassifierCriteria>, ClassifierCriteriaValidator>();
    builder.Services.AddScoped<IClassifierService, ClassifierService>();
}