//using HSEPractice2.Areas.Identity.Data;
//using HSEPractice2.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.EntityFrameworkCore;
//using static HSEPractice2.Constants;

//namespace HSEPractice2.Controllers
//{
//	public class CheckingController : Controller
//	{
//		private readonly WaterparkContext _db;

//		public CheckingController(WaterparkContext db)
//		{
//			_db = db;
//		}

//		//[Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}, {Constants.Roles.Director}")]
//		public IActionResult Index()
//		{
//			bool hasAccess = User.IsInRole(Constants.Roles.Instructor);
//			ViewBag.HasAccess = hasAccess;
//			IEnumerable<CheckingAttraction> objChecksList = _db.GetChecks().OrderBy(s => s.CheckingDate);
//			return View(objChecksList);
//		}


//		//public IActionResult Search(string searchString)
//		//{
//		//	bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Director);
//		//	ViewBag.HasAccess = hasAccess;
//		//	var services = _db.Services.AsQueryable();

//		//	if (!string.IsNullOrEmpty(searchString))
//		//	{
//		//		string searchStringOk = searchString.Replace(" ", "");
//		//		services = services.Where(s => s.ServiceName.ToUpper().Contains(searchStringOk.ToUpper()));
//		//	}

//		//	var model = services.OrderBy(s => s.ServiceName).ToList();
//		//	return View("Index", model);
//		//}

//		// GET ACTION METHOD
//		[Authorize(Roles = $"{Constants.Roles.Instructor}")]
//		public IActionResult Create()
//		{
//			ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
//			ViewBag.Employers = new SelectList(_db.Staff.Where(s=>s.Post==5).ToList(), "EmployeeId", "EmployeeSurname");
//			var check = new CheckingAttraction();
//			check.Attraction = new Attraction();
//			check.Employee = new Staff();
//			return View(check);
//		}


//		// POST ACTION METHOD
//		[HttpPost]
//		[ValidateAntiForgeryToken] 
//		[Authorize(Roles = $"{Constants.Roles.Instructor}")]
//		public IActionResult Create([Bind($"{nameof(CheckingAttraction.CheckingDate)}, {nameof(CheckingAttraction.AttractionId)}, {nameof(CheckingAttraction.EmployeeId)}, {nameof(CheckingAttraction.AdmissionLaunch)}, {nameof(CheckingAttraction.Note)}, {nameof(CheckingAttraction.Attraction)}, {nameof(CheckingAttraction.Employee)}")] CheckingAttraction obj)
//		{
//			//DateTime objCheckingDate = obj.CheckingDate.ToUniversalTime();

//			bool checkingExists = _db.CheckingAttractions
//				.Any(ca => ca.CheckingDate == obj.CheckingDate && ca.AttractionId == obj.AttractionId);


//			if (checkingExists)
//			{
//				ModelState.AddModelError("CheckingDate", "Ошибка! Проверка для такого аттракциона в выбранный день уже существует!");
//				TempData["error"] = "Проверка НЕ создана!";
//				ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
//				ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");
//				var check = new CheckingAttraction();
//				check.Attraction = new Attraction();
//				check.Employee = new Staff();
//				return View(check);
//			}
//			if (ModelState.IsValid)
//			{
//				_db.CheckingAttractions.Add(obj); // добавить в базу данных
//				_db.SaveChanges();
//				TempData["success"] = "Проверка успешно создана!";
//				return RedirectToAction("Index");
//			}
//			return View(obj);
//		}


//		[Authorize(Roles = $"{Constants.Roles.Instructor}")]
//		public IActionResult Edit(int? id, DateTime? date)
//		{
//			if (id == null || id == 0 || date == null)
//			{
//				return NotFound();
//			}

//			var checkFromDb = _db.CheckingAttractions.SingleOrDefault(ca => ca.CheckingDate == date && ca.AttractionId == id);

//			if (checkFromDb == null)
//			{
//				return NotFound();
//			}

//			ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
//			ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");

//			return View(checkFromDb);
//		}

