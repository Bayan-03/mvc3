using Microsoft.AspNetCore.Mvc;

namespace EventMVC3.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
