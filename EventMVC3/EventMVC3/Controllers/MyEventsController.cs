using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventMVC3.Models;
using Microsoft.AspNetCore.Hosting;
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
            var events = _context.Events
                .Include(e => e.CategoryNavigation)
                .ToList();

            return View(events);
        }

        // GET: MyEvents/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.EventCategories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    model.Image = "/images/" + uniqueFileName;
                }

                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.EventCategories.ToList();
            return View(model);
        }

        // GET: MyEvents/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = _context.Events.Find(id);
            if (eventItem == null) return NotFound();

            ViewBag.Categories = _context.EventCategories.ToList();
            return View(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _context.Events.FindAsync(id);

                    // تحديث الحقول
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

                    if (imageFile != null)
                    {
                        if (!string.IsNullOrEmpty(existingEvent.Image))
                        {
                            var oldImagePath = Path.Combine(_env.WebRootPath, existingEvent.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                                System.IO.File.Delete(oldImagePath);
                        }

                        string uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        existingEvent.Image = "/images/" + uniqueFileName;
                    }

                    _context.Update(existingEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(model.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.EventCategories.ToList();
            return View(model);
        }

        // GET: MyEvents/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = _context.Events
                .Include(e => e.CategoryNavigation)
                .FirstOrDefault(e => e.Id == id);

            if (eventItem == null) return NotFound();

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