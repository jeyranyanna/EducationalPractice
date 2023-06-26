using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace HSEPractice2.Controllers
{
    public class SalesController : Controller
    {
        private readonly WaterparkContext _db;

        public SalesController(WaterparkContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            bool hasAccess = User.IsInRole(Constants.Roles.Accountant) || User.IsInRole(Constants.Roles.Director);
            ViewBag.HasAccess = hasAccess;
            List<OperationsHistory> historyList = _db.GetHistoryList()
                                                            .Where(s => s.DateTimePurchase >= DateTime.Today && s.DateTimePurchase <= DateTime.Now)    
                                                            .OrderBy(s => s.DateTimePurchase)
                                                            .ToList();
            return View(historyList);
        }

        //Экспорт в эксель таблицы продаж
        public IActionResult ExportToExcel()
        {
            var query = _db.GetHistoryList()
                                .Where(s => s.DateTimePurchase >= DateTime.Today && s.DateTimePurchase <= DateTime.Now)
                                .OrderBy(s => s.DateTimePurchase)
                                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Установка контекста лицензирования
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Отчет");

            ws.Cells["A1"].Value = "Отчет";
            ws.Cells["B1"].Value = "Отчет по продажам";

            ws.Cells["A3"].Value = "Дата";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} в {0:H:mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Дата и время";
            ws.Cells["B6"].Value = "Браслет";
            ws.Cells["C6"].Value = "Услуга";
            ws.Cells["D6"].Value = "Количество";
            ws.Cells["E6"].Value = "Процент скидки";

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
            #endregion


            int rowStart = 7;
            foreach (var item in query)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.DateTimePurchase.ToString("dd.MM.yyyy hh:mm:ss");
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.BraceletId;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.ServiceNameNavigation.ServiceName;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ServiceCount;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.DiscountPercentageNavigation.DiscountPercentage;
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
