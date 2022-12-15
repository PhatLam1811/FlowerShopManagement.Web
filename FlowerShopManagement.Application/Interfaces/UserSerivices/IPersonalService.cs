using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IPersonalService : IUserService
{
    public Task<bool> EditInfoAsync(UserDetailsModel userModel);
    public Task<bool> ChangePasswordAsync(UserDetailsModel user, string newPassword);
}
