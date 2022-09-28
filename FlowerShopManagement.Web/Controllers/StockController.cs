using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ActionName("AddProduct")]
        public IActionResult Add()
        {
            return View();
        }

        [ActionName("EditProduct")]
        public IActionResult Edit()
        {
            return View();
        }

        [ActionName("DeleteProduct")]
        public IActionResult Delete()
        {
            return View();
        }

        [ActionName("DetailProduct")]
        public IActionResult Detail()
        {
            return View();
        }
    }
}
