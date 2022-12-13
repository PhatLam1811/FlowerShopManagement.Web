using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
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
        // Encrypt the password using MD5
        string encryptedPass = Validator.MD5Hash(password);

        // Create new user
        User newUser = new User();

        newUser.email = email;
        newUser.phoneNumber = phoneNb;
        newUser.password = encryptedPass;

        // Try creating new user record in database
        if (!await _userRepository.Add(newUser)) return null;

        // Customer's cart creation
        if (newUser.role.Value.Equals(Role.Customer.Value))
        {
            Cart newCart = new Cart(newUser._id);

            // Add created cart to the database
            if (!await _cartRepository.Add(newCart))
            {
                // Remove the created user record since there's problem while creating customer's cart
                await _userRepository.RemoveById(newUser._id);
                return null;
            }
        }

        // Return User Model
        return new UserModel(newUser);
    }

    public async Task<UserModel?> SignInAsync(HttpContext httpContext, string emailOrPhoneNb, string password)
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
        await HttpSignIn(httpContext, user._id, user.role.Value);

        return new UserModel(user);
    }

    public async Task<UserModel?> AuthenticateAsync(string id)
    {
        // Try to find the matched user in database
        var result = await _userRepository.GetById(id);

        // Counldn't find the user with given id
        if (result == null) return null;

        return new UserModel(result);
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
}
