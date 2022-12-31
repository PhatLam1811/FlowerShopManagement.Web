using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers;

public class ProductDetailController : Controller
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
