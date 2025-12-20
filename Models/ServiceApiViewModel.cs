namespace SPORSALONUYONETIM.Models
{
    public class ServiceApiViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string TrainerName { get; set; } = string.Empty;
    }
}
