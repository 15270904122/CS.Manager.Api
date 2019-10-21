using CS.Manager.Infrastructure.Jobs;
using CS.Manager.Infrastructure.Jobs.HangfireJobExtensions;
using Hangfire.Server;
using System;
using System.Threading.Tasks;

namespace CS.Manager.Job.RecurringJobs
{

    public class TestJob : IRecurringJob
    {
        public async Task Execute(PerformContext context)
        {
           // throw new Exception($"错误:{context.BackgroundJob.Id}");
        }
    }
}
