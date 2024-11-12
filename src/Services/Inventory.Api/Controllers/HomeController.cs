using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class HomeController : ControllerBase
    {
        //[HttpGet]
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
