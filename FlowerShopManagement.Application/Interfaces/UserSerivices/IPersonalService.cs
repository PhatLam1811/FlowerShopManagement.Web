using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IPersonalService : IUserService
{
    public Task<bool> EditInfoAsync(UserModel userModel);
    public Task<bool> ChangePasswordAsync(UserModel user, string newPassword);
}
