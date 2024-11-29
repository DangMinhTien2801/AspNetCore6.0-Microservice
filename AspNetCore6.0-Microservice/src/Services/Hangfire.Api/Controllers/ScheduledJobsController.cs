using Hangfire.Api.Services;
using Hangfire.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ScheduledJob;
using System.ComponentModel.DataAnnotations;

namespace Hangfire.Api.Controllers
{
    [Route("api/scheduled-jobs")]
    [ApiController]
    public class ScheduledJobsController : ControllerBase
    {
        private readonly IBackgroundJobService _jobService;

        public ScheduledJobsController(
            IBackgroundJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("send-email-reminder-checkout-order")]
        public IActionResult SendReminderCheckoutOrderEmail([FromBody] ReminderCheckoutOrderDto model)
        {
            var jobId = _jobService.SendEmailContent(model.email, model.subject, model.emailContent, model.enqueueAt);

            return Ok();
        }

        [HttpDelete]
        [Route("delete/jobId/{id}")]
        public IActionResult DeleteJobId([Required]string id)
        {
            var result = _jobService.ScheduledJobService.Delete(id);
            return Ok(result);
        }
    }
}
