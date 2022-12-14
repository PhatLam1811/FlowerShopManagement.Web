﻿using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IStaffService : IUserService
{
    public Task<List<UserDetailsModel>?> GetUsersAsync();
    public Task<UserDetailsModel?> GetUserByPhone(string phoneNb);
    public Task<List<SupplierModel>?> GetSuppliersAsync();
    public Task<bool> AddCustomerAsync(UserDetailsModel newCustomerModel);
}
