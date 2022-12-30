using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

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

    public async Task<UserModel> CreateNewUserAsync(string email, string phoneNb, string password, Role role = Role.Customer)
    {
        // Encrypt password
        string encryptedPass = Validator.MD5Hash(password);

        User newUser = new User()
        {
            email = email,
            phoneNumber = phoneNb,
            password = encryptedPass,
            role = role
        };

        try
        {
            await _userRepository.Add(newUser);

            // Add cart if the registrator is a customer
            if (newUser.role == Role.Customer)
            {
                Cart newCart = new Cart(newUser._id);
                await _cartRepository.Add(newCart);
            }

            // Success
            return new UserModel(newUser);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<UserModel?> ValidateSignInAsync(string emailOrPhoneNb, string password)
    {
        // Encrypt password
        string encryptedPass = Validator.MD5Hash(password);

        try
        {
            var user = await _userRepository.GetByEmailOrPhoneNb(emailOrPhoneNb);

            // Wrong email or phone number
            if (user is null) return null;

            // Wrong password
            if (user.password != encryptedPass) return null;

            // Success
            return new UserModel(user);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<UserModel?> GetAuthenticatedUserAsync(string id)
    {
        try
        {
            var user = await _userRepository.GetById(id);

            // User not found
            if (user is null) return null;

            // Success
            return new UserModel(user);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
