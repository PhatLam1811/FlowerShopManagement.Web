using Microsoft.AspNetCore.Mvc;
using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
