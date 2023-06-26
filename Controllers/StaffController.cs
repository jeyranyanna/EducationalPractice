using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OfficeOpenXml;
using System.Web.WebPages;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class StaffController : Controller
    {
        private readonly WaterparkContext _db;

        public StaffController(WaterparkContext db)
        {
            _db = db;
        }


		public IActionResult Index()
		{
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Director);
            ViewBag.HasAccess = hasAccess;
            List<Staff> staffList = _db.GetStaffListWithPostNavigation().OrderBy(s => s.EmployeeSurname).ToList();
			return View(staffList);
		}


		public IActionResult Search(string searchString)
        {
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator) || User.IsInRole(Constants.Roles.Director);
            ViewBag.HasAccess = hasAccess;
            var staffList = _db.GetStaffListWithPostNavigation().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
				staffList = staffList.Where(s => s.EmployeeSurname.ToUpper().Contains(searchString.ToUpper()));
            }

            var model = staffList.OrderBy(s => s.EmployeeSurname).ToList();
            return View("Index", model);
        }

        // GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
        public IActionResult Create()
        {
			ViewBag.Posts = new SelectList(_db.Posts, "PostId", "Post1");
			var staff = new Staff();
			staff.PostNavigation = new Post();
			return View(staff);
		}

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
        public IActionResult Create([Bind($"{nameof(Staff.EmployeeId)}, {nameof(Staff.EmployeeSurname)}, {nameof(Staff.EmployeeName)}, {nameof(Staff.EmployeePatronymic)}, {nameof(Staff.Post)}, {nameof(Staff.DateOfHiring)}, {nameof(Staff.PostNavigation)}, {nameof(Staff.Phone)}")] Staff obj)
        { 
            if (ModelState.IsValid)
            {
                _db.Staff.Add(obj); // add to database
                _db.SaveChanges();
                TempData["success"] = "Сотрудник успешно добавлен!";
                return RedirectToAction("Index");
            }
			// Если модель не прошла валидацию, повторное отображение формы с ошибками
			ViewBag.Posts = new SelectList(_db.Posts, "PostId", "Post1", obj.Post);
			return View(obj);
        }


        //GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
        public IActionResult Edit(int? id)
        {
			if (id == null || id == 0)
            {
                return NotFound();
            }
            var employeeFromDb = _db.Staff.Find(id);
			ViewBag.Posts = new SelectList(_db.Posts, "PostId", "Post1");

			if (employeeFromDb == null)
            {
                return NotFound();
            }
            return View(employeeFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // to avoid cross site request forgery
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
        public IActionResult Edit([Bind("EmployeeId", "EmployeeSurname", "EmployeeName", "EmployeePatronymic", "Post", "DateOfHiring", "Phone")] Staff obj)
        {
            if (ModelState.IsValid)
            {
                _db.Staff.Update(obj); // обновить запись в БД
                _db.SaveChanges();
                TempData["success"] = "Сотрудник успешно изменен!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET ACTION METHOD
        [HttpGet]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
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
        [ValidateAntiForgeryToken] 
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
        public IActionResult Delete(Staff obj)
        {
            var employeeFromDb = _db.Staff.Find(obj.EmployeeId);

            if (employeeFromDb == null)
            {
                return NotFound();
            }
            _db.Staff.Remove(employeeFromDb); // удалить из базы данных
            _db.SaveChanges();
            TempData["success"] = "Сотрудник успешно удален!";
            return RedirectToAction("Index");
        }

        //Экспорт в эксель таблицы сотрудников
        public IActionResult ExportToExcel()
        {
            var query = _db.Staff
                                .Join(_db.Posts, s => s.Post, p => p.PostId, (s, p) => new { Staff = s, Post = p })
                                .Select(result => new
                                {
                                    result.Staff.EmployeeId,
                                    result.Staff.EmployeeSurname,
                                    result.Staff.EmployeeName,
                                    result.Staff.EmployeePatronymic,
                                    Post = result.Post.Post1,  
                                    result.Staff.DateOfHiring,
                                    result.Staff.Phone,
                                    result.Post.SalaryAmount
                                })
                                .OrderByDescending(result => result.EmployeeSurname)
                                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Установка контекста лицензирования
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Отчет");

            ws.Cells["A1"].Value = "Отчет";
            ws.Cells["B1"].Value = "Отчет по персоналу";

            ws.Cells["A3"].Value = "Дата";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} в {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Фамилия";
            ws.Cells["B6"].Value = "Имя";
            ws.Cells["C6"].Value = "Отчество";
            ws.Cells["D6"].Value = "Должность";
            ws.Cells["E6"].Value = "Дата найма";
            ws.Cells["F6"].Value = "Номер телефона";
            ws.Cells["G6"].Value = "Заработная плата";

            #region color
            ws.Cells["A6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["A6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            ws.Cells["B6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["B6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            ws.Cells["C6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["C6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            ws.Cells["D6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["D6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            ws.Cells["E6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["E6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            ws.Cells["F6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["F6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            ws.Cells["G6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["G6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
            #endregion


            int rowStart = 7;
            foreach (var item in query)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.EmployeeSurname;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.EmployeeName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.EmployeePatronymic;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Post;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.DateOfHiring.ToString("dd.MM.yyyy");
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Phone;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.SalaryAmount;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment; filename=ExcelReport.xlsx");
            byte[] fileBytes = pck.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelReport.xlsx");
        }
    }
}
