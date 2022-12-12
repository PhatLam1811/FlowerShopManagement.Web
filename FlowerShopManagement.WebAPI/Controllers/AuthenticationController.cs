using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authServices;

    public AuthenticationController(IAuthenticationService authServices)
    {
        _authServices = authServices;
    }

    //[HttpPost]
    //public async Task<UserModel?> RegisterNewCustomer(string email, string phoneNumber, string password)
    //{
    //    await _authServices.RegisterNewCustomer(email, phoneNumber, password);
    //    return _userManager.GetUser();
    //}

    [HttpPost]
    public async Task<UserModel?> RegisterNewUser([EmailAddress] string email, [Phone] string phoneNumber, string password)
    {
        return await _authServices.RegisterAsync(email, phoneNumber, password);
    }

    //[HttpPost]
    //public async Task<UserModel?> SignIn(string emailOrPhoneNb, string password)
    //{
    //    await _authServices.SignIn(emailOrPhoneNb, password);

    //    if (_userManager.GetUser() == null) return null; 

    //    var claims = new List<Claim>
    //    {
    //        new Claim(ClaimTypes.NameIdentifier, _userManager.GetUser().id),
    //        new Claim(ClaimTypes.Email, _userManager.GetUser().email),
    //        new Claim(ClaimTypes.Role, _userManager.GetUserRole())
    //    };

    //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //    var principal = new ClaimsPrincipal(identity);

    //    await HttpContext.SignInAsync(
    //        CookieAuthenticationDefaults.AuthenticationScheme, 
    //        principal, 
    //        new AuthenticationProperties { IsPersistent = true });

    //    return _userManager.GetUser();
    //}
}
