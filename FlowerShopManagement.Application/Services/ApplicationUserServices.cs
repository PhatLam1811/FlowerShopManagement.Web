//using FlowerShopManagement.Application.Interfaces.Application;
//using FlowerShopManagement.Application.Interfaces.UseCases;
//using FlowerShopManagement.Application.Models;
//using FlowerShopManagement.Core.Enums;
//using FlowerShopManagement.Core.Interfaces;

//namespace FlowerShopManagement.Application.Services;

//public class ApplicationUserServices : IApplicationUserServices
//{
//    private UserModel? _currentUser;

//    private IAuthenticationServices _authServices;

//    public ApplicationUserServices(IAuthenticationServices authServices)
//    {
//        _authServices = authServices;
//    }

//    public UserModel? GetUser() => _currentUser;

//    public UserModel? Login(string username, string password)
//    {
//        _currentUser = _authServices.Login(username, password);
//        return _currentUser;
//    }

//    public bool Logout(string userId)
//    {
//        _currentUser = null;
//        return _authServices.Logout(userId);
//    }

//    public bool Register(string email, string phoneNumber, string pasword)
//    {
//        throw new NotImplementedException();
//    }
//}
