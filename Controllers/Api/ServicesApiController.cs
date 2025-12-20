using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUYONETIM.Data;

namespace SPORSALONUYONETIM.Controllers.Api
{
    [ApiController]
    [Route("api/services")]
    public class ServicesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var services = _context.Services
                .Include(s => s.Trainer)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.DurationMinutes,
                    s.Price,
                    TrainerName = s.Trainer.FullName
                })
                .ToList();

            return Ok(services);
        }
    }
}
