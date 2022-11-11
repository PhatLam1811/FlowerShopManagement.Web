using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Supplier = true;
            return View();
        }
    }
}
