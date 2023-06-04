using Microsoft.AspNetCore.Mvc;

namespace HSEPractice2.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
