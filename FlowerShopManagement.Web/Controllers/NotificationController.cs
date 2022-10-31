using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
