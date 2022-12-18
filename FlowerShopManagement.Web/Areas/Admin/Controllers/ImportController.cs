using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

public class ImportController : Controller
{
    private readonly IStaffService _staffService;
    private readonly IStockService _stockService;
    private readonly IMailService _mailService;
    private readonly IImportService _importService;
    private readonly IProductRepository _productRepository;

    public ImportController(
        IStaffService staffService, 
        IStockService stockService,
        IMailService mailService,
        IImportService importService,
        IProductRepository productRepository)
    {
        _staffService = staffService;
        _stockService = stockService;
        _mailService = mailService;
        _importService = importService;
        _productRepository = productRepository;
    }

    // ========================== VIEWS ========================== //

    #region Views
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Load data
        var lowOnStockProducts = await _stockService.GetLowOnStockProducts(_productRepository);
        var suppliers = await _staffService.GetAllSuppliersAsync();

        // Configure viewmodel
        var viewmodel = new ImportViewModel(lowOnStockProducts, suppliers);

        return View(viewmodel);
    }

    [HttpGet]
    public IActionResult SupplyRequestForm(SupplyFormModel model)
    {
        return View(model);
    }
    #endregion

    // ========================== ACTIONS ========================== //

    #region Actions
    public async Task<IActionResult> CreateSupplyRequestFormAsync(List<string> selectedProductIds, List<int> amounts, List<string> supplierIds)
    {
        var products = new List<ProductDetailModel>();
        var suppilers = new List<SupplierModel>();

        // Get selected products detail
        foreach(var id in selectedProductIds)
        {
            var result = await _stockService.GetADetailProduct(id, _productRepository);
            products.Add(result);
        }

        // Get selected suppliers detail
        foreach (var id in supplierIds)
        {
            var result = await _staffService.GetSupplierAsync(id);
            if (result != null) suppilers.Add(result);
        }

        // Create supply request form model
        var requestForm = _importService.CreateSupplyForm(products, amounts, suppilers);

        if (requestForm != null)
            return SupplyRequestForm(requestForm); // Successfully created a new supply request form!
        else
            return NotFound(); // Failed to create a new supply request form!
    }

    public async Task<IActionResult> RequestSupply()
    {


        return await Index();
    }
    #endregion
}


public class ImportViewModel
{
    public List<ProductModel>? _lowOnStockProducts { get; set; }
    public List<SupplierModel>? _suppliers { get; set; }

    public ImportViewModel(
        List<ProductModel>? lowOnStockProducts, 
        List<SupplierModel>? suppliers)
    {
        _lowOnStockProducts = lowOnStockProducts;
        _suppliers = suppliers;
    }
}