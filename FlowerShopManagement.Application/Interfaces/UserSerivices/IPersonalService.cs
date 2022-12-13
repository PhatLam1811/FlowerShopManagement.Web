using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IPersonalService : IUserService
{
    public Task<bool> EditInfoAsync(UserDetailsModel userModel);
    public Task<bool> ChangePasswordAsync(UserDetailsModel userModel, string newPassword);
}
