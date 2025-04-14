using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventMVC3.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using EventMVC3.Data;

namespace EventMVC3.Controllers
{
    public class MyEventsController : Controller
    {
        private readonly EventContext _context;
        private readonly IWebHostEnvironment _env;

        public MyEventsController(EventContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: MyEvents
        public IActionResult Index()
        {
            var events = _context.Events.Include(e => e.CategoryNavigation).ToList();
            return View(events);
        }

        // GET: MyEvents/Create
        //public IActionResult Create()
        //{
        //    ViewBag.Categories = _context.EventCategories.ToList();
        //    return View(new Event());
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Event model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var imageFile = Request.Form.Files["imageFile"];

        //            if (imageFile != null && imageFile.Length > 0)
        //            {
        //                var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
        //                if (!Directory.Exists(uploadsFolder))
        //                    Directory.CreateDirectory(uploadsFolder);

        //                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        //                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //                await using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await imageFile.CopyToAsync(stream);
        //                }

        //                model.Image = "/images/" + uniqueFileName;
        //            }

        //            _context.Events.Add(model);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "حدث خطأ: " + ex.Message);
        //        }
        //    }

        //    ViewBag.Categories = _context.EventCategories.ToList();
        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.EventCategories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.EventCategories.ToList();
                return View(model);
            }

            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.Image = "/Images/" + fileName;
            }

            _context.Events.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: MyEvents/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = _context.Events.Find(id);
            if (eventItem == null)
                return NotFound();

            ViewBag.Categories = _context.EventCategories.ToList();
            return View(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var imageFile = Request.Form.Files["imageFile"];
                    var existingEvent = await _context.Events.FindAsync(id);

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        if (!string.IsNullOrEmpty(existingEvent.Image))
                        {
                            var oldImagePath = Path.Combine(_env.WebRootPath, existingEvent.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                                System.IO.File.Delete(oldImagePath);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        await using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        existingEvent.Image = "/images/" + uniqueFileName;
                    }

                    // تحديث الخصائص
                    existingEvent.Name = model.Name;
                    existingEvent.StartDate = model.StartDate;
                    existingEvent.FinishDate = model.FinishDate;
                    existingEvent.StartTime = model.StartTime;
                    existingEvent.FineshTime = model.FineshTime;
                    existingEvent.PlaceName = model.PlaceName;
                    existingEvent.City = model.City;
                    existingEvent.Discription = model.Discription;
                    existingEvent.Category = model.Category;
                    existingEvent.Price = model.Price;
                    existingEvent.ConstraintAge = model.ConstraintAge;

                    _context.Update(existingEvent);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(model.Id))
                        return NotFound();
                    throw;
                }
            }

            ViewBag.Categories = _context.EventCategories.ToList();
            return View(model);
        }

        // GET: MyEvents/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = _context.Events
                .Include(e => e.CategoryNavigation)
                .FirstOrDefault(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // GET: MyEvents/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = _context.Events
                .Include(e => e.CategoryNavigation)
                .FirstOrDefault(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                if (!string.IsNullOrEmpty(eventItem.Image))
                {
                    var imagePath = Path.Combine(_env.WebRootPath, eventItem.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
