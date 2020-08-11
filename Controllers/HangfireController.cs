using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hangfire;

namespace hangfire_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome to our app"));

            return Ok($"Job ID: {jobId}. Welcome email sent to the user!");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            int waitTime = 30;
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Welcome to our app"), TimeSpan.FromSeconds(waitTime));

            return Ok($"Job ID: {jobId}. Discount email will be sent in {waitTime} seconds!");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated"), Cron.Minutely);
            return Ok("Database check job initiated!");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            int waitTime = 30;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You have asked to be unsubscribed!"), TimeSpan.FromSeconds(waitTime));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribed!"));

            return Ok("Confirmation job created!");
        }
    }
}
