using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces;

public interface IAuthService
{
    public Task<UserModel> CreateNewUserAsync(string email, string phoneNb, string password, Role role = Role.Customer);
    public Task<UserModel?> ValidateSignInAsync(string emailOrPhoneNb, string password);
    public Task<UserModel?> GetAuthenticatedUserAsync(string id);
}
