using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
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
    private readonly ICustomerfService _customerService;
    private readonly IPersonalService _personalService;
    private readonly IStockService _stockServices;
    private readonly IEmailService _mailServices;
    private readonly IProductRepository _productRepository;

    static List<string> listCategories = new List<string>();
    static List<Material> listDetailCategories = new List<Material>();
    static List<string> listMaterials = new List<string>();

    public HomeController(ILogger<HomeController> logger, IAuthService authServices, IEmailService mailServices,
        IProductRepository productRepository, IStockService stockServices, ICustomerfService customerfService, IPersonalService personalService)
    {
        _logger = logger;
        _authServices = authServices;
        _mailServices = mailServices;
        _productRepository = productRepository;
        _stockServices = stockServices;
        _customerService = customerfService;
        _personalService = personalService;

        //set up for the static list
        if (listMaterials.Count <= 0 && listDetailCategories.Count <= 0 && listCategories.Count <= 0)
        {
            var task = Task.Run(async () =>
            {
                listCategories = await _stockServices.GetCategories();
                listDetailCategories = await _stockServices.GetDetailMaterials();
                listMaterials = await _stockServices.GetMaterials();

            });
            task.Wait();
            listCategories.Insert(0, "All");
            listMaterials.Insert(0, "All");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Get current user Id
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        ViewBag.Home = true;
        ViewData["Categories"] = listCategories.Where(i => i != "Unknown").ToList();
        ViewData["Materials"] = listMaterials.Where(i => i != "Unknown").ToList();

        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts();

        // get user's id
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Unauthenticated user
        if (userId != null)
        {

            productMs = new List<ProductModel>();

            var temp = await _stockServices.GetUpdatedProducts();

            UserModel? user = await _authServices.GetAuthenticatedUserAsync(userId);

            for (int i = 0; i < temp.Count; i++)
            {
                if (user != null && user.FavProductIds.Where(o => o == temp[i].Id).Count() > 0)
                {
                    temp[i].IsLike = true;
                }
            }

            productMs = productMs.OrderBy(i => i.Name).ToList();

            return View(temp);
        }

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
        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts();
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
    public async Task<IActionResult> AddToWishList(string id)
    {
        // get user's id
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Unauthenticated user
        if (userId is null) return NotFound();

        // add a product item to wishlist of that user
        var isOk = await _customerService.AddFavProduct(userId, id, _authServices, _personalService);

        if (isOk)
        {
            return RedirectToAction("Index", "Home");
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveOutOfWishList(string id)
    {
        // get user's id
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Unauthenticated user
        if (userId is null) return NotFound();

        // remove a product item out of wishlist of that user
        var isOk = await _customerService.RemoveFavProduct(userId, id, _authServices, _personalService);

        if (isOk)
        {
            return RedirectToAction("Index", "Home");
        }

        return NotFound();
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