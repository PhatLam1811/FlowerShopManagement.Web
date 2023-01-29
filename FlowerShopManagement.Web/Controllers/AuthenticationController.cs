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

    // REGISTER
    [AllowAnonymous]
    [HttpGet("Register")]
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

            return Redirect("~/Home");
        }
        catch (Exception e)
        {
            if (e.Message.Contains("DuplicateKey"))
                return NotFound(); // Notify existed user

            // Unknown error
            throw new Exception(e.Message);
        }
    }

    // SIGN IN
    [AllowAnonymous]
    [HttpGet("SignIn")]
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

            // Seperated routing depending on user's role
            if (user.Role == Role.Customer)
                return Redirect("~/Home");
            else
                return Redirect("~/Admin/DashBoard");
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
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user._id),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("Username", user.Name),
            new Claim("Email", user.Email),
            new Claim("Avatar", user.Avatar)
        };

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

    // SIGN OUT
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
            return Redirect("~/Home");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
