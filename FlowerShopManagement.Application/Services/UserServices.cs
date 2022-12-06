using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public UserServices(IUserRepository userRepository, ICartRepository cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task<bool> EditInfoAsync(User entity, UserModel model)
    {
        // Update entity
        model.ToEntity(entity);
        entity.lastModified = DateTime.Now;

        // Update database
        var result = await _userRepository.UpdateById(entity._id, entity);

        return result;
    }

    public async Task<bool> ChangePasswordAsync(User entity, string password)
    {
        // Encrypt password using MD5
        string encryptedPass = Validator.MD5Hash(password);

        // Update entity
        entity.password = encryptedPass;
        entity.lastModified = DateTime.Now;

        // Update database
        var result = await _userRepository.UpdateById(entity._id, entity);

        return result;
    }

    public async Task<bool> ResetPasswordAsync(User entity)
    {
        return await ChangePasswordAsync(entity, "1");
    }

    public async Task<bool> RemoveAccountAsync(User entity)
    {
        // Remove current customer's cart
        if (entity.role.Equals(Role.Customer))
        {
            if (!await _cartRepository.RemoveByField("customerId", entity._id)) return false;
        }

        // Remove the user from the database
        return await _userRepository.RemoveById(entity._id);
    }
}

public class CustomerServices : UserServices, ICustomerServices
{
    public CustomerServices(IUserRepository userRepository, ICartRepository cartRepository) : base(userRepository, cartRepository) { }
}