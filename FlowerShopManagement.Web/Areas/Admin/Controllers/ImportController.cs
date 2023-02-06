using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Authorize(Policy = "StaffOnly")]
public class ImportController : Controller
{
    private readonly IStockService _stockService;
    private readonly ISupplierService _supplierService;
    private readonly IImportService _importService;
    private readonly IWebHostEnvironment _webHostEnv;

    private const string _reqTemplatePath = "/template/SupplyFormTemplate.html";

    public ImportController(
        IStockService stockService,
        IImportService importService,
        ISupplierService supplierService,
        IWebHostEnvironment webHostEnv)
    {
        _stockService = stockService;
        _importService = importService;
        _supplierService = supplierService;
        _webHostEnv = webHostEnv;
    }

    // IMPORT MAIN PAGE
    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            var requests = _importService.GetRequests();

            return View(requests);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    // REQUEST FORM
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        // Load data
        var lowOnStockProducts = _stockService.GetLowOnStockProducts();
        var suppliers = await _supplierService.GetAllAsync();

        // Configure viewmodel
        var viewmodel = new ImportViewModel(PaginatedList<ProductModel>.CreateAsync(lowOnStockProducts, 1, 100), suppliers);

        return View(viewmodel);
    }

    [HttpPost]
    [ActionName("CreateRequestForm")]
    public async Task CreateRequestForm(List<string> productIds, List<int> requestQty, string supplierIds)
    {
        var reqQty = new List<int>();

        var products = new List<ProductDetailModel>();
        for (int i = 0; i < productIds.Count; i++)
        {
            if (requestQty[i] <= 0) continue;

            var result = await _stockService.GetADetailProduct(productIds[i]);
            
            if (result == null) return;

            reqQty.Add(requestQty[i]);
            products.Add(result);
        }
        var supplier = await _supplierService.GetOneAsync(supplierIds);
        var htmlPath = _webHostEnv.WebRootPath + _reqTemplatePath;

        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = HttpContext.User.FindFirst("Username")?.Value;

        if (userId is null || userName is null) return;

        if (products.Count <= 0 || reqQty.Count <= 0 || supplier == null)
        {
            throw new Exception("Insufficient Values!");
        }

        var reqForm = _importService.CreateRequestForm(
            products,
            supplier,
            reqQty,
            userId, userName, htmlPath);

        try
        {
            await _importService.SendRequest(reqForm);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    // SUPPLY FORM
    [HttpGet("SupplyForm")]
    public IActionResult SupplyForm(ImportModel model)
    {
        return View(model);
    }
    
    // IMPORT DETAIL
    [HttpGet("Detail")]
    public async Task<IActionResult> Detail(string id, string? alert = null)
    {
        ViewData["alert"] = alert;

        var model = await _importService.GetRequest(id);

        return View(model);
    }

    [HttpPost("Verify")]
    public async Task<IActionResult> Verify(string importId, List<int> deliveredQties, List<string> notes)
    {
        try
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = HttpContext.User.FindFirst("Username")?.Value;

            var result = await _importService.Verify(importId, deliveredQties, notes, userId, userName);

            if (result != null) return RedirectToAction("Detail", new { id = importId, alert = result });

            await _importService.UpdateStock(importId);

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}

public class ImportViewModel
{
    public PaginatedList<ProductModel>? _lowOnStockProducts { get; set; }
    public List<SupplierModel>? _suppliers { get; set; } = new List<SupplierModel>();

    public ImportViewModel(
        PaginatedList<ProductModel>? lowOnStockProducts,
        List<SupplierModel>? suppliers)
    {
        _lowOnStockProducts = lowOnStockProducts;
        _suppliers = suppliers ?? new List<SupplierModel>();
    }
}