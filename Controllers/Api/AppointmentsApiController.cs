using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUYONETIM.Data;
using SPORSALONUYONETIM.Models;
using SPORSALONUYONETIM.Services;

namespace SPORSALONUYONETIM.Controllers.Api
{
    [ApiController]
    [Route("api/appointments")]
    [AllowAnonymous]

    public class AppointmentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppointmentRuleService _ruleService;

        public AppointmentsApiController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppointmentRuleService ruleService)
        {
            _context = context;
            _userManager = userManager;
            _ruleService = ruleService;
        }

        // POST: /api/appointments/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
        {
            var userId = _userManager.GetUserId(User)
             ?? "TEST_USER_API";

            var service = await _context.Services
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(s => s.Id == dto.ServiceId);

            if (service == null || service.Trainer == null)
                return BadRequest("Hizmet veya antrenör bulunamadı.");

            var startTime = dto.StartTime;
            var endTime = startTime.AddMinutes(service.DurationMinutes);

            //  Kurallar 
            var ruleError = _ruleService.CheckRules(userId, service.Trainer, startTime, endTime);
            if (ruleError != null)
                return BadRequest(ruleError);

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

            return Ok(new
            {
                message = "Randevu oluşturuldu.",
                appointmentId = appointment.Id
            });
        }
    }
}
