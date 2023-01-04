using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace FlowerShopManagement.Web.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    //Services
    private readonly ILogger<HomeController> _logger;
    private readonly IAuthService _authServices;
    private readonly IStockService _stockServices;
    private readonly IEmailService _mailServices;
    private readonly IProductRepository _productRepository;

    public HomeController(ILogger<HomeController> logger, IAuthService authServices, IEmailService mailServices,
        IProductRepository productRepository, IStockService stockServices)
    {
        _logger = logger;
        _authServices = authServices;
        _mailServices = mailServices;
        _productRepository = productRepository;
        _stockServices = stockServices;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Get current user Id
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        ViewBag.Home = true;
        ViewData["Categories"] = _stockServices.GetCategories();

        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);

        //Get wishlist
        //
        //
        //
        productMs = productMs.OrderBy(i => i.Name).ToList();
        return View(productMs);
    }

    public async Task<IActionResult> Sort(string sortOrder, string currentFilter, string searchString, int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name_asc";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        ViewData["CurrentFilter"] = searchString;
        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);
        if (productMs != null)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                productMs = (List<ProductModel>)productMs.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    productMs = (List<ProductModel>)productMs.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    productMs = (List<ProductModel>)productMs.OrderBy(s => s.Name);
                    break;
                default:
                    //productMs = productMs.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 3;
            return View(PaginatedList<ProductModel>.CreateAsync(productMs, pageNumber ?? 1, pageSize));
        }
        return NotFound();

    }

    [HttpPost]
    public IActionResult AddToWishList(string id)
    {
        return View();
    }

    [HttpPost]
    public IActionResult RemoveFromWishList(string id)
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewBag.About = true;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Welcome()
    {
        return View();
    }
}