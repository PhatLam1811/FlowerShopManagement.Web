using Microsoft.AspNetCore.Mvc;
using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductDetail()
        {
            return View(new ProductDetailModel(amount: 20, id: "huhu") { Name = "F hoa", UniPrice = 10 });
        }
    }
}
