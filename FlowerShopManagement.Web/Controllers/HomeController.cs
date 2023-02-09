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
        if (HttpContext.User.FindFirst(ClaimTypes.Role)?.Value == "Staff") 
            return RedirectToAction("index","dashboard", new { area = "admin" });

        // Get current user Id
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        ViewBag.Home = true;
        ViewData["Categories"] = listCategories.Where(i => i != "Unknown").ToList();
        ViewData["Materials"] = listMaterials.Where(i => i != "Unknown").ToList();

        List<ProductDetailModel> productMs = await _stockServices.GetUpdatedDetailProducts();

        // get user's id
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Unauthenticated user
        if (userId != null)
        {

            productMs = new List<ProductDetailModel>();

            var temp = await _stockServices.GetUpdatedDetailProducts();

            UserModel? user = await _authServices.GetAuthenticatedUserAsync(userId);

            for (int i = 0; i < temp.Count; i++)
            {
                if (user != null && user.FavProductIds != null&& user.FavProductIds.Where(o => o == temp[i].Id).Count() > 0)
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

    [Route("Sort")]
    [HttpPost]
    public async Task<IActionResult> Sort(string sortOrder, string currentFilter, string searchString,
       int? pageNumber, string? currentPrice, string currentMaterial, string? currentCategory)
    {
        int pageSize = 8;

        ViewData["CurrentSort"] = sortOrder;
        ViewData["CurrentPrice"] = currentPrice;
        ViewData["CurrentCategory"] = currentCategory;
        ViewData["CurrentMaterial"] = currentMaterial;
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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
        List<ProductDetailModel> productMs = await _stockServices.GetUpdatedDetailProducts();
        if (productMs != null)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                productMs = productMs.Where(s => s.Name.Contains(searchString)).ToList();
            }
            // sort order - feature incoming
            switch (sortOrder)
            {
                case "name_desc":
                    productMs = productMs.OrderByDescending(s => s.Name).ToList();
                    break;
                //case "Date":
                //    productMs = (List<ProductModel>)productMs.OrderBy(s => s.);
                //      break;
                case "name_asc":
                    productMs = productMs.OrderBy(s => s.Name).ToList();
                    break;
                default:
                    //case filter

                    break;
            }
            switch (currentPrice)
            {
                case "0$ -> 10$":
                    productMs = productMs.Where(s => s.UniPrice > 0 && s.UniPrice <= 10).ToList();
                    break;
                case "11$ -> 50$":
                    productMs = productMs.Where(s => s.UniPrice > 10 && s.UniPrice <= 50).ToList();
                    break;
                case "> 50$":
                    productMs = productMs.Where(s => s.UniPrice > 50).ToList();
                    break;
                default:

                    break;
            }

            if (currentMaterial != null && currentMaterial != "All")
            {
                productMs = productMs.Where(s => s.Material.Equals(currentMaterial)).ToList();
            }
            if (currentCategory != null && currentCategory != "All")
            {
                productMs = productMs.Where(s => s.Category.Equals(currentCategory)).ToList();
            }
            PaginatedList<ProductDetailModel> objs = PaginatedList<ProductDetailModel>
                .CreateAsync(productMs, pageNumber ?? 1, pageSize);
            return Json(new
            {
                isValid = true,
                htmlViewAll = Helper.RenderRazorViewToString(this, "_ViewAll", objs),
                htmlPagination = Helper.RenderRazorViewToString(this, "_Pagination", objs)

            });
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
            //return RedirectToAction("Index", "Home");
            return Json( new {isTrue = true});
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
			//return RedirectToAction("Index", "Home");
			return Json(new { isTrue = false });

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