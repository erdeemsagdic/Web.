using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUYONETIM.Data;

namespace SPORSALONUYONETIM.Controllers.Api
{
    [ApiController]
    [Route("api/trainers")]
    public class TrainersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("available")]
        public IActionResult GetAvailableTrainers(DateTime date)
        {
            var trainers = _context.Trainers
                .Where(t =>
                    !_context.Appointments.Any(a =>
                        a.TrainerId == t.Id &&
                        a.StartTime.Date == date.Date
                    )
                )
                .Select(t => new
                {
                    t.Id,
                    t.FullName,
                    t.SportType,
                    t.WorkStart,
                    t.WorkEnd
                })
                .ToList();

            return Ok(trainers);
        }
    }
}
