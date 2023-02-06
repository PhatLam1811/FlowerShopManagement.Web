using Microsoft.AspNetCore.Authorization;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using ChartJSCore.Models.ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;

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

            var beginDate = new DateTime(2023, 01, 01);
            var endDate = beginDate.AddMonths(1);

            // Analize data
            var dataSet1 = _reportService.GetTotalOrders(beginDate, endDate);
            var dataSet2 = _reportService.GetTotalRevenue(beginDate, endDate);
            var dataSet3 = _reportService.GetCategoryStatistic();

            Chart lineChart = GenerateLineChart(dataSet1, dataSet2);
            Chart donutChart = GenerateDonutChart(dataSet3);

            // Charts
            ViewData["LineChart"] = lineChart;
            ViewData["DonutChart"] = donutChart;

            // Orders Statistics
            ViewData["WaitingOrders"] = _reportService.GetOrdersCount(beginDate, endDate, Core.Enums.Status.Paying);
            ViewData["CanceledOrders"] = _reportService.GetOrdersCount(beginDate, endDate, Core.Enums.Status.Canceled);
            ViewData["CompletedOrders"] = _reportService.GetOrdersCount(beginDate, endDate);

            // Products Statistics
            ViewData["OutOfStocks"] = _reportService.GetProductsCount(0);
            ViewData["LowOnStocks"] = _reportService.GetProductsCount(20);
            ViewData["ProductsCount"] = _reportService.GetProductsCount(-1);

            // Sum-up
            ViewData["TopCustomers"] = _reportService.GetValuableCustomers(beginDate, endDate);
            ViewData["ProfitableProducts"] = _reportService.GetProfitableProducts(beginDate, endDate);

            return View();
        }

        [Route("Sort")]
        [HttpPost]
        public IActionResult Sort(string sortOrder, DateTime beginDate, DateTime endDate)
        {
            ViewBag.Report = true;
            
            // Analize data
            var dataSet1 = _reportService.GetTotalOrders(beginDate, endDate);
            var dataSet2 = _reportService.GetTotalRevenue(beginDate, endDate);
            var dataSet3 = _reportService.GetCategoryStatistic();

            Chart lineChart = GenerateLineChart(dataSet1, dataSet2);
            Chart donutChart = GenerateDonutChart(dataSet3);

            // Charts
            ViewData["LineChart"] = lineChart;
            ViewData["DonutChart"] = donutChart;

            // Orders Statistics
            ViewData["WaitingOrders"] = _reportService.GetOrdersCount(beginDate, endDate, Core.Enums.Status.Paying);
            ViewData["CanceledOrders"] = _reportService.GetOrdersCount(beginDate, endDate, Core.Enums.Status.Canceled);
            ViewData["CompletedOrders"] = _reportService.GetOrdersCount(beginDate, endDate);

            // Products Statistics
            ViewData["OutOfStocks"] = _reportService.GetProductsCount(0);
            ViewData["LowOnStocks"] = _reportService.GetProductsCount(20);
            ViewData["ProductsCount"] = _reportService.GetProductsCount(-1);

            // Sum-up
            ViewData["TopCustomers"] = _reportService.GetValuableCustomers(beginDate, endDate);
            ViewData["ProfitableProducts"] = _reportService.GetProfitableProducts(beginDate, endDate);

            return PartialView("_IndexPartial");
        }
        private Chart GenerateLineChart(List<double?> dataSet1, List<double?> dataSet2)
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

            for (int i = 1; i <= dataSet1.Count; i++)
            {
                data.Labels.Add("Day" + i.ToString());
                dataValues.Add(0);
                colors.Add(ChartColor.FromRgba(102, 152, 250, 1));
                borderColors.Add(ChartColor.FromRgb(102, 152, 250));

                index++;
            }


            var dataset1 = new LineDataset
            {
                Label = "Total Orders",
                Data = dataSet1,
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = colors,
                BorderColor = borderColors,
                BorderWidth = new List<int> { 4 },
                PointBorderWidth = new List<int> { 8 },
                PointHoverBorderWidth = new List<int> { 10 },
                PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(102, 152, 250, 0.4) },
            };

            var dataset2 = new LineDataset
            {
                Label = "Total Revenue",
                Data = dataSet2,
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(214, 103, 191, 1) },
                BorderColor = new List<ChartColor>() { ChartColor.FromRgba(214, 103, 191, 1) },
                BorderWidth = new List<int> { 4 },
                PointBorderWidth = new List<int> { 8 },
                PointHoverBorderWidth = new List<int> { 10 },
                PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(214, 103, 191, 0.4) },
            };

            data.Datasets = new List<Dataset> { dataset1, dataset2 };

            chart.Data = data;

            return chart;
        }

        private Chart GenerateDonutChart(List<CategoryStatisticModel>? dataSet)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Doughnut;

            if (dataSet == null) return chart;

            var numberOfProducts = new List<double?>();
            var labels = new List<string>();

            foreach (var category in dataSet)
            {
                labels.Add(category._id);
                numberOfProducts.Add(category.numberOfProducts);
            }

            var dataset = new DoughnutDataset
            {
                Label = "Numbers of Products",
                Data = numberOfProducts,
                BackgroundColor = new List<ChartColor>() 
                { 
                    ChartColor.FromHexString("#a4c2a3"), 
                    ChartColor.FromHexString("#6698fa"), 
                    ChartColor.FromHexString("#e5573c"), 
                    ChartColor.FromHexString("#eea13d") 
                },
                Spacing = 6,
            };
             
            var data = new Data();

            data.Labels = labels;

            data.Datasets = new List<Dataset>() { dataset };

            chart.Data = data;

            return chart;
        }
    }
}
