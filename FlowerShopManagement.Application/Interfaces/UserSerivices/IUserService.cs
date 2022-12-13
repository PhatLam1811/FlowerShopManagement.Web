using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IUserService
{
    public Task<bool> ResetPasswordAsync(UserDetailsModel userModel);
    public Task<bool> RemoveAccountAsync(UserDetailsModel userModel);
}
