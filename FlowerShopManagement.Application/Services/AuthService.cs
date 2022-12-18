﻿using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FlowerShopManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public AuthService(
        IUserRepository userRepository,
        ICartRepository cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task<bool> RegisterAsync(HttpContext httpContext, string email, string phoneNb, string password)
    {
        // Add new user to dabase
        var user = await RegisterAsync(email, phoneNb, password);

        if (user == null) return false;

        try 
        {
            // HttpContext sign in
            await HttpSignIn(httpContext, user._id, user.role.ToString());

            // Set session
            httpContext.Session.SetString("NameIdentifier", user._id);
            httpContext.Session.SetString("Role", user.role.ToString());
            httpContext.Session.SetString("Username", user.name);

            return true;
        }
        catch
        {
            // Failed to register new customer
            return false;
        }
    }

    public async Task<User?> RegisterAsync(string? email, string? phoneNb, string? password)
    {
        var emailRgx = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
        var phoneNbRgx = new Regex(@"^([\\+]?84[-]?|[0])?[1-9][0-9]{8}$");
        var passRgx = new Regex(@"^[a-zA-Z0-9]+$");

        if (password == null || email == null || phoneNb == null || password == null) return null;
        if (password.Length < 6 || !emailRgx.IsMatch(email) || !phoneNbRgx.IsMatch(phoneNb) || !passRgx.IsMatch(password)) return null;

        try
        {
            // Encrypt password
            string encryptedPass = Validator.MD5Hash(password);

            // Create new user
            var newUser = new User();

            newUser.email = email;
            newUser.phoneNumber = phoneNb;
            newUser.password = encryptedPass;

            // Add to database
            var result = await _userRepository.Add(newUser);

            return newUser;
        }
        catch
        {
            return null;
        }

    }

    public async Task<bool> SignInAsync(HttpContext httpContext, string emailOrPhoneNb, string password)
    {
        try
        {
            var user = await _userRepository.GetByEmailOrPhoneNb(emailOrPhoneNb);

            // Wrong email or phone number
            if (user == null) return false;

            // Encrypt the input password using MD5
            string encryptedPass = Validator.MD5Hash(password);

            // Wrong password
            if (!user.password.Equals(encryptedPass)) return false;

            // HttpContext sign in
            await HttpSignIn(httpContext, user._id, user.role.ToString());

            // Set session
            httpContext.Session.SetString("NameIdentifier", user._id);
            httpContext.Session.SetString("Role", user.role.ToString());
            httpContext.Session.SetString("Username", user.name);

            return true;
        }
        catch
        {
            // Failed to sign in
            return false;
        }
    }

    public async Task<bool> SignOutAsync(HttpContext httpContext)
    {
        try
        {
            await httpContext.SignOutAsync("Cookies");
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task HttpSignIn(HttpContext httpContext, string id, string role)
    {
        // Using cookies authentication scheme
        const string scheme = "Cookies";

        // Create user claims 
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Role, role)
        };

        // Create claim principal
        var identity = new ClaimsIdentity(claims, scheme);
        var principal = new ClaimsPrincipal(identity);

        // Sign in
        await httpContext.SignInAsync(
            scheme, principal,
            new AuthenticationProperties { IsPersistent = true });
    }

    public async Task<UserDetailsModel> GetUserAsync(HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        var user = await _userRepository.GetById(userId);
        return new UserDetailsModel(user);
    }

    public string? GetUserRole(HttpContext httpContext)
    {
        if (httpContext.User.Claims.ElementAt(1) != null)
            return httpContext.User.Claims.ElementAt(1).Value; // Get from cookies
        else
            return httpContext.Session.GetString("Role"); // Get from session
    }

    public string? GetUserId(HttpContext httpContext)
    {
        if (httpContext.User.Claims.ElementAt(0) != null)
            return httpContext.User.Claims.ElementAt(0).Value; // Get from cookies
        else
            return httpContext.Session.GetString("NameIdentifier"); // Get from session
    }
}
