using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.WebAPI.ViewModels.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class ImportController : Controller
{
    private readonly IStaffService _staffService;
    private readonly IStockService _stockService;
    private readonly IProductRepository _productRepository;

    public ImportController(
        IStaffService staffService,
        IStockService stockService,
        IProductRepository productRepository)
    {
        _staffService = staffService;
        _stockService = stockService;
        _productRepository = productRepository;
    }

    // ============ IMPORT PAGE ============
    [HttpGet]
    public ImportIndexVM/*IActionResult*/ Index()
    {
        // Load data
        var lowOnStockProducts = _stockService.GetLowOnStockProducts(_productRepository);
        var suppliers = _staffService.GetSuppliersAsync().Result;

        // Parse to view model
        var viewModel = new ImportIndexVM();
        viewModel.LowOnStockProductModels = lowOnStockProducts;
        viewModel.suppliers = suppliers;

        return viewModel;

        //return View(viewModel);
    }

    // ============ CREATE SUPPLY FORM ACTION ============
    //[HttpPost]
    //public SupplyFormModel /*IActionResult*/ CreateSupplyForm()
    //{
    //    return new NotImplementedException();
    //}

    // ============ REQUEST SUPPLY ACTION ============

    // Action performed after the user clicks the "Create Form" button
    //[HttpGet]
    //public SupplyFormModel/*IActionResult*/ CreateSupplyRequestForm(List<LowOnStockProductModel> supplyList, List<SupplierModel> supplierList)
    //{
    //    var supplyFormModel = new SupplyFormModel(supplyList, supplierList);

    //    return supplyFormModel;
    //}

    // Action performed after user clicks the "Request" button
    [HttpPost]
    public bool/*IActionResult*/ RequestSupply(SupplyFormModel supplyFormModel)
    {
        try
        {
            //_importServices.Request(supplyFormModel);
            
            // Successfully requested
            return true;
        }
        catch
        {
            // Failed to request
            return false;
        }
    }
}
