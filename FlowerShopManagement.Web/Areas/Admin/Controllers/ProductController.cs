using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        public ProductController() { 

        }
        public IActionResult Index()
        {
            ViewBag.Product = true;


            return View();
        }


    }
}
