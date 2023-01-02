using ChartJSCore.Helpers;
using ChartJSCore.Models;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "StaffOnly")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Dashboard = true;

            Chart verticalBarChart = GenerateVerticalBarChart();

            ViewData["VerticalBarChart"] = verticalBarChart;

            return View();
        }

        private Chart GenerateVerticalBarChart()
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

            for (int i = 1; i <= 7; i++)
            {
                data.Labels.Add("Day" + i.ToString());
                dataValues.Add(0);
                colors.Add(ChartColor.FromRgba(102, 152, 250, 1));
                borderColors.Add(ChartColor.FromRgb(102, 152, 250));

                index++;
            }


            var dataset = new BarDataset
            {
                Label = "Numbers of order",
                Data = new List<double?>() { 0, 40, 20, 36, 50, 23, 100 },
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
