using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IStaffService : IUserService
{
    public Task<List<UserDetailsModel>?> GetUsersAsync();
    public Task<UserDetailsModel?> GetUserByPhone(string phoneNb);
    public Task<List<SupplierModel>?> GetAllSuppliersAsync();
    public Task<SupplierModel?> GetSupplierAsync(string id);
    public Task<bool> AddCustomerAsync(UserDetailsModel newCustomerModel);
    public Task<bool> RemoveUserAsync(UserDetailsModel userModel);
}
