using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "StaffOnly")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
