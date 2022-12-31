using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IStaffService : IUserService
{
    public Task<List<UserModel>?> GetUsersAsync();
    public Task<UserModel?> GetUserByPhone(string phoneNb);
    public Task<List<SupplierModel>?> GetAllSuppliersAsync();
    public Task<List<SupplierDetailModel>?> GetAllSupplierDetailsAsync();
    public Task<SupplierModel?> GetSupplierAsync(string id);
    public Task<SupplierDetailModel?> GetSupplierDetailAsync(string id);
    public Task<bool> AddCustomerAsync(UserModel newCustomerModel);
    public Task<bool> RemoveUserAsync(UserModel userModel);
}
