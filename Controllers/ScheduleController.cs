using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using static HSEPractice2.Constants;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using HSEPractice2.Core.Repositories;

namespace HSEPractice2.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly WaterparkContext _db;

        public ScheduleController(WaterparkContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index([FromServices] UserManager<ApplicationUser> userManager)
        {
            var currentUsername = User.Identity.Name;

            var currentUser = await userManager.FindByNameAsync(currentUsername);

            //Получить по фамилии айдишник сотрудника
            int userId = _db.Staff.FirstOrDefault(s => s.EmployeeSurname == currentUser.LastName 
                                                  && s.EmployeeName == currentUser.FirstName 
                                                  && s.EmployeePatronymic == currentUser.Patronymic).EmployeeId;
            bool hasAccess = User.IsInRole(Constants.Roles.Administrator);
            ViewBag.HasAccess = hasAccess;

            IEnumerable<Shift> objServiceList = _db.Shifts.OrderByDescending(s => s.ShiftDate).ToList();
            IEnumerable<CurrentShift> objCurList = _db.GetShifts().OrderByDescending(s => s.Shift).ToList();

            var viewModel = new Tuple<IEnumerable<Shift>, IEnumerable<CurrentShift>, int>(objServiceList, objCurList, userId);

            return View(viewModel);
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Create([Bind($"{nameof(Shift.ShiftId)},{nameof(Shift.ShiftDate)}, {nameof(Shift.WorkShedule)}, {nameof(Shift.CurrentShifts)}")] Shift obj)
        {
			// Преобразование локального времени в UTC
			obj.ShiftDate = obj.ShiftDate.ToUniversalTime();
			if (_db.Shifts.Any(s => s.ShiftDate == obj.ShiftDate))
			{
				ModelState.AddModelError("ShiftDate", "Смена в эту дату уже существует.");
				TempData["error"] = "Смена НЕ создана!";
				return View();
			}
			if (ModelState.IsValid)
            {
                _db.Shifts.Add(obj); // добавить в базу данных
                _db.SaveChanges();
                TempData["success"] = "Смена успешно создана!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var shiftFromDb = _db.Shifts.Find(id);

            if (shiftFromDb == null)
            {
                return NotFound();
            }
            return View(shiftFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}, {Constants.Roles.Director}")]
        public IActionResult Edit([Bind($"{nameof(Shift.ShiftId)},{nameof(Shift.ShiftDate)}, {nameof(Shift.WorkShedule)}, {nameof(Shift.CurrentShifts)}")] Shift obj)
        {
            // Преобразование локального времени в UTC
            obj.ShiftDate = obj.ShiftDate.ToUniversalTime();
            if (_db.Shifts.Any(s => s.ShiftDate == obj.ShiftDate && s.ShiftId != obj.ShiftId))
            {
                ModelState.AddModelError("ShiftDate", "Смена в эту дату уже существует.");
                TempData["error"] = "Смена НЕ изменена!";
            }

            if (ModelState.IsValid)
            {
                _db.Shifts.Update(obj); 
                _db.SaveChanges();
                TempData["success"] = "Смена успешно изменена!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

		// GET ACTION METHOD
		[HttpGet]
		[Authorize(Roles = $"{Constants.Roles.Administrator}")]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var shiftFromDb = _db.Shifts.Find(id);

			if (shiftFromDb == null)
			{
				return NotFound();
			}
			return View(shiftFromDb);
		}

		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = $"{Constants.Roles.Administrator}")]
		public IActionResult Delete(Shift obj)
		{
			var shiftFromDb = _db.Shifts.Find(obj.ShiftId);

			if (shiftFromDb == null)
			{
				return NotFound();
			}
			_db.Shifts.Remove(shiftFromDb);
			_db.SaveChanges();
			TempData["success"] = "Смена успешно удалена!";
			return RedirectToAction("Index");
		}


        public IActionResult CreateCur()
        {
            ViewBag.Shi = new SelectList(_db.Shifts.OrderByDescending(s=>s.ShiftDate), "ShiftId", "ShiftDate");
            var cursh = new CurrentShift();
            cursh.ShiftNavigation = new Shift();
            return View(cursh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCurAsync(
		[Bind($"{nameof(CurrentShift.CurrentShiftId)},{nameof(CurrentShift.Shift)}, {nameof(CurrentShift.Employee)}, {nameof(CurrentShift.EmployeeNavigation)}, {nameof(CurrentShift.ShiftNavigation)}")] CurrentShift obj,
		[FromServices] UserManager<ApplicationUser> userManager) 
		{
			if (ModelState.IsValid)
			{
				var currentUsername = User.Identity.Name; 

				var currentUser = await userManager.FindByNameAsync(currentUsername);

                //Получить по фамилии айдишник сотрудника
                int i = _db.Staff.FirstOrDefault(s => s.EmployeeSurname == currentUser.LastName && s.EmployeeName == currentUser.FirstName).EmployeeId;
                obj.Employee = i;

				//Проверить есть ли уже такая запись в базе данных
				if (_db.GetShifts().Any(s => s.Shift == obj.Shift && s.Employee == obj.Employee))
				{
					ModelState.AddModelError("Shift", "Смена в эту дату для данного сотрудника уже существует.");
					TempData["error"] = "Текущая смена НЕ создана!";
					ViewBag.Shi = new SelectList(_db.Shifts.OrderByDescending(s => s.ShiftDate), "ShiftId", "ShiftDate");
					var shift = new CurrentShift();
					return View(shift);
				}

				_db.CurrentShifts.Add(obj); // добавить в базу данных
				_db.SaveChanges();
				TempData["success"] = "Текущая смена успешно создана!";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//GET ACTION METHOD
		public IActionResult EditCur(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var curshFromDb = _db.GetShifts().Find(s=>s.CurrentShiftId==id);
			ViewBag.Shi = new SelectList(_db.Shifts.OrderByDescending(s => s.ShiftDate), "ShiftId", "ShiftDate");

			if (curshFromDb == null)
			{
				return NotFound();
			}
			return View(curshFromDb);
		}

		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditCurAsync(
		[Bind($"{nameof(CurrentShift.CurrentShiftId)},{nameof(CurrentShift.Shift)}, {nameof(CurrentShift.Employee)}, {nameof(CurrentShift.EmployeeNavigation)}, {nameof(CurrentShift.ShiftNavigation)}")] CurrentShift obj,
		[FromServices] UserManager<ApplicationUser> userManager)
		{
			if (ModelState.IsValid)
			{
				//Проверить есть ли уже такая запись в базе данных
				if (_db.GetShifts().Any(s => s.Shift == obj.Shift && s.Employee == obj.Employee && s.CurrentShiftId != obj.CurrentShiftId))
				{
					ModelState.AddModelError("Shift", "Смена в эту дату для данного сотрудника уже существует.");
					TempData["error"] = "Текущая смена НЕ изменена!";
					ViewBag.Shi = new SelectList(_db.Shifts.OrderByDescending(s => s.ShiftDate), "ShiftId", "ShiftDate");
					var shift = new CurrentShift();
					return View(shift);
				}

				var existingEntity = _db.CurrentShifts.Find(obj.CurrentShiftId);
				if (existingEntity != null)
				{
					// Обновляем существующую сущность новыми значениями
					existingEntity.Shift = obj.Shift;
					existingEntity.Employee = obj.Employee;

					_db.SaveChanges();
					TempData["success"] = "Текущая смена успешно изменена!";
					return RedirectToAction("Index");
				}
				else
				{
					ModelState.AddModelError("", "Запись не найдена.");
					return View(obj);
				}
			}
			return View(obj);
		}

		// GET ACTION METHOD
		[HttpGet]
        public IActionResult DeleteCur(int? id)
        {
			List<CurrentShift> curShiftList = _db.GetShifts().OrderBy(s => s.ShiftNavigation.ShiftDate).ToList();
			if (id == null || id == 0)
            {
                return NotFound();
            }
            var shiftFromDb = curShiftList.Find(s => s.CurrentShiftId == id);

            if (shiftFromDb == null)
            {
                return NotFound();
            }
            return View(shiftFromDb);
        }


		// POST ACTION METHOD
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCur(CurrentShift obj)
        {
            var curShiftFromDb = _db.CurrentShifts.Find(obj.CurrentShiftId);

            if (curShiftFromDb == null)
            {
                return NotFound();
            }
            _db.CurrentShifts.Remove(curShiftFromDb); // удалить из базы данных
            _db.SaveChanges();
            TempData["success"] = "Смена успешно удалена!";
            return RedirectToAction("Index");
        }

        //Экспорт в эксель круговая диаграмма
        public IActionResult ExportToExcel()
        {
            var currentDate = DateTime.Now;
            var currentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var startDate = currentMonth.ToUniversalTime();
            var endDate = currentMonth.AddMonths(1).ToUniversalTime();

            var query = from cs in _db.CurrentShifts
                        join s in _db.Shifts on cs.Shift equals s.ShiftId
                        join st in _db.Staff on cs.Employee equals st.EmployeeId
                        where cs.ShiftNavigation.ShiftDate >= startDate && cs.ShiftNavigation.ShiftDate <= endDate
                        group cs by new
                        {
                            st.EmployeeSurname,
                            st.EmployeeName,
                            st.EmployeePatronymic
                        } into g
                        orderby g.Key.EmployeeSurname ascending
                        select new
                        {
                            g.Key.EmployeeSurname,
                            g.Key.EmployeeName,
                            g.Key.EmployeePatronymic,
                            ShiftsCount = g.Count()
                        };
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Установка контекста лицензирования
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Отчет");

            ws.Cells["A1"].Value = "Отчет";
            ws.Cells["B1"].Value = "Отчет по производительности сотрудников";

            ws.Cells["A2"].Value = "Период";
            ws.Cells["B2"].Value = $"В период с {startDate} по {endDate}";

            ws.Cells["A3"].Value = "Дата";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} в {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Фамилия";
            ws.Cells["B6"].Value = "Имя";
            ws.Cells["C6"].Value = "Отчество";
            ws.Cells["D6"].Value = "Кол-во смен за период";

            int rowStart = 7;
            foreach (var item in query)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.EmployeeSurname;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.EmployeeName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.EmployeePatronymic;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ShiftsCount;
                if(item.ShiftsCount < 15)
                {
                    ws.Cells[string.Format("A{0}:D{0}", rowStart)].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[string.Format("A{0}:D{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
                }
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
