using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
	public class ZoneController : Controller
	{
		private readonly WaterparkContext _db;

		public ZoneController(WaterparkContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Instructor);
            ViewBag.HasAccess = hasAccess;
            IEnumerable<Zone> objZoneList = _db.Zones.OrderBy(s => s.ZoneName);
			return View(objZoneList);
		}


        // GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Instructor}")]
        public IActionResult Create()
		{
			return View();
		}

		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken] // to avoid cross site request forgery
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Instructor}")]
        public IActionResult Create(Zone obj)
		{
            if (_db.Zones.Any(s => s.ZoneName == obj.ZoneName))
            {
                ModelState.AddModelError("ZoneName", "Зона с таким названием уже существует.");
                TempData["error"] = "Зона НЕ создана!";
            }
            if (ModelState.IsValid)
			{
				_db.Zones.Add(obj); 
				_db.SaveChanges();
				TempData["success"] = "Зона успешно добавлена!";
				return RedirectToAction("Index");
			}
			return View(obj);
		}


        //GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Instructor}")]
        public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var zoneFromDb = _db.Zones.Find(id);

			if (zoneFromDb == null)
			{
				return NotFound();
			}
			return View(zoneFromDb);
		}

		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Instructor}")]
        public IActionResult Edit([Bind("ZoneId", "ZoneName")] Zone obj)
		{
            if (_db.Zones.Any(s => s.ZoneName == obj.ZoneName && s.ZoneId != obj.ZoneId))
            {
                ModelState.AddModelError("ZoneName", "Зона с таким названием уже существует.");
                TempData["error"] = "Зона НЕ создана!";
            }
            if (ModelState.IsValid)
			{
				_db.Zones.Update(obj); 
				_db.SaveChanges();
				TempData["success"] = "Зона успешно изменена!";
				return RedirectToAction("Index");
			}
			return View(obj);
		}


		// GET ACTION METHOD
		[HttpGet]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Instructor}")]
        public IActionResult Delete(int? id)
		{
			List<Zone> zoneList = _db.Zones.OrderBy(s => s.ZoneId).ToList();
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var zoneFromDb = zoneList.Find(s => s.ZoneId == id);

			if (zoneFromDb == null)
			{
				return NotFound();
			}
			return View(zoneFromDb);
		}


		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Instructor}")]
        public IActionResult Delete(Zone obj)
		{
			var zoneFromDb = _db.Zones.Find(obj.ZoneId);

			if (zoneFromDb == null)
			{
				return NotFound();
			}
			_db.Zones.Remove(zoneFromDb); 
			_db.SaveChanges();
			TempData["success"] = "Зона успешно удалена!";
			return RedirectToAction("Index");
		}
	}
}
