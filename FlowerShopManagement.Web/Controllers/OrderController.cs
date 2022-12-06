using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers
{
	public class OrderController : Controller
	{
		public IActionResult Index()
		{
            ViewBag.Order = true;
            return View();
		}
	}
}		
