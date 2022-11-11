using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VoucherController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Voucher = true;
            return View();
        }
    }
}
