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

		[Route("")]
		[Route("Index")]
		public IActionResult Index()
		{
			ViewBag.Dashboard = true;

			var today = DateTime.Today;
			var beginDate = new DateTime(today.Year, today.Month, 01);
			var endDate = today.AddDays(1);

			var dataSet = _reportService.GetTotalOrders(today, endDate);

			Chart verticalBarChart = GenerateVerticalBarChart(dataSet);

			// Day
			ViewData["Today"] = today;

			// Current staff info
			ViewData["Username"] = HttpContext.User.FindFirst("Username")?.Value;
			ViewData["Email"] = HttpContext.User.FindFirst("Email")?.Value;
			ViewData["Avatar"] = HttpContext.User.FindFirst("Avatar")?.Value;

			// Today's total orders
			ViewData["VerticalBarChart"] = verticalBarChart;

            // Current month's statistic
            ViewData["WaitingOrder"] = _reportService.GetOrdersCount(beginDate, endDate, Core.Enums.Status.Waiting);
            ViewData["ValuableCustomers"] = _reportService.GetValuableCustomers(beginDate, endDate);
            ViewData["ProfitableProducts"] = _reportService.GetProfitableProducts(beginDate, endDate);
            ViewData["OutOfStocksCount"] = _reportService.GetProductsCount(0);
            ViewData["LowOnStocksCount"] = _reportService.GetProductsCount(20) - (int)ViewData["OutOfStocksCount"];

            return View();
		}

		private Chart GenerateVerticalBarChart(List<double?> dataSet)
		{
			Chart chart = new Chart();
			chart.Type = Enums.ChartType.Bar;

			ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

			data.Labels = new List<string>();

			List<double?> dataValues = new List<double?>();
			List<ChartColor> colors = new List<ChartColor>();
			List<ChartColor> borderColors = new List<ChartColor>();

			int index = 0;

			for (int i = 0; i < dataSet.Count; i++)
			{
				data.Labels.Add(i.ToString() + (i > 11 ? "PM" : "AM"));
				dataValues.Add(0);
				colors.Add(ChartColor.FromRgba(102, 152, 250, 1));
				borderColors.Add(ChartColor.FromRgb(102, 152, 250));

				index++;
			}


			var dataset = new BarDataset
			{
				Label = "Total per hour",
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
