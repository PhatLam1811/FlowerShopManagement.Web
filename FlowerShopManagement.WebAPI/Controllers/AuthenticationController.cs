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
    private readonly IAuthService _authServices;

    public AuthenticationController(IAuthService authServices)
    {
        _authServices = authServices;
    }


    // ============ SIGN IN PAGE ============
    //[HttpGet]
    //public IActionResult SignIn()
    //{
    //    return View();
    //}

    // ============ REGISTER PAGE ============
    //[HttpGet]
    //public IActionResult Register()
    //{
    //    return View();
    //}

    // ============ REGISTER EVENT ============
    [HttpPost]
    public async Task<UserModel?> Register([EmailAddress] string email, [Phone] string phoneNumber, string password)
    {
        var currentUser = await _authServices.RegisterAsync(HttpContext, email, phoneNumber, password);

        return null;

        // return CustomerPageView(currentUser);
    }

    // ============ SIGN IN EVENT ============
    [HttpPost]
    public async Task<UserModel?> SignIn(string emailOrPhoneNb, string password)
    {
        var currentUser = await _authServices.SignInAsync(HttpContext, emailOrPhoneNb, password);

        return null;

        //if (_authServices.GetUserRole == Role.Customer.Value)
        //    return CustomerPageView(userModel);
        //else
        //    return StaffPageView(userModel);
    }
}
