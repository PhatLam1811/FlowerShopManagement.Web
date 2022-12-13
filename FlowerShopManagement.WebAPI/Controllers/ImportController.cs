using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.WebAPI.ViewModels.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class ImportController : Controller
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IStockServices _stockService;
    private readonly IImportService _importServices;
    private readonly IProductRepository _productRepository;

    public ImportController(
        ISupplierRepository supplierRepository,
        IStockServices stockService,
        IImportService importServices,
        IProductRepository productRepository)
    {
        _supplierRepository = supplierRepository;
        _stockService = stockService;
        _importServices = importServices;
        _productRepository = productRepository;
    }

    // Main page of import operation
    [HttpGet]
    public ImportIndexVM/*IActionResult*/ Index()
    {
        // Load data
        var lowOnStockProducts = _stockService.GetLowOnStockProducts(_productRepository);
     
        // Need to encapsulate the code
        var suppliers = new List<SupplierModel>();
        var result = _supplierRepository.GetAll().Result;

        foreach (var supplier in result)
        {
            var model = new SupplierModel(supplier);
            suppliers.Add(model);
        }

        // Parse to view model
        var viewModel = new ImportIndexVM();
        viewModel.LowOnStockProductModels = lowOnStockProducts;
        viewModel.suppliers = suppliers;

        return viewModel;
    }

    // Action performed after the user clicks the "Create Form" button
    [HttpGet]
    public SupplyFormModel/*IActionResult*/ CreateSupplyRequestForm(List<LowOnStockProductModel> supplyList, List<SupplierModel> supplierList)
    {
        var supplyFormModel = new SupplyFormModel(supplyList, supplierList);

        return supplyFormModel;
    }

    // Action performed after user clicks the "Request" button
    [HttpPost]
    public bool/*IActionResult*/ RequestSupply(SupplyFormModel supplyFormModel)
    {
        try
        {
            _importServices.Request(supplyFormModel);
            
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
