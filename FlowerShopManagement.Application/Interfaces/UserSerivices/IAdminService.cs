using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IAdminService : IStaffService
{
    public Task<bool> AddStaffAsync(UserModel newStaffModel, Role role);
    public Task<bool> AddSupplierAsync(SupplierDetailModel newSupplierModel);
    public Task<bool> EditSupplierAsync(SupplierDetailModel supplierModel);
    public Task<bool> RemoveSupplierAsync(SupplierModel supplierModel);
    public Task<bool> EditUserRoleAsync(UserModel userModel, Role role);
    public Task<bool> EditUserAsync(UserModel userModel);
}
