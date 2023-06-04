using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using System.Security.Authentication.ExtendedProtection;

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
            // Средний итог по выручке 
            int averageIncome = (int)await _db.Results
                .Select(r => r.TotalAmount)
                .AverageAsync();
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
            List<string> labels = _db.Results.OrderByDescending(p => p.Workday).Take(7).Select(p => p.Workday.ToString()).ToList();
            data.Add(labels);
            List<decimal?> SalesNumber = _db.Results.OrderByDescending(p => p.TotalAmount).Take(7).Select(p => p.TotalAmount).ToList();
            data.Add(SalesNumber);

            // Код для второй диаграммы
            List<string> serviceLabels = new List<string>();
            List<decimal?> serviceCounts = new List<decimal?>();

            var serviceData = _db.OperationsHistories
                .Where(o => o.DateTimePurchase >= new DateTime(2022, 01, 01))
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
    }
}


