using ChartJSCore.Helpers;
using ChartJSCore.Models;
using ChartJSCore.Models.ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Report = true;

            Chart verticalBarChart = GenerateVerticalBarChart();
            Chart donutChart = GenerateDonutChart();

            ViewData["VerticalBarChart"] = verticalBarChart;
            ViewData["DonutChart"] = donutChart;

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

        private Chart GenerateDonutChart()
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Doughnut;

            var dataset = new DoughnutDataset
            {
                Label = "Numbers of order",
                Data = new List<double?>() { 23, 41, 17, 29 },
                BackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#a4c2a3"), ChartColor.FromHexString("#6698fa"), ChartColor.FromHexString("#e5573c"), ChartColor.FromHexString("#eea13d") },
                Spacing = 6,
            };
             
            var data = new Data();

            data.Datasets = new List<Dataset>() { dataset };

            chart.Data = data;

            return chart;
        }
    }
}
