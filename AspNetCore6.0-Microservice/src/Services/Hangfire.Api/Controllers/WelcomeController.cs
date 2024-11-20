using Contracts.ScheduledJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
        private readonly IScheduledJobService _jobService;
        private readonly ILogger<WelcomeController> _logger;

        public WelcomeController(IScheduledJobService jobService,
            ILogger<WelcomeController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }
        [HttpPost("[action]")]
        public IActionResult Welcome()
        {
            var jobId = _jobService.Enqueue(() => ResponseWelcome("Welcome to Hangfire API"));
            return Ok($"Job ID: {jobId} - Enqueue Job");
        }

        [HttpPost("[action]")]
        public IActionResult DelayWelcome()
        {
            var seconds = 5;
            var jobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"), TimeSpan.FromSeconds(seconds));
            return Ok($"Job ID: {jobId} - Enqueue Job");
        }

        [HttpPost("[action]")]
        public IActionResult WelcomeAt()
        {
            var enqueueAt = DateTimeOffset.UtcNow.AddSeconds(10);
            var jobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"), enqueueAt);
            return Ok($"Job ID: {jobId} - Enqueue Job");
        }

        [HttpPost("[action]")]
        public IActionResult ConfirmedWelcome()
        {
            const int timeInSeconds = 10;
            var parentJobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"), TimeSpan.FromSeconds(10));

            var jobId = _jobService.ContinueQueueWith(parentJobId, () => ResponseWelcome("Welcome message is sent"));

            return Ok($"Job ID: {jobId} - Confirmed Welcome will be sent in {timeInSeconds} seconds");
        }

        [NonAction]
        public void ResponseWelcome(string text)
        {
            _logger.LogInformation(text);
        }
    }
}
