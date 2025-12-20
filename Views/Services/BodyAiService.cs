namespace SPORSALONUYONETIM.Services
{
    public class BodyAiService
    {
        public BodyAiResult Analyze(int heightCm, int weightKg)
        {
            //BMI hesapla
            double heightMeter = heightCm / 100.0;
            double bmi = weightKg / (heightMeter * heightMeter);
            bmi = Math.Round(bmi, 2);

            // BMI aralığı ve durumu
            string bmiRange;
            string bmiStatus;
            string recommendedSport;

            if (bmi < 18.5)
            {
                bmiRange = "< 18.5";
                bmiStatus = "Zayıf";
                recommendedSport = "Fitness (Kas kazanımı)";
            }
            else if (bmi >= 18.5 && bmi <= 24.9)
            {
                bmiRange = "18.5 – 24.9";
                bmiStatus = "Normal";
                recommendedSport = "CrossFit veya Fitness";
            }
            else if (bmi >= 25 && bmi <= 29.9)
            {
                bmiRange = "25 – 29.9";
                bmiStatus = "Fazla Kilolu";
                recommendedSport = "Fitness + Kardiyo";
            }
            else
            {
                bmiRange = "30 ve üzeri";
                bmiStatus = "Obez";
                recommendedSport = "Kardiyo + Yoga";
            }

            // Sonucu dön
            return new BodyAiResult
            {
                Bmi = bmi,
                BmiRange = bmiRange,
                BmiStatus = bmiStatus,
                RecommendedSport = recommendedSport
            };
        }
    }

    // AI çıktısı 
    public class BodyAiResult
    {
        public double Bmi { get; set; }
        public string BmiRange { get; set; } = "";
        public string BmiStatus { get; set; } = "";
        public string RecommendedSport { get; set; } = "";
    }
}
