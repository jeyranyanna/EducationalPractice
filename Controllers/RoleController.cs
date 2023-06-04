using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;

namespace HSEPractice2.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        //[Authorize(Roles = $"{Constants.Roles.Director}")]
        [Authorize(Policy = $"{Constants.Policies.RequireDirector}")]
        public IActionResult Director()
        {
            return View();
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Admin()
        {
            return View();
        }

    }
}
