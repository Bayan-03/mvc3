using Microsoft.AspNetCore.Mvc;

namespace EventMVC3.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult eventDetails() 
        {

            return View();
        }
    }
}
