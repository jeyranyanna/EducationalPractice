using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class PostsController : Controller
    {
        private readonly WaterparkContext _db;

        public PostsController(WaterparkContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Director);
            ViewBag.HasAccess = hasAccess;
            IEnumerable<Post> postList = _db.Posts.OrderBy(s => s.Post1).ToList();
            return View(postList);
        }
        public IActionResult SearchPosts(string searchString)
        {
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Director);
            ViewBag.HasAccess = hasAccess;
            var posts = _db.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Post1.ToUpper().Contains(searchString.ToUpper()));
            }

            var model = posts.OrderBy(s => s.Post1).ToList();
            return View("Index", model);
        }

        // GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult CreatePosts()
        {
            return View();
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult CreatePosts(Post obj)
        {
            if (_db.Posts.Any(s => s.Post1 == obj.Post1))
            {
                ModelState.AddModelError("Post1", "Должность с таким названием уже существует.");
                TempData["error"] = "Должность НЕ создана!";
            }
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
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult EditPosts(int? id)
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult EditPosts([Bind("PostId", "Post1", "SalaryAmount")] Post obj)
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
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult DeletePosts(int? id)
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}, {Constants.Roles.Accountant}")]
        public IActionResult DeletePosts(Post obj)
        {
            var employeeFromDb = _db.Posts.Find(obj.PostId);

            if (employeeFromDb == null)
            {
                return NotFound();
            }
            _db.Posts.Remove(employeeFromDb); // remove from database
            _db.SaveChanges();
            TempData["success"] = "Должность успешно удалена!";
            return RedirectToAction("Index");
        }

    }
}
