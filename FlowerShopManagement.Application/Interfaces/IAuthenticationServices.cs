using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IAuthenticationServices
{
    public Task<UserModel?> RegisterNewCustomer(string email, string phoneNb, string password);
    public Task<UserModel?> RegisterNewStaff(string email, string phoneNb, string password);
    public Task<UserModel?> SignIn(string emailOrPhoneNb, string password);
    public void SignOut();
}
