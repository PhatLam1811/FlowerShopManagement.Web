using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopManagement.Web.Controllers;

[Authorize]
[Route("[controller]")]
public class AuthenticationController : Controller
{
    private readonly IAuthService _authServices;

    public AuthenticationController(IAuthService authServices)
    {
        _authServices = authServices;
    }

    // Register
    [AllowAnonymous]
    [Route("Register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("RegisterAsync")]
    public async Task<IActionResult> RegisterAsync(RegisterModel model)
    {
        if (!ModelState.IsValid) return Register();

        string email = model.Email;
        string phoneNb = model.PhoneNumber;
        string password = model.Password;

        // Email verification?

        try
        {
            var newUser = await _authServices.CreateNewUserAsync(email, phoneNb, password);

            // Http authentication
            await HttpSignInAsync(newUser);

            // Session configuration
            HttpContext.Session.SetString("Username", newUser.Name);
            HttpContext.Session.SetString("Avatar", newUser.Avatar);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            if (e.Message.Contains("DuplicateKey"))
                return NotFound(); // Notify existed user

            // Unknown error
            throw new Exception(e.Message);
        }
    }

    // Sign in
    [AllowAnonymous]
    [Route("SignIn")]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("SignInAsync")]
    public async Task<IActionResult> SignInAsync(SignInModel model)
    {
        if (!ModelState.IsValid) return SignIn();

        string emailOrPhoneNb = model.EmailorPhone;
        string password = model.Password;

        try
        {
            var user = await _authServices.ValidateSignInAsync(emailOrPhoneNb, password);

            // Unregistered user (should notify)
            if (user is null) return SignIn();

            // Http authentication
            await HttpSignInAsync(user);

            // Session configuration
            HttpContext.Session.SetString("Username", user.Name);
            HttpContext.Session.SetString("Avatar", user.Avatar);

            // Seperated routing depending on user's role
            if (user.Role == Role.Customer)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Index", "Admin/Product");
        }
        catch (Exception e)
        {
            // Unknown error
            throw new Exception(e.Message);
        }
    }

    [AllowAnonymous]
    private async Task HttpSignInAsync(UserModel user)
    {
        // Using cookie authentication scheme
        var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Create user's claims
        var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user._id),
                new Claim(ClaimTypes.Role, user.Role.ToString())};

        var claimsIdentity = new ClaimsIdentity(claims, authScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authProperties = new AuthenticationProperties 
        {
            IsPersistent = true,
            AllowRefresh = true
        };

        try
        {
            await HttpContext.SignInAsync(authScheme, claimsPrincipal, authProperties);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    // Sign out
    [HttpPost]
    [Route("SignOutAsync")]
    public async Task<IActionResult> SignOutAsync()
    {
        // Using cookie authentication scheme
        var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        
        try
        {
            await HttpContext.SignOutAsync(authScheme);

            // Success
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
