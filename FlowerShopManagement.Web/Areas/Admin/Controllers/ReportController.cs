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
        private readonly Dictionary<int, string> strMonths = new Dictionary<int, string>
        {
            { 1, "Jan" },
            { 2, "Feb" },
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "Aug" },
            { 9, "Sep" },
            { 10, "Oct" },
            { 11, "Nov" },
            { 12, "Dec" }
        };

        private DateTime beginDate; 
        private DateTime endDate;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;

            var today = DateTime.Today;
            beginDate = new DateTime(today.Year, today.Month, 01);
            endDate = beginDate.AddMonths(1);
        }

        public IActionResult Index(DateTime? date = null, int? month = null, int? year = null)
        {
            ViewBag.Report = true;

            

            if (date is null && year is null && month is null)
            {
                ViewData["Date"] = "MM/dd/yyyy";
                ViewData["Year"] = DateTime.Today.Year;
                ViewData["Month"] = DateTime.Today.Month;
            }
            else
            {
                ViewData["Date"] = date is null ? "MM/dd/yyyy" : date.Value.ToString("MM/dd/yyyy");
                ViewData["Year"] = year;
                ViewData["Month"] = month;
            }

            if (month != null)
            {
                if (year == null)
                    beginDate = new DateTime(DateTime.Today.Year, (int)month, 01);
                else
                    beginDate = new DateTime((int)year, (int)month, 01);

                endDate = beginDate.AddMonths(1);
            }
            else
            if (year != null)
            {
                beginDate = new DateTime((int)year, 01, 01);
                endDate = beginDate.AddYears(1);
            }

            if (date != null)
            {
                beginDate = (DateTime)date;
                endDate = beginDate.AddDays(1);
            }

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
            ViewData["LowOnStocks"] = _reportService.GetProductsCount(20) - (int)ViewData["OutOfStocks"];
            ViewData["ProductsCount"] = _reportService.GetProductsCount(-1);

            // Sum-up
            ViewData["TopCustomers"] = _reportService.GetValuableCustomers(beginDate, endDate);
            ViewData["ProfitableProducts"] = _reportService.GetProfitableProducts(beginDate, endDate);

            return View();
        }

        private Chart GenerateLineChart(List<double?> dataSet1, List<double?> dataSet2)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Line;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            data.Labels = new List<string>();

            List<double?> dataValues = new List<double?>();
            List<ChartColor> colors = new List<ChartColor>();
            List<ChartColor> borderColors = new List<ChartColor>();

            int index = 0;

            for (int i = 1; i <= dataSet1.Count; i++)
            {
                if (dataSet1.Count == 24)
                {
                    data.Labels.Add((i-1).ToString() + ((i - 1) > 11 ? "PM" : "AM"));
                }
                else if (dataSet1.Count >= 28 && dataSet1.Count <= 31)
                {
                    var thisMonth = beginDate.Month;
                    var thisYear = beginDate.Year;
                    data.Labels.Add(i.ToString() + "-" + strMonths[thisMonth] + "-" + thisYear.ToString());
                }
                else
                {
                    data.Labels.Add(strMonths[i]);
                }

                 
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
