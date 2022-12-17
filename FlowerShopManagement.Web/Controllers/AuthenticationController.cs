﻿using FlowerShopManagement.Application.Interfaces;
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

    // ========================== VIEWS ========================== //

    #region Views
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }
    #endregion

    // ========================== ACTIONS ========================== //

    #region Actions
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterModel model)
    {
        // Model validation
        if (!ModelState.IsValid) return Register();

        string email = model.Email;
        string phoneNb = model.PhoneNumber;
        string password = model.Password;

        // Email verification

        // Register new user
        var isSuccess = await _authServices.RegisterAsync(HttpContext, email, phoneNb, password);

        // Redirect
        if (isSuccess)
            return RedirectToAction("Index", "Home"); // Successfully registered!
        else
            return Register(); // Failed to register!
    }

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
            return RedirectToAction("Index", "Home"); // Successfully signed in!
        else
            return SignIn(); // Failed to sign in!
    }

    [HttpPost]
    public async Task<IActionResult> SignOutAsync()
    {
        var isSuccess = await _authServices.SignOutAsync(HttpContext);

        if (isSuccess)
            return RedirectToAction("Index", "Home"); // Signed out successfully!
        else
            return View(); // Failed to sign out!
    }
    #endregion
}
