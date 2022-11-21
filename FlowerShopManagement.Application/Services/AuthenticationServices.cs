using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FlowerShopManagement.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private User? _currentUser;

    public AuthenticationServices(
        IUserRepository userRepository, 
        ICartRepository cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task<UserModel?> RegisterAsync(string email, string phoneNb, string password, Role? role = null)
    {
        // Encrypt the password using MD5
        string encryptedPass = Validator.MD5Hash(password);

        // Create new user
        User newUser = new User();

        newUser.email = email;
        newUser.phoneNumber = phoneNb;
        newUser.password = encryptedPass;
        newUser.role = role != null ? role : Role.Customer;

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

        // Assign Current User
        _currentUser = newUser;

        // Return User Model
        return new UserModel(newUser);
    }

    public async Task<UserModel?> AuthenticateAsync(string emailOrPhoneNb, string password)
    {
        // Try to find the matched user in database
        var result = await _userRepository.GetByEmailOrPhoneNb(emailOrPhoneNb);

        // Counldn't find the user with given email or phoneNb
        if (result == null) return null;

        // Encrypt the input password using MD5
        string encryptedPass = Validator.MD5Hash(password);

        // Wrong password
        if (!result.password.Equals(encryptedPass)) return null;

        // Assign Current User
        _currentUser = result;

        return new UserModel(result);
    }

    public async Task<UserModel?> AuthenticateAsync(string id)
    {
        // Try to find the matched user in database
        var result = await _userRepository.GetById(id);

        // Counldn't find the user with given id
        if (result == null) return null;

        // Assign Current User
        _currentUser = result;

        return new UserModel(result);
    }

    public ClaimsPrincipal CreateUserClaims(string id, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        return principal;
    }

    public User? GetUser() => _currentUser;
}
