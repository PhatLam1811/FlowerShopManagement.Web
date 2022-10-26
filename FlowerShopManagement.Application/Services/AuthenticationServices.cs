using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;

namespace FlowerShopManagement.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private IUser? _currentUser;
    private ISecurityServices _securityServices;
    private IUserServices _userServices;

    public AuthenticationServices(ISecurityServices securityServices, IUserServices userServices)
    {
        _securityServices = securityServices;
        _userServices = userServices;
    }

    public bool Login(string username, string password)
    {
        // Validate username
        if (!_securityServices.IsValidEmail(username) && 
            !_securityServices.IsValidPhoneNumber(username)) 
            return false;

        // Try receiving the user has the same input username
        var result = _userServices.GetUsersByUsername(username).Result;
        if (result == null) return false;

        // Decrypt password
        var decryptedPassword = _securityServices.Decrypt(result._password);
        if (!password.Equals(decryptedPassword)) return false;

        // Login successfully & set up the current user
        _currentUser = result;
        return true;
    }

    public void Logout() => _currentUser = null;

    public bool Register(string username, string pasword, string reEnter)
    {
        throw new NotImplementedException();
    }
}
