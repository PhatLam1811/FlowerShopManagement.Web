using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    private readonly IOrderRepository _orderRepository;

    private const string _reqTemplatePath = "/template/SupplyFormTemplate.html";

    public ImportController(
        IStockService stockService,
        IImportService importService,
        ISupplierService supplierService,
        IWebHostEnvironment webHostEnv,
        IOrderRepository orderRepository)
    {
        _stockService = stockService;
        _importService = importService;
        _supplierService = supplierService;
        _webHostEnv = webHostEnv;
        _orderRepository = orderRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        // Load data
        var lowOnStockProducts = await _stockService.GetLowOnStockProducts();
        var suppliers = await _supplierService.GetAllAsync();

        // Configure viewmodel
        var viewmodel = new ImportViewModel(PaginatedList<ProductModel>.CreateAsync(lowOnStockProducts, 1, 10), suppliers);

        return View(viewmodel);
    }

    // SUPPLY FORM
    [HttpGet("SupplyForm")]
    public IActionResult SupplyForm(SupplyFormModel model)
    {
        try
        {
            return View(model);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPost]
    [ActionName("CreateRequestForm")]
    public async Task CreateRequestForm(List<string> productIds, List<int> reqAmounts, List<string> supplierIds)
    {
        try 
        {
            // await _orderRepository.TotalSale();
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }

        //var products = await _stockService.GetByIdsAsync(productIds);
        //var suppliers = await _supplierService.GetByIdsAsync(supplierIds);
        //var htmlPath = _webHostEnv.WebRootPath + _reqTemplatePath;

        //if (products.Count <= 0  || reqAmounts.Count <= 0 )
        //{
        //    throw new NullReferenceException("Insufficient Values!");
        //}

        //var reqForm = _importService.CreateReqSupplyForm(products, suppliers, reqAmounts, htmlPath);
        //try
        //{
        //    _importService.SendRequest(reqForm);
        //}
        //catch (Exception e)
        //{
        //    throw new Exception(e.Message);
        //}

        //return PartialView("SupplyForm1", reqForm);

        //if (requestForm != null)
        //    return Json(new
        //    {
        //        isValid = true,
        //        html = Helper.RenderRazorViewToString(this, "SupplyForm", requestForm),
        //    });
        //else
        //    return NotFound(); // Failed to create a new supply request form!
    }

    [HttpGet]
    public async Task<IActionResult> ImportDetail(string? id)
    {
        return View();
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