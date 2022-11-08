using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.UseCases;

public interface IAuthenticationServices
{
    public bool Register(string email, string phoneNumber, string pasword);
    public UserModel? Login(string username, string password);
    public bool Logout(string userId);
}
