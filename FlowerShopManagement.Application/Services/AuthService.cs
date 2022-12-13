using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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

    public async Task<UserModel?> RegisterAsync(HttpContext httpContext, string email, string phoneNb, string password)
    {
        var customer = new User();
        var cart = new Cart(customer._id);

        try
        {
            // Encrypt the password using MD5
            string encryptedPass = Validator.MD5Hash(password);

            // Configure new customer's info
            customer.email = email;
            customer.phoneNumber = phoneNb;
            customer.password = encryptedPass;

            // Add to database
            await _cartRepository.Add(cart);
            await _userRepository.Add(customer);

            // HttpContext sign in
            await HttpSignIn(httpContext, customer._id, customer.role.ToString());

            return new UserModel(customer);
        }
        catch
        {
            // Failed to register new customer
            return null;
        }
    }

    public async Task<UserModel?> SignInAsync(HttpContext httpContext, string emailOrPhoneNb, string password)
    {
        try
        {
            // Try to find the matched user in database
            var user = await _userRepository.GetByEmailOrPhoneNb(emailOrPhoneNb);

            // Counldn't find the user with given email or phoneNb
            if (user == null) return null;

            // Encrypt the input password using MD5
            string encryptedPass = Validator.MD5Hash(password);

            // Wrong password
            if (!user.password.Equals(encryptedPass)) return null;

            // HttpContext sign in
            await HttpSignIn(httpContext, user._id, user.role.ToString());

            return new UserModel(user);
        }
        catch
        {
            // Failed to sign in
            return null;
        }
    }

    public async Task<UserModel?> AuthenticateAsync(string id)
    {
        // Try to find the matched user in database
        var user = await _userRepository.GetById(id);

        // Counldn't find the user with given id
        if (user == null) return null;

        return new UserModel(user);
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

    public string? GetUserRole(HttpContext httpContext)
    {
        // Get claim's role value
        return httpContext.User.Claims.ElementAt(1).Value;
    }
}
