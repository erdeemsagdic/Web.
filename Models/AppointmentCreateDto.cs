using System;

namespace SPORSALONUYONETIM.Models
{
    public class AppointmentCreateDto
    {
        public int TrainerId { get; set; }
        public int ServiceId { get; set; }
        public DateTime StartTime { get; set; }
    }

}
