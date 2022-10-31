using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers
{
	public class WishListController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
