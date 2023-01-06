using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Authorize(Policy = "StaffOnly")]
public class ImportController : Controller
{
    private readonly IStockService _stockService;
    private readonly IEmailService _mailService;
    private readonly IImportService _importService;
    private readonly ISupplierService _supplierService;
    private readonly IWebHostEnvironment _webHostEnv;

    public ImportController(
        IStockService stockService,
        IEmailService mailService,
        IImportService importService,
        ISupplierService supplierService,
        IWebHostEnvironment webHostEnv)
    {
        _stockService = stockService;
        _mailService = mailService;
        _importService = importService;
        _supplierService = supplierService;
        _webHostEnv = webHostEnv;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
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
    [ActionName("CreateSupplyRequestFormAsync")]
    public async Task<IActionResult> CreateSupplyRequestFormAsync(List<string> productIds, List<int> reqAmounts, List<string> supplierIds)
    {
        SupplyFormModel reqForm = new SupplyFormModel();
        var products = await _stockService.GetByIdsAsync(productIds);
        var suppilers = await _supplierService.GetByIdsAsync(supplierIds);
        var htmlPath = _webHostEnv.WebRootPath + reqForm.Template;

        if (products == null || suppilers == null || reqAmounts == null) return NotFound();

        reqForm.Configurate(suppilers, products, reqAmounts, htmlPath);

        return PartialView("SupplyForm1", reqForm);

        //var message = _mailService.CreateMimeMessage(reqForm);

        //try
        //{
        //    await _mailService.SendAsync(message);
        //}
        //catch (Exception e)
        //{
        //    throw new Exception(e.Message);
        //}

        //if (requestForm != null)
        //    return Json(new
        //    {
        //        isValid = true,
        //        html = Helper.RenderRazorViewToString(this, "SupplyForm", requestForm),
        //    });
        //else
        //    return NotFound(); // Failed to create a new supply request form!
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