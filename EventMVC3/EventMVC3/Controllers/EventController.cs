using EventMVC3.Data;
using EventMVC3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMVC3.Controllers
{
    public class EventController : Controller
    {
        private readonly EventContext _db;
        public EventController(EventContext db)
        {
            _db = db;
        }
        public IActionResult Index(string? name)
        {
            List<EventCategory> category = _db.EventCategories.ToList();
            int? id = category.Where(c => c.CategoryName == name).Select(c => c.Id).FirstOrDefault();
            IEnumerable<Event> eventList = _db.Events.Where(e => e.Category == id).ToList();
            ViewBag.n = name;
            return View(eventList);
        }

        public IActionResult eventDetails(int Id)
        {
            var eventDetails = _db.Events
        .Include(e => e.Comments)      // تحميل التعليقات المرتبطة
        .ThenInclude(c => c.User)      // تحميل بيانات المستخدم لكل تعليق
        .FirstOrDefault(e => e.Id == Id);

            if (eventDetails == null)
            {
                return NotFound();
            }

            return View(eventDetails);
            //Event ev=_db.Events.Where(x=>x.Id==Id).FirstOrDefault();
            //return View(ev);
        }

        //public IActionResult Index(string? searchTerm)
        //{
        //    var eventList = _db.Events.AsQueryable();

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        eventList = eventList.Where(e => e.Name.Contains(searchTerm) || e.City.Contains(searchTerm) || e.Discription.Contains(searchTerm));
        //    }

        //    return View(eventList.ToList());
        //}

        //public IActionResult Index(string name, string searchTerm)
        //{
        //    Console.WriteLine($"بحث عن: {searchTerm}"); // طباعة القيمة في الكونسول للتحقق
        //    List<EventCategory> category = _db.EventCategories.ToList();
        //    int? id = category.Where(c => c.CategoryName == name).Select(c => c.Id).FirstOrDefault();

        //    IEnumerable<Event> eventList = _db.Events.Where(e => e.Category == id);

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        eventList = eventList.Where(e => e.Name.Contains(searchTerm) || e.City.Contains(searchTerm) || e.Discription.Contains(searchTerm));
        //    }

        //    ViewBag.n = name;
        //    return View(eventList.ToList());
        //}


        //public IActionResult Search(string searchTerm)
        //{
        //    if (string.IsNullOrEmpty(searchTerm))
        //    {
        //        return RedirectToAction("Index"); // إذا كان البحث فارغًا، ارجع للصفحة الرئيسية
        //    }

        //    var filteredEvents = _db.Events
        //                            .Where(e => e.Name.Contains(searchTerm) || e.City.Contains(searchTerm))
        //                            .ToList();

        //    return View("Index", filteredEvents); // عرض نفس صفحة Index لكن بالنتائج المفلترة
        //}
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index"); // إذا كان البحث فارغًا، ارجع للصفحة الرئيسية
            }

            IEnumerable<Event> filteredEvents = _db.Events
                                    .Where(e => e.Name.Contains(searchTerm) ||
                                                e.City.Contains(searchTerm) ||
                                                e.Discription.Contains(searchTerm))
                                    .ToList();

            ViewBag.SearchTerm = searchTerm; // حفظ مصطلح البحث لعرضه في الفيو

            return View(filteredEvents); // عرض النتائج في صفحة Search.cshtml
        }
        public IActionResult EventDate(string searchTerm)
        {
            return View();
        }


    }
}


