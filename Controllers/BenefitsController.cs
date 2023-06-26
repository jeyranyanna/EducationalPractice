using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class BenefitsController : Controller
    {
        private readonly WaterparkContext _db; 

        public BenefitsController(WaterparkContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Director) || User.IsInRole(Constants.Roles.Accountant);
            ViewBag.HasAccess = hasAccess;
            IEnumerable<Benefit> objBenefitList = _db.Benefits.OrderByDescending(s => s.DiscountPercentage);
            return View(objBenefitList);
        }

        public IActionResult Search(string searchString)
        {
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Director) || User.IsInRole(Constants.Roles.Accountant);
            ViewBag.HasAccess = hasAccess;
            var benefits = _db.Benefits.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                string searchStringOk = searchString.Replace(" ", "");
                benefits = benefits.Where(s => s.BenefitName.ToUpper().Contains(searchStringOk.ToUpper()));
            }

            var model = benefits.OrderBy(s => s.DiscountPercentage).ToList();
            return View("Index", model);
        }

        // GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult Create()
        {
            return View();
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult Create(Benefit obj)
        {
            if (_db.Benefits.Any(s => s.BenefitName == obj.BenefitName))
            {
                ModelState.AddModelError("BenefitName", "Льгота с таким наименованием уже существует.");
                TempData["error"] = "Льгота НЕ создана!";
            }
            if (ModelState.IsValid)
            {
                _db.Benefits.Add(obj); // добавить в базу данных
                _db.SaveChanges();
                TempData["success"] = "Льгота успешно создана!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


		//GET ACTION METHOD
		[Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var benefitFromDb = _db.Benefits.Find(id);

			if (benefitFromDb == null)
			{
				return NotFound();
			}
			return View(benefitFromDb);
		}


		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken] 
		[Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
		public IActionResult Edit([Bind("BenefitId", "BenefitName", "ConditionForReceivingBenefit", "DiscountPercentage")] Benefit obj)
		{
			if (_db.Benefits.Any(s => s.BenefitName == obj.BenefitName && s.BenefitId != obj.BenefitId))
			{
				ModelState.AddModelError("BenefitName", "Льгота с таким названием уже существует.");
				TempData["error"] = "Льгота НЕ изменена!";
			}
			if (ModelState.IsValid)
			{
				_db.Benefits.Update(obj); 
				_db.SaveChanges();
				TempData["success"] = "Льгота успешно изменена!";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		// GET ACTION METHOD
		[HttpGet]
		[Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var benefitFromDb = _db.Benefits.Find(id);

			if (benefitFromDb == null)
			{
				return NotFound();
			}
			return View(benefitFromDb);
		}

		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
		public IActionResult Delete(Benefit obj)
		{
			var benefitFromDb = _db.Benefits.Find(obj.BenefitId);

			if (benefitFromDb == null)
			{
				return NotFound();
			}
			_db.Benefits.Remove(benefitFromDb);

			_db.SaveChanges();
			TempData["success"] = "Льгота успешно удалена!";
			return RedirectToAction("Index");
		}
	}
}