//        // POST ACTION METHOD
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = $"{Constants.Roles.Instructor}")]
//        public IActionResult Edit([Bind($"{nameof(CheckingAttraction.CheckingDate)}, {nameof(CheckingAttraction.AttractionId)}, {nameof(CheckingAttraction.EmployeeId)}, {nameof(CheckingAttraction.AdmissionLaunch)}, {nameof(CheckingAttraction.Note)}, {nameof(CheckingAttraction.Attraction)}, {nameof(CheckingAttraction.Employee)}")] CheckingAttraction obj)
//        {
//            var existingChecking = _db.CheckingAttractions.FirstOrDefault(ca => ca.CheckingDate == obj.CheckingDate && ca.AttractionId == obj.AttractionId);

//            // Check if CheckingDate or AttractionId were changed
//            if (existingChecking != null && (existingChecking.CheckingDate != obj.CheckingDate || existingChecking.AttractionId != obj.AttractionId))
//            {
//                bool checkingExists = _db.CheckingAttractions
//                    .Any(ca => ca.CheckingDate == obj.CheckingDate && ca.AttractionId == obj.AttractionId);

//                if (checkingExists)
//                {
//                    ModelState.AddModelError("CheckingDate", "Ошибка! Проверка для такого аттракциона в выбранный день уже существует!");
//                    TempData["error"] = "Проверка НЕ создана!";
//                    ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
//                    ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");
//                    var check = new CheckingAttraction();
//                    check.Attraction = new Attraction();
//                    check.Employee = new Staff();
//                    return View(check);
//                }

//                // Detach existingChecking entity
//                _db.Entry(existingChecking).State = EntityState.Detached;
//            }


//            if (ModelState.IsValid)
//            {
//                _db.CheckingAttractions.Attach(obj);
//                _db.Entry(obj).State = EntityState.Modified;
//                _db.SaveChanges();
//                TempData["success"] = "Проверка успешно изменена!";
//                return RedirectToAction("Index");
//            }

//            //if (ModelState.IsValid)
//            //{
//            //    _db.CheckingAttractions.Update(obj);
//            //    _db.SaveChanges();
//            //    TempData["success"] = "Проверка успешно изменена!";
//            //    return RedirectToAction("Index");
//            //}

//            return View(obj);
//            //bool checkingExists = _db.CheckingAttractions
//            //    .Any(ca => ca.CheckingDate == obj.CheckingDate && ca.AttractionId == obj.AttractionId);


//            //if (checkingExists)
//            //{
//            //    ModelState.AddModelError("CheckingDate", "Ошибка! Проверка для такого аттракциона в выбранный день уже существует!");
//            //    TempData["error"] = "Проверка НЕ создана!";
//            //    ViewBag.Attractions = new SelectList(_db.Attractions, "AttractionId", "AttractionName");
//            //    ViewBag.Employers = new SelectList(_db.Staff.Where(s => s.Post == 5).ToList(), "EmployeeId", "EmployeeSurname");
//            //    var check = new CheckingAttraction();
//            //    check.Attraction = new Attraction();
//            //    check.Employee = new Staff();
//            //    return View(check);
//            //}
//            //if (ModelState.IsValid)
//            //{
//            //    _db.CheckingAttractions.Update(obj);
//            //    _db.SaveChanges();
//            //    TempData["success"] = "Проверка успешно изменена!";
//            //    return RedirectToAction("Index");
//            //}
//            //return View(obj);
//        }

//        //// POST ACTION METHOD
//        //[HttpPost]
//        //[ValidateAntiForgeryToken] // to avoid cross site request forgery
//        //[Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
//        //public IActionResult Edit([Bind("EmployeeId", "EmployeeSurname", "EmployeeName", "EmployeePatronymic", "Post", "DateOfHiring", "Phone")] Staff obj)
//        //{
//        //	if (ModelState.IsValid)
//        //	{
//        //		_db.Staff.Update(obj); // обновить запись в БД
//        //		_db.SaveChanges();
//        //		TempData["success"] = "Сотрудник успешно изменен!";
//        //		return RedirectToAction("Index");
//        //	}
//        //	return View(obj);
//        //}

//    }
//}
