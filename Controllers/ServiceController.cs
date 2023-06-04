using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class ServiceController : Controller
    {
        private readonly WaterparkContext _db;

        public ServiceController(WaterparkContext db)
        {
            _db = db;
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Cashier}, {Constants.Roles.Accountant}")]
        public IActionResult Index()
        {
            IEnumerable<Service> objServiceList = _db.Services.OrderBy(s=>s.ServiceId);
            return View(objServiceList);
        }
        public IActionResult Search(string searchString)
        {
            var services = _db.Services.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                string searchStringOk = searchString.Replace(" ", "");
                services = services.Where(s => s.ServiceName.ToUpper().Contains(searchStringOk.ToUpper()));
            }

            var model = services.OrderBy(s => s.ServiceId).ToList();
            return View("Index", model);
        }

        // GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
        public IActionResult Create()
        {
            return View();
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Create(Service obj)
        {
            if (obj.ServiceName == obj.Description)
            {
                ModelState.AddModelError("description", "Ошибка! Описание услуги не может совпадать с ее наименованием");
                TempData["error"] = "Услуга НЕ создана!";
            }
            if (_db.Services.Any(s => s.ServiceName == obj.ServiceName))
            {
				ModelState.AddModelError("ServiceName", "Услуга с таким названием уже существует.");
                TempData["error"] = "Услуга НЕ создана!";
            }
            if (ModelState.IsValid)
            {
                _db.Services.Add(obj); // add to database
                _db.SaveChanges();
                TempData["success"] = "Услуга успешно создана!";
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
            var serviceFromDb = _db.Services.Find(id);

            if (serviceFromDb == null)
            {
                return NotFound();
            }
            return View(serviceFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Edit([Bind("ServiceId", "ServiceName", "Description", "Price")] Service obj)
        {
            //Console.WriteLine(obj.ServiceId.ToString());
            if (obj.ServiceName == obj.Description)
            {
                ModelState.AddModelError("description", "Ошибка! Описание услуги не может совпадать с ее наименованием");
                TempData["error"] = "Услуга НЕ изменена!";
            }
            if (_db.Services.Any(s => s.ServiceName == obj.ServiceName && s.ServiceId != obj.ServiceId))
            {
                ModelState.AddModelError("ServiceName", "Услуга с таким названием уже существует.");
                TempData["error"] = "Услуга НЕ изменена!";
            }

            if (ModelState.IsValid)
            {
                _db.Services.Update(obj); // update database
                _db.SaveChanges();
                TempData["success"] = "Услуга успешно изменена!";
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
            var serviceFromDb = _db.Services.Find(id);

            if (serviceFromDb == null)
            {
                return NotFound();
            }
            return View(serviceFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Delete(Service obj)
        {
            var serviceFromDb = _db.Services.Find(obj.ServiceId);

            if (serviceFromDb == null)
            {
                return NotFound();
            }
            _db.Services.Remove(serviceFromDb); // remove from database
            _db.SaveChanges();
            TempData["success"] = "Услуга успешно удалена!";
            return RedirectToAction("Index");
        }
    }
}
