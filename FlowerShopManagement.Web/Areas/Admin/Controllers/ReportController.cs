using Microsoft.AspNetCore.Authorization;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using ChartJSCore.Models.ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;
using FlowerShopManagement.Application.Interfaces;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    [Authorize(Policy = "StaffOnly")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            ViewBag.Report = true;

            Chart donutChart = GenerateDonutChart();
            Chart lineChart = GenerateLineChart();

            ViewData["DonutChart"] = donutChart;
            ViewData["LineChart"] = lineChart;

            return View();
        }

        private Chart GenerateLineChart()
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Line;

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


            var dataset = new LineDataset
            {
                Label = "Numbers of order",
                Data = new List<double?>() { 0, 40, 20, 36, 50, 23, 100 },
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = colors,
                BorderColor = borderColors,
                BorderWidth = new List<int> { 4 },
                PointBorderWidth = new List<int> { 8 },
                PointHoverBorderWidth = new List<int> { 10 },
                PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(102, 152, 250, 0.4) },
            };

            var dataset1 = new LineDataset
            {
                Label = "Total of order",
                Data = new List<double?>() { 0, 10, 20, 16, 50, 23, 33 },
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(214, 103, 191, 1) },
                BorderColor = new List<ChartColor>() { ChartColor.FromRgba(214, 103, 191, 1) },
                BorderWidth = new List<int> { 4 },
                PointBorderWidth = new List<int> { 8 },
                PointHoverBorderWidth = new List<int> { 10 },
                PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(214, 103, 191, 0.4) },
            };

            data.Datasets = new List<Dataset> { dataset, dataset1 };

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
