using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FlowerShopManagement.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    IAppUserManager _userManager;
    IAuthenticationServices _authServices;
    string? test;

    public AuthenticationController(
        IAuthenticationServices authServices, 
        IAppUserManager userManager, 
        IHttpContextAccessor httpContextAccessor)
    {
        _authServices = authServices;
        _userManager = userManager;
    }

    //[HttpPost]
    //public async Task<UserModel?> RegisterNewCustomer(string email, string phoneNumber, string password)
    //{
    //    await _authServices.RegisterNewCustomer(email, phoneNumber, password);
    //    return _userManager.GetUser();
    //}

    //[HttpPost]
    //public async Task<UserModel?> RegisterNewStaff([EmailAddress] string email, [Phone] string phoneNumber, string password)
    //{
    //    await _authServices.RegisterNewStaff(email, phoneNumber, password);
    //    return _userManager.GetUser();
    //}

    [HttpPost]
    public async Task<UserModel?> SignIn(string emailOrPhoneNb, string password)
    {
        await _authServices.SignIn(emailOrPhoneNb, password);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, _userManager.GetUser().id),
            new Claim(ClaimTypes.Email, _userManager.GetUser().email),
            new Claim(ClaimTypes.Role, _userManager.GetUserRole())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            principal, 
            new AuthenticationProperties { IsPersistent = true });

        return _userManager.GetUser();
    }
}
