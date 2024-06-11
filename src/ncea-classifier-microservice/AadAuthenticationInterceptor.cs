using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using System.Data.Common;

namespace Ncea.Classifier.Microservice;

public class AadAuthenticationInterceptor : DbConnectionInterceptor
{
    private static readonly string[] _azurePostGreScopes = new[]
    {
        "https://ossrdbms-aad.database.windows.net/.default"
    };

    private static readonly TokenCredential _credential = new ChainedTokenCredential(
        new ManagedIdentityCredential(),
        new EnvironmentCredential());

    public override InterceptionResult ConnectionOpening(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connection.ConnectionString);
        if (DoesConnectionNeedAccessToken(connectionStringBuilder))
        {
            var tokenRequestContext = new TokenRequestContext(_azurePostGreScopes);
            var token = _credential.GetToken(tokenRequestContext, default);
            connectionStringBuilder.Password = token.Token;
        }
        connection.ConnectionString = connectionStringBuilder.ConnectionString;
        return base.ConnectionOpening(connection, eventData, result);
    }

    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connection.ConnectionString);
        if (DoesConnectionNeedAccessToken(connectionStringBuilder))
        {
            var tokenRequestContext = new TokenRequestContext(_azurePostGreScopes);
            var token = await _credential.GetTokenAsync(tokenRequestContext, cancellationToken);

            connectionStringBuilder.Password = token.Token;
        }
        connection.ConnectionString = connectionStringBuilder.ConnectionString;
        return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
    }

    private static bool DoesConnectionNeedAccessToken(NpgsqlConnectionStringBuilder connectionStringBuilder)
    {
        return connectionStringBuilder.Host!.Contains("postgres.database.azure.com", StringComparison.OrdinalIgnoreCase);
    }
}
