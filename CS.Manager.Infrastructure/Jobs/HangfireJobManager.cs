using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.Infrastructure.Jobs
{
    /// <inheritdoc cref="IJobManager" />
    public class HangfireJobManager : IJobManager
    {
        /// <inheritdoc />
        public string Enqueue<TJob, TArgs>(TArgs args, TimeSpan? delay = null, string continueWith = null) where TJob : IJob<TArgs>
        {
            string jobId;

            if (!string.IsNullOrEmpty(continueWith))
            {
                jobId = BackgroundJob.ContinueJobWith<TJob>(continueWith, job => job.Execute(args));
            }
            else if (delay.HasValue && delay.Value > TimeSpan.Zero)
            {
                jobId = BackgroundJob.Schedule<TJob>(job => job.Execute(args), delay.Value);
            }
            else
            {
                jobId = BackgroundJob.Enqueue<TJob>(job => job.Execute(args));
            }

            return jobId;
        }

        /// <inheritdoc />
        public string Enqueue<TJob>(Expression<Func<TJob, Task>> methodCall, TimeSpan? delay = null, string continueWith = null)
        {
            string jobId;

            if (!string.IsNullOrEmpty(continueWith))
            {
                jobId = BackgroundJob.ContinueJobWith<TJob>(continueWith, methodCall);
            }
            else if (delay.HasValue && delay.Value > TimeSpan.Zero)
            {
                jobId = BackgroundJob.Schedule<TJob>(methodCall, delay.Value);
            }
            else
            {
                jobId = BackgroundJob.Enqueue<TJob>(methodCall);
            }

            return jobId;
        }

        /// <inheritdoc />
        public bool Delete(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            var successfulDeletion = BackgroundJob.Delete(jobId);
            return successfulDeletion;
        }
    }
}
