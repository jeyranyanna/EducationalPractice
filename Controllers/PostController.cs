using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HSEPractice2.Controllers
{
    public class PostController : Controller
    {
        private readonly WaterparkContext _db;

        public PostController(WaterparkContext db)
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
            var services = _db.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                services = services.Where(s => s.Post1.ToUpper().Contains(searchString.ToUpper()));
            }

            var model = services.OrderBy(s => s.Post1).ToList();
            return View("Index", model);
        }

        // GET ACTION METHOD
        public IActionResult Create()
        {
            return View();
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Create(Post obj)
        {
            if (ModelState.IsValid)
            {
                _db.Posts.Add(obj); // add to database
                _db.SaveChanges();
                TempData["success"] = "Должность успешно добавлена!";
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
            var postFromDb = _db.Posts.Find(id);

            if (postFromDb == null)
            {
                return NotFound();
            }
            return View(postFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Edit([Bind("PostId", "Post1", "SalaryAmount")] Post obj)
        {
            if (ModelState.IsValid)
            {
                _db.Posts.Update(obj); // update database
                _db.SaveChanges();
                TempData["success"] = "Должность успешно изменена!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET ACTION METHOD
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            List<Post> postList = _db.Posts.OrderBy(s => s.PostId).ToList();
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var postFromDb = postList.Find(s => s.PostId == id);

            if (postFromDb == null)
            {
                return NotFound();
            }
            return View(postFromDb);
        }


        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        public IActionResult Delete(Post obj)
        {
            var employeeFromDb = _db.Posts.Find(obj.PostId);

            if (employeeFromDb == null)
            {
                return NotFound();
            }
            _db.Posts.Remove(employeeFromDb); // remove from database
            _db.SaveChanges();
            TempData["success"] = "Сотрудник успешно удален!";
            return RedirectToAction("Index");
        }
    }
}
