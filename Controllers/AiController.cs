using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPORSALONUYONETIM.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        [HttpGet]
        public IActionResult Recommend()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Recommend(double height, double weight)
        {
            
            double heightMeter = height / 100;
            double bmi = weight / (heightMeter * heightMeter);

            string category;
            string sport;
            string message;

            if (bmi < 18.5)
            {
                category = "Zayıf";
                sport = "Fitness / Yoga";
                message = "Kas kazanımı ve denge odaklı sporlar önerilir.";
            }
            else if (bmi < 25)
            {
                category = "Normal";
                sport = "Fitness / CrossFit";
                message = "Genel kondisyonu artıran sporlar sana uygun.";
            }
            else if (bmi < 30)
            {
                category = "Fazla Kilolu";
                sport = "Fitness / Kick Boks";
                message = "Yağ yakımını destekleyen sporlar önerilir.";
            }
            else
            {
                category = "Obez";
                sport = "Yoga + Hafif Fitness";
                message = "Düşük riskli ve sürdürülebilir sporlar önerilir.";
            }

            ViewBag.BMI = Math.Round(bmi, 2);
            ViewBag.Category = category;
            ViewBag.Sport = sport;
            ViewBag.Message = message;

            return View();
        }
    }
}
