using FlowerShopManagement.Application.Interfaces.Models;
using FlowerShopManagement.Application.Interfaces.UseCases;
using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces.Application;

public interface IApplicationUserServices : IAuthenticationServices
{
    public UserModel? GetUser();
}
