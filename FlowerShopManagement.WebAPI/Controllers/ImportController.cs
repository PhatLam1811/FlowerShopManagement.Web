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
    private readonly IEmailService _mailService;

    public ImportController(
        IStaffService staffService,
        IStockService stockService,
        IProductRepository productRepository,
        IEmailService mailService)
    {
        _staffService = staffService;
        _stockService = stockService;
        _productRepository = productRepository;
        _mailService = mailService;
    }

    // ============ IMPORT PAGE ============
    [HttpGet]
    public ImportIndexVM/*IActionResult*/ Index()
    {
        // Load data
        var lowOnStockProducts = _stockService.GetLowOnStockProducts(_productRepository);
        var suppliers = _staffService.GetAllSuppliersAsync().Result;

        // Parse to view model
        var viewModel = new ImportIndexVM();
        //viewModel.LowOnStockProductModels = lowOnStockProducts;
        viewModel.Suppliers = suppliers;

        return viewModel;

        //return View(viewModel);
    }

    // ============ CREATE & REVIEW SUPPLY FORM ACTION ============
    [HttpPost]
    public SupplyFormModel?/*IActionResult*/ CreateSupplyForm(ImportIndexVM viewModel)
    {
        var selectedItems = viewModel.SelectedItems;
        var selectedSuppliers = viewModel.SelectedSuppliers;

        // Requested items and suppliers must not be null
        if (selectedItems == null || selectedSuppliers == null) return null;

        // Fill in the form model
        //var supplyForm = new SupplyFormModel(selectedItems, selectedSuppliers);

        return null;

        //return View(supplyForm);
    }

    // ============ REQUEST SUPPLY ACTION ============
    [HttpPost]
    public async Task<IActionResult> RequestSupply(SupplyFormModel supplyForm)
    {
        // Model to message
        var mimeMessage = _mailService.CreateMimeMessage(supplyForm);

        // Send the request message to suppliers
        await _mailService.Send(mimeMessage);

        return View();
    }
}
