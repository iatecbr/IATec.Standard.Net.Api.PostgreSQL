using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Configurations.Options;
using Persistence.Context;

namespace Persistence.Configurations.Extensions;

public static class DatabaseExtension
{
    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var efOption = configuration.GetSection(EntityFrameworkOption.Key).Get<EntityFrameworkOption>();

        if (efOption is null)
            throw new ArgumentNullException($"{nameof(EntityFrameworkOption)} cannot be null!");

        var postgresOption = configuration.GetSection(PostgreSqlOption.Key).Get<PostgreSqlOption>();

        var readConnectionString = postgresOption!.GetConnectionString();
        services.AddDbContext<ReadDataContext>((_, options) => options
            .UseNpgsql(readConnectionString)
            .UseSnakeCaseNamingConvention()
            .EnableSensitiveDataLogging(efOption.SensitiveDataLogging)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution));

        var writeConnectionString = postgresOption.GetConnectionString(false);
        services.AddDbContext<WriteDataContext>((_, options) => options
            .EnableSensitiveDataLogging(efOption.SensitiveDataLogging)
            .UseNpgsql(writeConnectionString)
            .UseSnakeCaseNamingConvention());
    }
}