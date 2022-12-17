using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IAdminService : IStaffService
{
    public Task<bool> AddStaffAsync(UserDetailsModel newStaffModel, Role role);
    public Task<bool> AddSupplierAsync(SupplierDetailModel newSupplierModel);
    public Task<bool> EditSupplierAsync(SupplierDetailModel supplierModel);
    public Task<bool> RemoveSupplierAsync(SupplierModel supplierModel);
    public Task<bool> EditUserRoleAsync(UserDetailsModel userModel, Role role);
    public Task<bool> EditUserAsync(UserDetailsModel userModel);
}
