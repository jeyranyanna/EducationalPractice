using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class TrainController : Controller
    {
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Instructor}, {Constants.Roles.Director}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
