using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "StaffOnly")]
public class SupplierController : Controller
{
    private readonly IAuthService _authService;
    private readonly IPersonalService _personalService;
    private readonly IAdminService _adminService;
    private readonly IStaffService _staffService;
    public SupplierController(
    IAuthService authService,
    IAdminService adminService,
    IStaffService staffService,
    IPersonalService personalService)
    {
        _authService = authService;
        _adminService = adminService;
        _staffService = staffService;
        _personalService = personalService;
    }
    public async Task<IActionResult> Index()
    {
        ViewBag.Supplier = true;
        var supplier = await _staffService.GetAllSupplierDetailsAsync();
        return View(supplier);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new SupplierDetailModel());
    }


    [HttpPost]
    public async Task<IActionResult> Create(SupplierDetailModel model)
    {
        // Create
        bool result = false;
        try
        {
            result = await _adminService.AddSupplierAsync(model);
            if (result)
            {
                return RedirectToAction("Index");

            }
            else
            {
                return NotFound();
            }

        }
        catch { return NotFound(); }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var supplier = await _staffService.GetSupplierDetailAsync(id);
        return View(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SupplierDetailModel supplier)
    {
        var isSuccess = await _adminService.EditSupplierAsync(supplier);

        if (isSuccess)
            return RedirectToAction("Index", "Supplier");
        else
            return RedirectToAction("Edit", "Supplier");
    }

    public IActionResult Back()
    {
        return Redirect(Request.Headers["Referer"].ToString());
    }
}
