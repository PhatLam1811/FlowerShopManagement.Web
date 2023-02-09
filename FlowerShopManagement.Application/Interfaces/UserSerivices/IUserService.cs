using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IUserService
{
    public Task<bool> ResetPasswordAsync(UserModel userModel);
    public Task<bool> RemoveAccountAsync(string userId, string userRole);


}
