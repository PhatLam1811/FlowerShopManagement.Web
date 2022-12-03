using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FlowerShopManagement.Infrustructure.Google.Interfaces;
using FlowerShopManagement.Infrustructure.Mail;
using FlowerShopManagement.Application.Templates;

namespace FlowerShopManagement.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAuthenticationServices _authServices;
    private readonly IGmailServices _gmailServices;
    private readonly MailServices _mailServices;

    public HomeController(ILogger<HomeController> logger, IAuthenticationServices authServices, IGmailServices gmailServices, MailServices mailServices)
    {
        _logger = logger;
        _authServices = authServices;
        _gmailServices = gmailServices;
        _mailServices = mailServices;
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

    public IActionResult Index()
    {
        ViewBag.Home = true;
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
        // Create supply request form
        var requestForm = new SupplyRequestFormModel(
            "z613zgm@gmail.com",
            "Supply Request From Dallas",
            "This is a supply request form!",
            null, new string[0] { }, new string[0] { }, new int[0] { });

        //_gmailServices.Send();
        await _mailServices.Send(requestForm);

        return true;

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
    }
}