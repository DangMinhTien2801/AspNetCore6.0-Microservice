using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Saga.Orchestrator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
