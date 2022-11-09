using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces;

public interface IAppUserManager
{
    public void SetUser(UserModel? user);

    public UserModel? GetUser();

    public Roles? GetUserRole();

    public void EditProfile();
}
