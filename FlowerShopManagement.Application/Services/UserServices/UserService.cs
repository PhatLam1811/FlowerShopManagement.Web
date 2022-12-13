using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Services.UserServices;

public class UserService : IPersonalService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public UserService(IUserRepository userRepository, ICartRepository cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task<bool> EditInfoAsync(UserDetailsModel userModel)
    {
        var user = new User();

        try
        {
            // Model to entity
            userModel.ToEntity(ref user);

            // Set last modified date
            user.lastModified = DateTime.Now;

            // Update database
            return await _userRepository.UpdateById(user._id, user);
        }
        catch
        {
            // Failed to edit user's info
            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(UserDetailsModel userModel, string newPassword)
    {
        var user = new User();

        try
        {
            // Model to entity
            userModel.ToEntity(ref user);

            // Encrypt password using MD5
            var encryptedPass = Validator.MD5Hash(newPassword);

            // Set new password
            user.password = encryptedPass;

            // Set last modified date
            user.lastModified = DateTime.Now;

            // Update database
            return await _userRepository.UpdateById(user._id, user);
        }
        catch
        {
            // Failed to change user's password
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(UserDetailsModel userModel)
    {
        return await ChangePasswordAsync(userModel, "1");
    }

    public async Task<bool> RemoveAccountAsync(UserDetailsModel userModel)
    {
        var user = new User();

        try
        {
            // Model to entity
            userModel.ToEntity(ref user);

            // Remove cart if user is customer
            if (user.role == Role.Customer)
                await _cartRepository.RemoveByField("customerId", user._id);

            // Remove user from database
            await _userRepository.RemoveById(user._id);

            // Successfully removed user
            return true;
        }
        catch
        {
            // Failed to remove user
            return false;
        }
    }
}