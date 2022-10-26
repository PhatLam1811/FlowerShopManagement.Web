using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Interfaces;

namespace FlowerShopManagement.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private IUser? _currentUser;
    private ISecurityServices _securityServices;

    public AuthenticationServices(ISecurityServices securityServices)
    {
        _securityServices = securityServices;
    }

    public bool Login(string username, string password)
    {
        // Validate username
        if (!_securityServices.IsValidEmail(username) && !_securityServices.IsValidPhoneNumber(username)) return false;

        // Try receiving the user has the same input username


        // Decrypt password

        return true;
    }

    public bool Logout()
    {
        throw new NotImplementedException();
    }

    public bool Register(string username, string pasword, string reEnter)
    {
        throw new NotImplementedException();
    }
}
