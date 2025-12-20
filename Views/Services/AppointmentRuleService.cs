using SPORSALONUYONETIM.Data;
using SPORSALONUYONETIM.Models;

namespace SPORSALONUYONETIM.Services
{
    public class AppointmentRuleService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRuleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string? CheckRules(
            string userId,
            Trainer trainer,
            DateTime startTime,
            DateTime endTime
        )
        {
            // Geçmiş tarih kontrolü
            if (startTime < DateTime.Now)
                return "Geçmiş tarihli randevu alınamaz.";

            // Antrenör çalışma saatleri kontrolü
            bool isOvernightShift = trainer.WorkEnd < trainer.WorkStart;

            if (!isOvernightShift)
            {
                // Normal vardiya (ör: 09:00 - 17:00)
                if (startTime.TimeOfDay < trainer.WorkStart ||
                    endTime.TimeOfDay > trainer.WorkEnd)
                {
                    return "Antrenör çalışma saatleri dışında randevu alınamaz.";
                }
            }
            else
            {
                // Gece vardiyası (ör: 16:00 - 23:59)
                bool valid =
                    startTime.TimeOfDay >= trainer.WorkStart ||
                    endTime.TimeOfDay <= trainer.WorkEnd;

                if (!valid)
                {
                    return "Antrenör çalışma saatleri dışında randevu alınamaz.";
                }
            }

            // Aynı kullanıcının saat çakışması
            bool userConflict = _context.Appointments.Any(a =>
                a.UserId == userId &&
                startTime < a.EndTime &&
                endTime > a.StartTime
            );

            if (userConflict)
                return "Aynı saat aralığında başka bir randevunuz var.";

            // Antrenör doluluk / çakışma
            bool trainerConflict = _context.Appointments.Any(a =>
                a.TrainerId == trainer.Id &&
                startTime < a.EndTime &&
                endTime > a.StartTime
            );

            if (trainerConflict)
                return "Bu antrenör bu saat aralığında dolu.";

            return null;
        }
    }
}
