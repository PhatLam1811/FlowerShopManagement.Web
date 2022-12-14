using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.User = true;
            return View(new List<UserDetailsModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserDetailsModel());
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new UserDetailsModel());
        }
    }
}
