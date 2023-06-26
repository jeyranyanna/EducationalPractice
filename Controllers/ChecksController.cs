using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Reflection.Metadata;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
	public class ChecksController : Controller
	{
		private readonly WaterparkContext _db;

		public ChecksController(WaterparkContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			bool hasAccess = User.IsInRole(Constants.Roles.Instructor) || User.IsInRole(Constants.Roles.Administrator);
			ViewBag.HasAccess = hasAccess;
			IEnumerable<Checking> objChecksList = _db.GetCheckings().OrderByDescending(s => s.CheckingDate);
			return View(objChecksList);
		}

        [Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
        public IActionResult Create()
        {
            ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
            ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");
            var check = new Checking();
            check.Attraction = new Attraction();
            check.Employee = new Staff();
            return View(check);
        }


        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
        public IActionResult Create([Bind($"{nameof(Checking.CheckingDate)}, {nameof(Checking.AttractionId)}, {nameof(Checking.EmployeeId)}, {nameof(Checking.AdmissionLaunch)}, {nameof(Checking.Note)}, {nameof(Checking.Attraction)}, {nameof(Checking.Employee)}")] Checking obj)
        {
            DateTime objCheckingDate = obj.CheckingDate.ToUniversalTime();

            bool checkingExists = _db.Checkings
                .Any(ca => ca.CheckingDate == obj.CheckingDate && ca.AttractionId == obj.AttractionId && ca.CheckingId != obj.CheckingId);


            if (checkingExists)
            {
                ModelState.AddModelError("CheckingDate", "Ошибка! Проверка для такого аттракциона в выбранный день уже существует!");
                TempData["error"] = "Проверка НЕ создана!";
                ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
                ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");
                var check = new Checking();
                check.Attraction = new Attraction();
                check.Employee = new Staff();
                return View(check);
            }
            if (ModelState.IsValid)
            {
                _db.Checkings.Add(obj); // добавить в базу данных
                _db.SaveChanges();
                TempData["success"] = "Проверка успешно создана!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

		// GET ACTION METHOD
		[HttpGet]
		[Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var checkFromDb = _db.GetCheckings().SingleOrDefault(s=>s.CheckingId == id);
			ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
			ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");

			if (checkFromDb == null)
			{
				return NotFound();
			}
			return View(checkFromDb);
		}

		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
		public IActionResult Delete(Checking obj)
		{
			var checkFromDb = _db.Checkings.Find(obj.CheckingId);

			if (checkFromDb == null)
			{
				return NotFound();
			}
			_db.Checkings.Remove(checkFromDb);
			_db.SaveChanges();
			TempData["success"] = "Проверка аттракциона успешно удалена!";
			return RedirectToAction("Index");
		}

        //Экспорт в эксель таблицы сотрудников
        public IActionResult ExportToExcel()
        {
            var query = _db.Attractions
                .Where(a => _db.Checkings.Any(ca =>
                    ca.AttractionId == a.AttractionId &&
                    ca.AdmissionLaunch == "Нет" &&
                    ca.CheckingDate >= new DateTime(2023, 01, 01) &&
                    ca.CheckingDate <= new DateTime(2023, 12, 31)))
                .Select(a => new
                {
                    a.AttractionName,
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Отчет");

            ws.Cells["A1"].Value = "Отчет";
            ws.Cells["B1"].Value = "Отчет по проверкам аттракционов";

            ws.Cells["A3"].Value = "Дата";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} в {0:H:mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Название аттракциона";

            int rowStart = 7;
            foreach (var item in query)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.AttractionName;

                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment; filename=ExcelReport.xlsx");
            byte[] fileBytes = pck.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelReport.xlsx");
        }


        //[Authorize(Roles = $"{Constants.Roles.Instructor}")]
        //public IActionResult Edit(int? id)
        //{
        //	if (id == null || id == 0)
        //	{
        //		return NotFound();
        //	}

        //	var checkFromDb = _db.Checkings.Find(id);

        //	if (checkFromDb == null)
        //	{
        //		return NotFound();
        //	}

        //	ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
        //	ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");

        //	return View(checkFromDb);
        //}

        //// POST ACTION METHOD
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = $"{Constants.Roles.Instructor}")]
        //public IActionResult Edit([Bind($"{nameof(Checking.CheckingId)}, {nameof(Checking.CheckingDate)}, {nameof(Checking.AttractionId)}, {nameof(Checking.EmployeeId)}, {nameof(Checking.AdmissionLaunch)}, {nameof(Checking.Note)}, {nameof(Checking.Attraction)}, {nameof(Checking.Employee)}")] Checking obj)
        //{
        //	bool checkingExists = _db.Checkings
        //		.Any(ca => ca.CheckingDate == obj.CheckingDate && ca.AttractionId == obj.AttractionId && ca.CheckingId != obj.CheckingId);

        //	if (checkingExists)
        //	{
        //		ModelState.AddModelError("CheckingDate", "Ошибка! Проверка для такого аттракциона в выбранный день уже существует!");
        //		TempData["error"] = "Проверка НЕ создана!";
        //		ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
        //		ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");
        //		var check = new Checking();
        //		check.Attraction = new Attraction();
        //		check.Employee = new Staff();
        //		return View(check);
        //	}
        //	if (ModelState.IsValid)
        //	{
        //		_db.Checkings.Update(obj);
        //		_db.SaveChanges();
        //		TempData["success"] = "Проверка успешно изменена!";
        //		return RedirectToAction("Index");
        //	}
        //	return View(obj);
        //}
    }
}
