using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IStaffService : IUserService
{
    public Task<List<UserDetailsModel>?> GetStaffsAsync();
    public Task<List<UserDetailsModel>?> GetCustomersAsync();
    public Task<List<SupplierModel>?> GetSuppliersAsync();
    public Task<bool> AddCustomerAsync(UserDetailsModel newCustomerModel);
}
