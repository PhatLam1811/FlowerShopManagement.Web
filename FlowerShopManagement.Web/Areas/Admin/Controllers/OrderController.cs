using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Order = true;
            return View(new List<OrderModel>());
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
