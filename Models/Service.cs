using System.ComponentModel.DataAnnotations;

namespace SPORSALONUYONETIM.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public decimal Price { get; set; }
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

    }
}
