using System.ComponentModel.DataAnnotations;

namespace SPORSALONUYONETIM.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public SportType SportType { get; set; }

        public TimeSpan WorkStart { get; set; }
        public TimeSpan WorkEnd { get; set; }
    }
}
// github test
