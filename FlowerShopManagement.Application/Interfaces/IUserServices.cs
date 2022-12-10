﻿using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface IUserServices
{
    public Task<bool> EditInfoAsync(User entity, UserModel model);

    public Task<bool> ChangePasswordAsync(User entity, string password);

    public Task<bool> ResetPasswordAsync(User entity);

    public Task<bool> RemoveAccountAsync(User entity);

    public Task<List<UserModel>> GetUpdatedCustomers(IUserRepository userRepository);
}

public interface ICustomerServices : IUserServices
{

}
