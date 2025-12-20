using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUYONETIM.Data;

namespace SPORSALONUYONETIM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // TÜM RANDEVULAR (ADMIN)
        // ===============================
        public async Task<IActionResult> Appointments()
        {
            var list = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            return View(list);
        }

        // ===============================
        // ADMIN RANDEVU SİL
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);

            if (appt == null)
                return NotFound();

            _context.Appointments.Remove(appt);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Appointments));
        }
    }
}
