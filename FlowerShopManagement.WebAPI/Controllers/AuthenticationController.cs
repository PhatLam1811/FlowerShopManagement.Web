using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Authentication;
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

    //[HttpGet]
    //public IActionResult Index()
    //{
    //    return View();
    //}

    //[HttpPost]
    //public async Task<UserModel?> RegisterNewCustomer(string email, string phoneNumber, string password)
    //{
    //    await _authServices.RegisterNewCustomer(email, phoneNumber, password);
    //    return _userManager.GetUser();
    //}

}
