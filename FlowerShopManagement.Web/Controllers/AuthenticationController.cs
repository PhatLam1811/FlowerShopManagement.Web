using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class AuthenticationController : Controller
{
    private readonly IAuthService _authServices;

    public AuthenticationController(IAuthService authServices)
    {
        _authServices = authServices;
    }

    // ========================== VIEWS ========================== //

    #region Views
    [Route("Register")]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [Route("SignIn")]
    [HttpGet]

    public IActionResult SignIn()
    {
        return View();
    }


    #endregion

    // ========================== ACTIONS ========================== //

    #region Actions
    [Route("RegisterAsync")]

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterModel model)
    {
        // Model validation
        if (!ModelState.IsValid) return Register();

        string email = model.Email;
        string phoneNb = model.PhoneNumber;
        string password = model.Password;

        // Email verification?

        // Register new user
        var isSuccess = await _authServices.RegisterAsync(HttpContext, email, phoneNb, password);

        // Redirect
        if (isSuccess)
            return RedirectToAction("Index", "Home"); // Successfully registered!
        else
            return Register(); // Failed to register!
    }
    [Route("SignInAsync")]

    [HttpPost]
    public async Task<IActionResult> SignInAsync(SignInModel model)
    {
        // Model validation
        if (!ModelState.IsValid) return SignIn();

        string emailOrPhoneNb = model.EmailorPhone;
        string password = model.Password;

        // Sign in to system
        var isSuccess = await _authServices.SignInAsync(HttpContext, emailOrPhoneNb, password);

        // Redirect
        if (isSuccess)
            return RedirectToAction("Index", "Product", new { action = "Index", area ="Admin"}); // Successfully signed in!
        else
            return SignIn(); // Failed to sign in!
    }

    [HttpPost]
	[Route("SignOutAsync")]
	public async Task<IActionResult> SignOutAsync()
    {
        var isSuccess = await _authServices.SignOutAsync(HttpContext);

        if (isSuccess)
            return RedirectToAction("SignIn", "Authentication"); // Signed out successfully!
        else
            return NotFound(); // Failed to sign out!
    }

    #endregion
}
