using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Drawing.Drawing2D;
using System.Security.Authentication.ExtendedProtection;
using System.Web.WebPages;
using System.Web;

namespace HSEPractice2.Controllers
{
    public class DashboardController : Controller
    {

        private readonly WaterparkContext _db;

        public DashboardController(WaterparkContext db)
        {
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
			DateTimeOffset currentDate = DateTimeOffset.UtcNow;
			DateTimeOffset startOfWeek = currentDate.AddDays(-7);
			DateTimeOffset endOfWeek = startOfWeek.AddDays(7);

			int averageIncome = (int)await _db.Results
				.Where(r => r.Workday >= startOfWeek && r.Workday <= endOfWeek)
				.Select(r => r.TotalAmount)
				.AverageAsync();


			// Средний итог по выручке 
			ViewBag.AverageIncome = averageIncome.ToString();

            // Общее количество сотрудников в аквапарке 
            int totalEmployees = _db.Staff.Count();
            ViewBag.TotalEmployees = totalEmployees.ToString();

            // Количество аттракционов
            int totalAttractions = _db.Attractions.Count();
            ViewBag.TotalAttractions = totalAttractions.ToString();

            
            return View();
        }

        [HttpPost]
        public List<object> GetSalesData()
        {
			List<object> data = new List<object>();

			// Код для первой диаграммы
			DateTimeOffset currentDate = DateTimeOffset.UtcNow.UtcDateTime;
			DateTimeOffset startOfWeek = currentDate.AddDays(-7);
			DateTimeOffset endOfWeek = startOfWeek.AddDays(7);

			List<string> labels = _db.Results
				.Where(p => p.Workday >= startOfWeek && p.Workday <= endOfWeek)
				.OrderByDescending(p => p.Workday)
				.Take(7)
				.OrderBy(p => p.Workday)
				.Select(p => p.Workday.ToString())
				.ToList();

			data.Add(labels);

			List<decimal?> SalesNumber = _db.Results
				.Where(p => p.Workday >= startOfWeek && p.Workday <= endOfWeek)
				.OrderByDescending(p => p.Workday)
				.Take(7)
				.OrderBy(p => p.Workday)
				.Select(p => p.TotalAmount)
				.ToList();

			data.Add(SalesNumber);


			// Код для второй диаграммы
			List<string> serviceLabels = new List<string>();
            List<decimal?> serviceCounts = new List<decimal?>();

            DateTime startDate = DateTime.Now.Date.AddDays(-6); // Вычисляем начальную дату (сегодня - 6 дней)
            DateTime endDate = DateTime.Now.Date.AddDays(1); // Вычисляем конечную дату (сегодня)

            var serviceData = _db.OperationsHistories
                .Where(o => o.DateTimePurchase >= startDate && o.DateTimePurchase <= endDate)
                .Join(_db.Services, o => o.ServiceName.ToString(), s => s.ServiceId.ToString(), (o, s) => new { Service = s, Count = o.ServiceCount })
                .GroupBy(x => x.Service.ServiceName)
                .Select(g => new
                {
                    ServiceName = g.Key,
                    TotalCount = g.Sum(x => x.Count)
                })
                .OrderByDescending(x => x.TotalCount)
                .Take(5)
                .ToList();

            foreach (var item in serviceData)
            {
                serviceLabels.Add(item.ServiceName);
                serviceCounts.Add(item.TotalCount);
            }

            data.Add(serviceLabels);
            data.Add(serviceCounts);

            return data;
        }

		//Экспорт в эксель столбчатая диаграмма
		public IActionResult ExportToExcelStolb()
		{
			List<object> data = new List<object>();

			// Код для первой диаграммы
			DateTimeOffset currentDate = DateTimeOffset.UtcNow.UtcDateTime;
			DateTimeOffset startOfWeek = currentDate.AddDays(-7);
			DateTimeOffset endOfWeek = startOfWeek.AddDays(7);

			List<string> labels = _db.Results
				.Where(p => p.Workday >= startOfWeek && p.Workday <= endOfWeek)
				.OrderByDescending(p => p.Workday)
				.Take(7)
				.OrderBy(p => p.Workday)
				.Select(p => p.Workday.ToString())
				.ToList();

			data.Add(labels);

			List<decimal?> SalesNumber = _db.Results
				.Where(p => p.Workday >= startOfWeek && p.Workday <= endOfWeek)
                .OrderByDescending(p => p.Workday)
                .Take(7)
                .OrderBy(p => p.Workday)
                .Select(p => p.TotalAmount)
                .ToList();
            data.Add(SalesNumber);


			ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Установка контекста лицензирования
			ExcelPackage pck = new ExcelPackage();
			ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Отчет");

			ws.Cells["A1"].Value = "Отчет";
			ws.Cells["B1"].Value = "Отчет по выручкам";

			ws.Cells["A2"].Value = "Период";
			ws.Cells["B2"].Value = $"В период с {startOfWeek.UtcDateTime} по {endOfWeek.UtcDateTime}";

			ws.Cells["A3"].Value = "Дата";
			ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} в {0:H: mm tt}", DateTimeOffset.Now);

			ws.Cells["A6"].Value = "Дата";
			ws.Cells["B6"].Value = "Итоговая выручка";
			ws.Cells["A6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			ws.Cells["A6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

			ws.Cells["B6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			ws.Cells["B6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

			int rowStart = 7;
			for (int i = 0; i < labels.Count; i++)
			{
				ws.Cells[string.Format("A{0}", rowStart)].Value = labels[i];
				ws.Cells[string.Format("B{0}", rowStart)].Value = SalesNumber[i];
				rowStart++;
			}

			ws.Cells["A:AZ"].AutoFitColumns();
			Response.Clear();
			Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			Response.Headers.Add("content-disposition", "attachment; filename=ExcelReport.xlsx");
			byte[] fileBytes = pck.GetAsByteArray();
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelReport.xlsx");
		}

		//Экспорт в эксель круговая диаграмма
		public IActionResult ExportToExcel()
        {
			DateTime startDate = DateTime.Now.Date.AddDays(-6); // Вычисляем начальную дату (сегодня - 6 дней)
			DateTime endDate = DateTime.Now.Date.AddDays(1); // Вычисляем конечную дату (сегодня)
															 //startDate && o.DateTimePurchase <= endDate)

			var serviceData = _db.OperationsHistories
				.Where(o => o.DateTimePurchase >= startDate && o.DateTimePurchase <= endDate)
				.Join(_db.Services, o => o.ServiceName.ToString(), s => s.ServiceId.ToString(), (o, s) => new { Service = s, Count = o.ServiceCount })
				.GroupBy(x => x.Service.ServiceName)
				.Select(g => new
				{
					ServiceName = g.Key,
					TotalCount = g.Sum(x => x.Count)
				})
				.OrderByDescending(x => x.TotalCount)
				.Take(5)
				.ToList();

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Установка контекста лицензирования
			ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Отчет");

            ws.Cells["A1"].Value = "Отчет";
            ws.Cells["B1"].Value = "Отчет по топ-5 самым популярный услугам";

			ws.Cells["A2"].Value = "Период";
			ws.Cells["B2"].Value = $"В период с {startDate} по {endDate}";

			ws.Cells["A3"].Value = "Дата";
			ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} в {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Наименование услуги";
            ws.Cells["B6"].Value = "Количество";


			ws.Cells["A6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			ws.Cells["A6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

			ws.Cells["B6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			ws.Cells["B6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

			int rowStart = 7;
            foreach (var item in serviceData)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ServiceName;
				ws.Cells[string.Format("B{0}", rowStart)].Value = item.TotalCount;
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