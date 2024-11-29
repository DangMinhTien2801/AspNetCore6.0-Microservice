using Contracts.ScheduledJobs;
using Contracts.Services;
using Hangfire.Api.Services.Interfaces;
using Shared.Services.Email;

namespace Hangfire.Api.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IScheduledJobService _jobService;
        private readonly ISmtpEmailService _emailService;
        private readonly Serilog.ILogger _logger;

        public BackgroundJobService(
            IScheduledJobService jobService,
            ISmtpEmailService emailService,
            Serilog.ILogger logger)
        {
            _jobService = jobService;
            _emailService = emailService;
            _logger = logger;
        }

        public IScheduledJobService ScheduledJobService => _jobService;

        public string? SendEmailContent(string email, string subject, string emailContent, DateTimeOffset enqueueAt)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = email,
                Subject = subject,
                Body = emailContent,
            };

            try
            {
                var jobId = _jobService.Schedule(() => _emailService.SendEmail(emailRequest), enqueueAt);
                _logger.Information($"Send email to {email} with subject: {subject} - Job Id: {jobId}");
                return jobId;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed due to an error with the email service: {ex.Message}");
            }

            return null;
        }
    }
}
