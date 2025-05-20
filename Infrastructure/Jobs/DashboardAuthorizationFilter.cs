using Hangfire.Dashboard;

namespace Infrastructure.Jobs;

public class DashboardAuthorizationFilter: IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return env is "Development" or "Stage";
    }
}