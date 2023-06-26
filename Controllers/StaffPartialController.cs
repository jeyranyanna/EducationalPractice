using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HSEPractice2.Controllers
{
    public class StaffPartialController : Controller
    {
        private readonly WaterparkContext _db;

        public StaffPartialController(WaterparkContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Staff> staffList = _db.GetStaffListWithPostNavigation().OrderBy(s => s.EmployeeSurname).ToList();
            //IEnumerable<Staff> staffList = _db.Staff.ToList();
            IEnumerable<Post> postList = _db.Posts.ToList();

            var model = new Tuple<IEnumerable<Staff>, IEnumerable<Post>>(staffList, postList);

            return View(model);
            //return View(staffList);
        }
        public IActionResult Search(string searchString)
        {
            var services = _db.GetStaffListWithPostNavigation().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                services = services.Where(s => s.EmployeeSurname.ToUpper().Contains(searchString.ToUpper()));
            }

            var model = services.OrderBy(s => s.EmployeeSurname).ToList();
            return View("Index", model);
        }

        // GET ACTION METHOD
        public IActionResult Create()
        {
            ViewBag.Posts = new SelectList(_db.Posts, "PostId", "Post1");
            return View();
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Create(Staff obj)
        {
            //if (obj.ServiceName == obj.Description)
            //{
            //    ModelState.AddModelError("description", "Ошибка! Описание услуги не может совпадать с ее наименованием");
            //}
            //if (_db.Services.Any(s => s.ServiceName == obj.ServiceName))
            //{
            //    ModelState.AddModelError("ServiceName", "Услуга с таким названием уже существует.");
            //}

            Console.WriteLine(obj.Post);
            Console.WriteLine(obj.DateOfHiring);
            //string d = obj.DateOfHiring.ToString();
            //int year = int.Parse(d.Substring(6, 4));
            //int month = int.Parse(d.Substring(3, 2));
            //int day = int.Parse(d.Substring(0, 2));
            //DateOnly date = new DateOnly(year, month, day);
            //Console.WriteLine( date);
            //obj.DateOfHiring = date;
            if (ModelState.IsValid)
            {
                _db.Staff.Add(obj); // add to database
                _db.SaveChanges();
                TempData["success"] = "Сотрудник успешно добавлен!";
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine(obj.Post.ToString());
                Console.WriteLine(obj.DateOfHiring);
                Console.WriteLine(obj.EmployeeSurname);
                Console.WriteLine(obj.EmployeeName);
                Console.WriteLine(obj.EmployeePatronymic);

            }
            // Если модель не прошла валидацию, повторное отображение формы с ошибками
            ViewBag.Posts = new SelectList(_db.Posts, "PostId", "Post1", obj.Post);
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
            List<Staff> staffList = _db.GetStaffListWithPostNavigation().OrderBy(s => s.EmployeeId).ToList();
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var staffFromDb = staffList.Find(s => s.EmployeeId == id);

            if (staffFromDb == null)
            {
                return NotFound();
            }
            ViewBag.Posts = GetPostsList(); // Получите список должностей и передайте его в ViewBag
            return View(staffFromDb);
        }


        // Метод для получения списка должностей
        private List<SelectListItem> GetPostsList()
        {
            List<Post> posts = _db.Posts.ToList();
            List<SelectListItem> postsList = posts.Select(p => new SelectListItem
            {
                Value = p.PostId.ToString(),
                Text = p.Post1
            }).ToList();
            return postsList;
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Delete(Staff obj)
        {
            var employeeFromDb = _db.Staff.Find(obj.EmployeeId);

            if (employeeFromDb == null)
            {
                return NotFound();
            }
            _db.Staff.Remove(employeeFromDb); // remove from database
            _db.SaveChanges();
            TempData["success"] = "Сотрудник успешно удален!";
            return RedirectToAction("Index");
        }
    }
}
