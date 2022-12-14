using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers;

public class ProfileController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Profile = true;
        return View();
    }

    [HttpGet]
    public IActionResult PersonalInformation()
    {
        return PartialView("PersonalInformation");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return PartialView("ChangePassword", new ChangePasswordModel());
    }

    [HttpGet]
    public IActionResult ManageAddress()
    {
        // load list infor
        return PartialView("ManageAddress", new List<InforDeliveryModel>());
    }

    [HttpGet]
    public IActionResult Voucher()
    {
        // load list voucher
        return PartialView("Voucher");
    }

    [HttpGet]
    public IActionResult ManageAccount()
    {
        return PartialView("ManageAccount");
    }
}
