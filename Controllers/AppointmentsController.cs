using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUYONETIM.Data;
using SPORSALONUYONETIM.Models;
using SPORSALONUYONETIM.Services;

namespace SPORSALONUYONETIM.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppointmentRuleService _ruleService;

        public AppointmentsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppointmentRuleService ruleService)
        {
            _context = context;
            _userManager = userManager;
            _ruleService = ruleService;
        }

        // KULLANICININ RANDEVULARI
        public async Task<IActionResult> MyAppointments()
        {
            var userId = _userManager.GetUserId(User);

            var list = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            return View(list);
        }

        // RANDEVU ALMA FORMU
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services
                .Include(s => s.Trainer)
                .Select(s => new
                {
                    s.Id,
                    Display = $"{s.Name} ({s.Trainer.FullName})"
                })
                .ToList();

            return View();
        }

        // RANDEVU OLUŞTUR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int serviceId, DateTime startTime)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var service = await _context.Services
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null || service.Trainer == null)
            {
                ModelState.AddModelError("", "Hizmet veya antrenör bulunamadı.");
                return Create();
            }

            var endTime = startTime.AddMinutes(service.DurationMinutes);

            // TÜM RANDEVU KURALLARI BURADA
            var ruleError = _ruleService.CheckRules(
                userId,
                service.Trainer,
                startTime,
                endTime
            );

            if (ruleError != null)
            {
                ModelState.AddModelError("", ruleError);
                return Create();
            }

            var appointment = new Appointment
            {
                UserId = userId,
                TrainerId = service.TrainerId,
                ServiceId = service.Id,
                StartTime = startTime,
                EndTime = endTime
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyAppointments));
        }

        // RANDEVU SİLME ONAYI
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var appt = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (appt == null)
                return NotFound();

            return View(appt);
        }

        // RANDEVU SİL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var appt = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (appt == null)
                return NotFound();

            _context.Appointments.Remove(appt);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyAppointments));
        }
    }
}
