using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Services;

public class AppUserManager : IAppUserManager
{
    private UserModel? _currentUser;

    private readonly IUserRepository _userRepository;

    public AppUserManager(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void SetUser(UserModel? user) => _currentUser = user;

    public UserModel? GetUser() => _currentUser;

    public Roles? GetUserRole()
    {
        if (_currentUser == null) return null;
        return _currentUser.role;
    }

    public void EditProfile()
    {
        if (_currentUser == null) return;

        User currentUserEntity = _currentUser.ToEntity();

        _userRepository.UpdateById(_currentUser.id, currentUserEntity);
    }
}
