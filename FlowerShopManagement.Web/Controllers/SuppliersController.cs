using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers
{
    public class SuppliersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ActionName("AddSupplier")]
        public IActionResult Add()
        {
            return View();
        }

        [ActionName("EditSupplier")]
        public IActionResult Edit()
        {
            return View();
        }

        [ActionName("DeleteSupplier")]
        public IActionResult Delete()
        {
            return View();
        }

        [ActionName("DetailSupplier")]
        public IActionResult Detail()
        {
            return View();
        }
    }
}
