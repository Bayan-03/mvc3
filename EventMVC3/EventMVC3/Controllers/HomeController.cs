using System.Diagnostics;
using EventMVC3.Data;
using EventMVC3.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventMVC3.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly EventContext _db;
        public HomeController(EventContext db)
        {
            _db = db;
        }

        public void creatList()
        {
            List<EventCategory> categories = _db.EventCategories.ToList();
            var categoryCards = categories.Select(x => new Dictionary<string, string>
            {
                {"name",x.CategoryName },
                {"icon",x.Icon },
            }).ToList();
            ViewBag.CategoriesList = categoryCards;
        }

        public IActionResult Index()
        {
            IEnumerable<Event> eventList = _db.Events.ToList();
            creatList();
            return View(eventList);
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
