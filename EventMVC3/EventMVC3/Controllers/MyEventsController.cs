using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventMVC3.Models;
using System.Linq;
using System.Threading.Tasks;
using EventMVC3.Data;

namespace EventMVC3.Controllers
{
    public class MyEventsController : Controller
    {
        private readonly EventContext _context;

        public MyEventsController(EventContext context)
        {
            _context = context;
        }

        // عرض أحداث المستخدم الحالي
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var userEvents = await _context.Reservations
                .Include(r => r.Event)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(userEvents);
        }

        // عرض تفاصيل الحدث (معدلة لعرض كل المعلومات)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Event)
                .ThenInclude(e => e.CategoryNavigation) // لجلب بيانات التصنيف
                .FirstOrDefaultAsync(r => r.IdNumber == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        // عرض الأحداث المتاحة للإضافة (معدلة)
        public async Task<IActionResult> Create()
        {
            var availableEvents = await _context.Events
                .Include(e => e.CategoryNavigation)
                .ToListAsync();

            return View(availableEvents);
        }

        // إضافة حدث جديد (معدلة)
        //[HttpPost]
        //public async Task<IActionResult> AddEvent(int eventId)
        //{
        //    var reservation = new Reservation
        //    {
        //        EventId = eventId,
        //        UserId = GetCurrentUserId(),
        //        BookingDate = DateTime.Now
        //    };

        //    _context.Reservations.Add(reservation);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(int eventId)
        {
            // 1. التحقق من وجود الحدث
            if (!await _context.Events.AnyAsync(e => e.Id == eventId))
                return NotFound();

            // 2. إنشاء الحجز
            var reservation = new Reservation
            {
                EventId = eventId,
                UserId = GetCurrentUserId(), // تأكد أن الزملاء لم يغيروا طريقة العمل
                BookingDate = DateTime.Now
            };

            // 3. المحاولة مع معالجة الأخطاء
            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        // الحذف (كما هو)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.IdNumber == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            // سيتم استبدالها بنظام المصادقة الفعلي
            return 1; // قيمة افتراضية للاختبار
        }
    }
}