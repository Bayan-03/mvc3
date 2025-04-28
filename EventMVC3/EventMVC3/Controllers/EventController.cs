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
        [HttpGet]
        public IActionResult booking(int idnumvent)
        {

            //List<Reservation> book = _db.Reservations.ToList();
            //int? id = book.Where(c => c.IdNumber == idnumvent).Select(c => c.IdNumber).FirstOrDefault();
            //IEnumerable<Event> eventList = _db.Events.Where(e => e.Id == idnumvent).ToList();
            Event selectedEvent = _db.Events.FirstOrDefault(e => e.Id == idnumvent);
            if (selectedEvent == null)
                return NotFound();
            var reservation = new Reservation { EventId = selectedEvent.Id };


            return View(reservation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult booking(Reservation reservation)
        {


            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home"); // أو صفحة تأكيد


            //return View(reservation); // يرجع للفورم لو فيه خطأ
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


        public IActionResult EventDate()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Search(string searchTerm, string n)
        {
            List<EventCategory> category = _db.EventCategories.ToList();
            int id = category.Where(c => c.CategoryName == n).Select(c => c.Id).FirstOrDefault();

            var events = _db.Events
                .Where(e =>
                    (string.IsNullOrEmpty(searchTerm) || e.Name.Contains(searchTerm)) &&
                    (string.IsNullOrEmpty(n) || e.Category == id)) // افترضنا أن n تمثل التصنيف مثلاً
                .Select(e => new
                {
                    id = e.Id,
                    name = e.Name,
                    discription = e.Discription,
                    image = e.Image
                })
                .ToList();

            return Json(events);
        }


        [HttpPost]
        public IActionResult AddComment(int eventId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
                return RedirectToAction("eventDetails", new { Id = eventId });

            Comment newComment = new Comment
            {
                EventId = eventId,
                UserId = 1, // المستخدم الثابت
                Comment1 = commentText,
            };

            _db.Comments.Add(newComment);
            _db.SaveChanges();
            // نعيد التوجيه مع Fragment يدويًا
            return Redirect($"/Event/eventDetails/{eventId}#comments-section");
        }




    }
}


