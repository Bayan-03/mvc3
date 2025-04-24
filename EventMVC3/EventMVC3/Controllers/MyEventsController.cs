using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventMVC3.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using EventMVC3.Data;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        //[HttpPost]
        //public async Task<IActionResult> Create(Event model, IFormFile imageFile)
        //{

        //        ViewBag.Categories = _context.EventCategories.ToList();
        //        return View(model);


        //    if (imageFile != null)
        //    {
        //        var fileName = Path.GetFileName(imageFile.FileName);
        //        var filePath = Path.Combine("wwwroot/Images", fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await imageFile.CopyToAsync(stream);
        //        }

        //        model.Image = "/Images/" + fileName;
        //    }

        //    _context.Events.Add(model);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}


        [HttpPost]
        public async Task<IActionResult> Create(Event model, IFormFile imageFile)
        {
            
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
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.EventCategories.ToList();
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var categories = await _context.EventCategories.ToListAsync();

            

            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName", eventItem.Category);

            return View(eventItem);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            if (id != model.Id)
                return NotFound();
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"❌ خطأ في الحقل '{key}': {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.Id == model.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            // ✅ هذا ضروري جداً هنا!
            var categories = await _context.EventCategories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName", model.Category);

            return View(model);
        }


        // التحقق من وجود الحدث بناءً على ID















        //[HttpPost]
        //public async Task<IActionResult> Edit(Event model, IFormFile imageFile)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Categories = _context.EventCategories.ToList();
        //        return View(model);
        //    }

        //    var existingEvent = await _context.Events.FindAsync(model.Id);
        //    if (existingEvent == null)
        //        return NotFound();

        //    if (imageFile != null && imageFile.Length > 0)
        //    {
        //        var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
        //        if (!Directory.Exists(uploadsFolder))
        //            Directory.CreateDirectory(uploadsFolder);

        //        // حذف القديمة
        //        if (!string.IsNullOrEmpty(existingEvent.Image))
        //        {
        //            var oldPath = Path.Combine(_env.WebRootPath, existingEvent.Image.TrimStart('/'));
        //            if (System.IO.File.Exists(oldPath))
        //                System.IO.File.Delete(oldPath);
        //        }

        //        var uniqueName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        //        var filePath = Path.Combine(uploadsFolder, uniqueName);
        //        using var stream = new FileStream(filePath, FileMode.Create);
        //        await imageFile.CopyToAsync(stream);

        //        existingEvent.Image = "/images/" + uniqueName;
        //    }

        //    // تحديث البيانات
        //    existingEvent.Name = model.Name;
        //    existingEvent.Category = model.Category;
        //    existingEvent.StartDate = model.StartDate;
        //    existingEvent.FinishDate = model.FinishDate;
        //    existingEvent.StartTime = model.StartTime;
        //    existingEvent.FineshTime = model.FineshTime;
        //    existingEvent.PlaceName = model.PlaceName;
        //    existingEvent.City = model.City;
        //    existingEvent.ConstraintAge = model.ConstraintAge;
        //    existingEvent.Discription = model.Discription;
        //    existingEvent.Price = model.Price;

        //    _context.Events.Update(existingEvent);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}




















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
