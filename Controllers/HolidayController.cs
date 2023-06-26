using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class HolidayController : Controller
    {
        private readonly WaterparkContext _db;

        public HolidayController(WaterparkContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
			bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Animator);
			ViewBag.HasAccess = hasAccess;
			IEnumerable<Event> objEventsList = _db.GetEvents().OrderByDescending(s => s.DateTime);
			return View(objEventsList);
		}

        // GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Animator}")]
        public IActionResult Create()
        {
			ViewBag.Animators = new SelectList(_db.Staff.Where(s=>s.Post==5).OrderBy(s=>s.EmployeeSurname).ToList(), "EmployeeId", "EmployeeSurname");
			var _event = new Event();
			_event.Employee = new Staff();
			return View(_event);
		}

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize(Roles = $"{Constants.Roles.Administrator},  {Constants.Roles.Animator}")]
        public IActionResult Create([Bind("EventId", "EventName", "DateTime", "Duration", "Description", "EmployeeId", "Employee")] Event obj)
        {
			// Преобразование локального времени в UTC
			obj.DateTime = obj.DateTime.ToUniversalTime();
			if (_db.Events.Any(s => s.EventName == obj.EventName && s.DateTime == obj.DateTime))
            {
                ModelState.AddModelError("EventName", "Введенное мероприятие уже существует.");
                TempData["error"] = "Мероприятие НЕ создано!";
            }
            if (ModelState.IsValid)
            {
                _db.Events.Add(obj); // добавить в базу данных
                _db.SaveChanges();
                TempData["success"] = "Мероприятие успешно создано!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator},  {Constants.Roles.Animator}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var eventFromDb = _db.GetEvents().Find(s=>s.EventId == id);
			ViewBag.Animators = new SelectList(_db.Staff.Where(s => s.Post == 5).OrderBy(s => s.EmployeeSurname).ToList(), "EmployeeId", "EmployeeSurname");

			if (eventFromDb == null)
            {
                return NotFound();
            }
            return View(eventFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator},  {Constants.Roles.Animator}")]
        public IActionResult Edit([Bind("EventId", "EventName", "DateTime", "Duration", "Description", "EmployeeId", "Employee")] Event obj)
        {
            // Преобразование локального времени в UTC
            obj.DateTime = obj.DateTime.ToUniversalTime();
            if (_db.Events.Any(s => s.EventName == obj.EventName && s.DateTime == obj.DateTime && s.EventId != obj.EventId))
            {
                ModelState.AddModelError("EventName", "Введенное мероприятие уже существует.");
                TempData["error"] = "Мероприятие НЕ изменено!";
				ViewBag.Animators = new SelectList(_db.Staff.Where(s => s.Post == 5).OrderBy(s => s.EmployeeSurname).ToList(), "EmployeeId", "EmployeeSurname");
				var _event = new Event();
                return View();
			}
            if (ModelState.IsValid)
            {
                _db.Events.Update(obj); // изменить запись в базе данных
                _db.SaveChanges();
                TempData["success"] = "Мероприятие успешно изменено!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


		// GET ACTION METHOD
		[HttpGet]
		[Authorize(Roles = $"{Constants.Roles.Administrator},  {Constants.Roles.Animator}")]
		public IActionResult Delete(int? id)
		{
			List<Event> eventList = _db.GetEvents().ToList();
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var eventFromDb = eventList.Find(s => s.EventId == id);

			if (eventFromDb == null)
			{
				return NotFound();
			}
			return View(eventFromDb);
		}


		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = $"{Constants.Roles.Administrator},  {Constants.Roles.Animator}")]
		public IActionResult Delete(Event obj)
		{
			var eventFromDb = _db.GetEvents().Find(s=>s.EventId == obj.EventId);

			if (eventFromDb == null)
			{
				return NotFound();
			}
			_db.Events.Remove(eventFromDb); // удалить из базы данных
			_db.SaveChanges();
			TempData["success"] = "Мероприятие успешно удалено!";
			return RedirectToAction("Index");
		}
	}
}
