using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers
{
    public class ProfileController : Controller
    {
        
        public IActionResult Index()
        {
            ViewBag.Profile = true;
            return View();
        }
    }
}
