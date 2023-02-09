using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IStaffService : IUserService
{
    public Task<List<UserModel>?> GetUsersAsync();
    public Task<List<UserModel1>?> GetUsersAsync1();
    public Task<UserModel?> GetUserByPhone(string phoneNb);
    public Task<bool> AddCustomerAsync(UserModel newCustomerModel);
    public Task<bool> RemoveUserAsync(UserModel userModel);


}
