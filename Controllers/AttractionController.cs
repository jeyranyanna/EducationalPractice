using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class AttractionController : Controller
    {
        private readonly WaterparkContext _db;

        public AttractionController(WaterparkContext db)
        {
            _db = db;
        }

     
        public IActionResult Index()
        {
            IEnumerable<Attraction> objServiceList = _db.GetAttractionListWithZoneNavigation().OrderBy(s => s.AttractionId);
            return View(objServiceList);
        }
        public IActionResult Search(string searchString)
        {
            var attractions = _db.GetAttractionListWithZoneNavigation().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                string searchStringOk = searchString.Replace(" ", "");
                attractions = attractions.Where(s => s.AttractionName.ToUpper().Contains(searchStringOk.ToUpper()));
            }

            var model = attractions.OrderBy(s => s.AttractionId).ToList();
            return View("Index", model);
        }

        // GET ACTION METHOD
        public IActionResult Create()
        {
			ViewBag.Zones = new SelectList(_db.Zones, "ZoneId", "ZoneName");
			var attraction = new Attraction();
			attraction.ZoneLocationNavigation = new Zone();
			return View(attraction);
		}

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("AttractionId", "AttractionName", "AttractionDescription", "GrowthRestrictions", "WeightRestrictions", "SlideHeight", "DescentTime", "ZoneLocation", "ZoneLocationNavigation")] Attraction obj)
        {
            Console.WriteLine(obj);
            if (_db.Attractions.Any(s => s.AttractionName == obj.AttractionName))
            {
                ModelState.AddModelError("AttractionName", "Аттракцион с таким названием уже существует.");
                TempData["error"] = "Аттракицон НЕ создан!";
            }
            if (ModelState.IsValid)
            {
                _db.Attractions.Add(obj); 
                _db.SaveChanges();
                TempData["success"] = "Аттракцион успешно создан!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //GET ACTION METHOD
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var attractionFromDb = _db.Attractions.Find(id);
            ViewBag.Zones = new SelectList(_db.Zones, "ZoneId", "ZoneName");

            if (attractionFromDb == null)
            {
                return NotFound();
            }
            return View(attractionFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Edit([Bind("AttractionId", "AttractionName", "AttractionDescription", "GrowthRestrictions", "WeightRestrictions", "SlideHeight", "DescentTime", "ZoneLocation", "ZoneLocationNavigation")] Attraction obj)
        {
            if (_db.Attractions.Any(s => s.AttractionName == obj.AttractionName && s.AttractionId != obj.AttractionId))
            {
                ModelState.AddModelError("AttractionName", "Аттракцион с таким названием уже существует.");
                TempData["error"] = "Аттракцион НЕ изменен!";
                ViewBag.Zones = new SelectList(_db.Zones, "ZoneId", "ZoneName");
                return View(obj);
            }

            if (ModelState.IsValid)
            {
                _db.Attractions.Update(obj); // update database
                _db.SaveChanges();
                TempData["success"] = "Аттракцион успешно изменен!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET ACTION METHOD
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var attractionFromDb = _db.Attractions.Find(id);
			ViewBag.Zones = new SelectList(_db.Zones, "ZoneId", "ZoneName");

			if (attractionFromDb == null)
            {
                return NotFound();
            }
            return View(attractionFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Attraction obj)
        {
            var attractionFromDb = _db.Attractions.Find(obj.AttractionId);

            if (attractionFromDb == null)
            {
                return NotFound();
            }
            _db.Attractions.Remove(attractionFromDb);
            _db.SaveChanges();
            TempData["success"] = "Аттракцион успешно удален!";
            return RedirectToAction("Index");
        }
    }
}
