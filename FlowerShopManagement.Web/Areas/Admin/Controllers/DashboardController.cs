using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "StaffOnly")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Dashboard = true;
        return View();
    }
}
