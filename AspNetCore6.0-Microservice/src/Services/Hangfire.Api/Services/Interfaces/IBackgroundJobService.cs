using Contracts.ScheduledJobs;

namespace Hangfire.Api.Services.Interfaces
{
    public interface IBackgroundJobService
    {
        IScheduledJobService ScheduledJobService { get; }
        string SendEmailContent(string email, string subject, string emailContent, DateTimeOffset enqueueAt);
    }
}
