using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Jobs;

public static class DependencyInjection
{
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<MovieSyncJob>();
        services.AddScoped<MovieSyncJob>();
        services.AddScoped<HangfireJobScheduler>();
        
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions()
                {
                    TablesPrefix = "Hangfire"
                }));
        });

        services.AddHangfireServer();
        

        return services;
    }
    
}