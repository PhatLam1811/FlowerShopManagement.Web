using ChartJSCore.Helpers;
using ChartJSCore.Models;
using FlowerShopManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    [Authorize(Policy = "StaffOnly")]
    public class DashboardController : Controller
    {
        private readonly IReportService _reportService;

        public DashboardController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            ViewBag.Dashboard = true;

            //var beginDate = DateTime.Today;
            //var endDate = DateTime.Today.AddDays(1);

            var beginDate = new DateTime(2022, 01, 01);
            var endDate = new DateTime(2023, 01, 01);

            _reportService.GetTotalSum(beginDate, endDate, "$month", Core.Enums.Status.Purchased);
            
            var dataSet = new List<double?>();

            Chart verticalBarChart = GenerateVerticalBarChart(dataSet);

            ViewData["VerticalBarChart"] = verticalBarChart;

            return View();
        }

        private Chart GenerateVerticalBarChart(List<double?> dataSet)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            data.Labels = new List<string>();

            List<double?> dataValues = new List<double?>();
            List<ChartColor> colors = new List<ChartColor>();
            List<ChartColor> borderColors = new List<ChartColor>();

            int index = 0;

            for (int i = 0; i < 24; i++)
            {
                data.Labels.Add(i.ToString());
                dataValues.Add(0);
                colors.Add(ChartColor.FromRgba(102, 152, 250, 1));
                borderColors.Add(ChartColor.FromRgb(102, 152, 250));

                index++;
            }


            var dataset = new BarDataset
            {
                Label = "Numbers of order",
                Data = dataSet,
                BackgroundColor = colors,
                BorderColor = borderColors,
                BorderWidth = new List<int> { 4 },
                BarPercentage = 0.5,
                BarThickness = 6,
                MaxBarThickness = 8,
                MinBarLength = 2,
            };

            data.Datasets = new List<Dataset> { dataset };

            chart.Data = data;

            return chart;
        }
    }
}
