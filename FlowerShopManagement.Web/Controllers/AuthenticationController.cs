using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Web.Controllers;

public class AuthenticationController : Controller
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

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // ============ REGISTER ACTION ============
    [HttpPost]
    public async Task<UserModel?> Register([EmailAddress] string email, [Phone] string phoneNumber, string password, string confirmPassword)
    {
        var currentUser = await _authServices.RegisterAsync(HttpContext, email, phoneNumber, password);

        return currentUser;

        // return CustomerPageView(currentUser);
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    // ============ SIGN IN ACTION ============
    [HttpPost("SignIn")]
    public async Task<UserModel?> SignIn(string emailOrPhoneNb, string password)
    {
        var currentUser = await _authServices.SignInAsync(HttpContext, emailOrPhoneNb, password);

        return currentUser;

        //if (_authServices.GetUserRole == Role.Customer.Value)
        //    return CustomerPageView(userModel);
        //else
        //    return StaffPageView(userModel);
    }
}
