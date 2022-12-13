using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Supplier = true;
            return View(new List<SupplierDetailModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SupplierDetailModel());
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new SupplierDetailModel());
        }
    }
}
