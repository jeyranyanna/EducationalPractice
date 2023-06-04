using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HSEPractice2.Controllers
{
    public class HomeController : Controller
    {
        private readonly WaterparkContext _db;



        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, WaterparkContext db)
        {
            _logger = logger;
            _db = db;
        }



        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                List<Attraction> attractions = _db.Attractions.Include(a => a.ZoneLocationNavigation).ToList();

                return View(attractions);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}