using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Authorize(Policy = "StaffOnly")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<SupplierModel>? supplier = await _supplierService.GetAllAsync(0, 10);
        return View(supplier);
    }

    // CREATE
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [ActionName("CreateAsync")]
    public async Task<IActionResult> CreateAsync(SupplierModel model)
    {
        try
        {
            var isSuccess = await _supplierService.AddOneAsync(model);

            if (isSuccess)
                return Redirect("~/Admin/Supplier");
            else
                return NotFound();
        }
        catch (Exception e)
        { 
            throw new Exception(e.Message); 
        }
    }

    // EDIT
    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            SupplierModel? model = await _supplierService.GetOneAsync(id);

            // Supplier not found
            if (model is null) return NotFound();

            return View(model);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPost("Edit")]
    [ActionName("EditAsync")]
    public async Task<IActionResult> EditAsync(SupplierModel supplier)
    {
        try
        {
            var isSuccess = await _supplierService.UpdateOneAsync(supplier);

            if (isSuccess)
                return Redirect("~/Admin/Supplier");
            else
                return NotFound();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    // DELETE
    [HttpPost("Remove")]
    [ActionName("RemoveAsync")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        try
        {
            var isSuccess = await _supplierService.RemoveOneAsync(id);

            if (isSuccess)
                return Redirect("~/Admin/Supplier");
            else
                return NotFound();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
