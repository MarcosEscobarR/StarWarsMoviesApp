using Hangfire;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Jobs;

public class HangfireJobScheduler(IRecurringJobManager recurringJobManager, IConfiguration configuration)
{
    public void ScheduleJobs()
    {
        var cronExpression = configuration["Jobs:MovieSyncCron"] ?? "0 3 * * *";

        recurringJobManager.AddOrUpdate<MovieSyncJob>(
            "movie-sync-job",
            job => job.SyncMoviesAsync(),
            cronExpression
        );
    }
}