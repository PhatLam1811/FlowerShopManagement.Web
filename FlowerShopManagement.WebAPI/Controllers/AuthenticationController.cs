using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    IAppUserManager _userManager;
    IAuthenticationServices _authServices;

    public AuthenticationController(IAuthenticationServices authServices, IAppUserManager userManager)
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

    [HttpPost]
    public async Task<UserModel?> RegisterNewStaff(string email, string phoneNumber, string password)
    {
        await _authServices.RegisterNewStaff(email, phoneNumber, password);
        return _userManager.GetUser();
    }

    [HttpGet]
    public async Task<UserModel?> SignIn(string emailOrPhoneNb, string password)
    {
        await _authServices.SignIn(emailOrPhoneNb, password);
        return _userManager.GetUser();
    }
}
