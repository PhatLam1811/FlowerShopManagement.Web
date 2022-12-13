    using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using FlowerShopManagement.Infrustructure.Mail;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;
using System.Security.Claims;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Application.Services;


namespace FlowerShopManagement.Web.Controllers;

public class HomeController : Controller
{
    //Services
    private readonly ILogger<HomeController> _logger;
    private readonly IAuthService _authServices;
    private readonly IStockServices _stockServices;
    private readonly MailKitService _mailServices;
    private readonly IProductRepository _productRepository;

    public HomeController(ILogger<HomeController> logger, IAuthService authServices, MailKitService mailServices,
        IProductRepository productRepository, IStockServices stockServices)
    {
        _logger = logger;
        _authServices = authServices;
        _mailServices = mailServices;
        _productRepository = productRepository;
        _stockServices = stockServices;
    }

    // =======================================================================================================
    // THESE FUNCTIONS BELOW IN THE SAMPLE REGION ARE ONLY USED FOR TESTING & UNDERSTANDING CLEAN ARCHITECTURE
    // - Change the function names in index.cshtml file for testing multiple functions (button onlick event)
    // - Can code some new functions to test (please keep it down to only 1 or 2 more...)
    // =======================================================================================================
    #region Sample function
    public IActionResult AddItemtoCart()
    {
        return View();
    }

    public Task<bool> AddNewCustomer()
    {
        // Hardcode a Customer object for simpleness
        //CustomerModel customer = new Customer();
        //customer.password = "1"; // this should be encrypted later
        //customer.profile.fullName = "Lam Tan Phat";

        //return _customerServices.AddNewCustomer(customer);
        throw new NotImplementedException();
    }

    public bool RemoveCustomerById()
    {
        // Hardcode for simpleness
        //string removedId = "1e69fd8b-ec24-4754-bee5-1151e8c78876";
        //return _customerServices.RemoveCustomerById(removedId);
        throw new NotImplementedException();
    }
    
   
    #endregion

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.Home = true;
        ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);

        //Get wishlist
        //
        //
        //
        productMs = productMs.OrderBy(i => i.Name).ToList();
        return View(/*Viewmodel*/);
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

    public async Task<bool> SignIn()
    {
        // Create request items list
        List<LowOnStockProductModel> supplyItemModels = new List<LowOnStockProductModel>();

        // Create supply request form
        var requestForm = new SupplyFormModel(new List<LowOnStockProductModel>(), new List<SupplierModel>());

        //_gmailServices.Send();
        //await _mailServices.Send(requestForm);

        return true;

        #region Authenticate login code
        //// Authenticate input email or phone Nb & password
        //var result = await _authServices.AuthenticateAsync("phatlam1811@gmail.com", "123123");

        //// Invalid account
        //if (_authServices.GetUser() == null) return View("Error");

        //// Get user's id and role
        //string userId = _authServices.GetUser()._id;
        //string userRole = _authServices.GetUser().role.Value;

        //// Cookies authenticating section
        //var principal = _authServices.CreateUserClaims(userId, userRole);

        //await HttpContext.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    principal,
        //    new AuthenticationProperties { IsPersistent = true });

        //return RedirectToAction("Index", "Profile");
        #endregion
    }


}