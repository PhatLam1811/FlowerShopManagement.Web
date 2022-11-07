using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.Temp;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private User? _currentUser;
    private ISecurityServices _securityServices;
    private IUserManagementServices _userServices;

    public AuthenticationServices(ISecurityServices securityServices, IUserManagementServices userServices)
    {
        _securityServices = securityServices;
        _userServices = userServices;
    }

    public bool Register(string email, string phoneNumber, string pasword)
    {
        // Encrypt password
        var encryptedPassword = _securityServices.Encrypt(pasword);

        // Create new user with input email, phone number & encrypted password
        User newUser = new();
        newUser._email = email;
        newUser._phoneNumber = phoneNumber;
        newUser._password = encryptedPassword;

        // Add newly created user to database
        if (!_userServices.Add(newUser).Result) return false;

        // Successfully registered new user
        _currentUser = newUser;
        return true;
    }

    public bool Login(string username, string password)
    {
        // Validate username
        if (!IsValidEmail(username) && !IsValidPhoneNumber(username)) return false;

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

    #region Considering Remove
    public bool VerifyEmail(string generatedCode, string inputCode) => generatedCode.Equals(inputCode);

    public string EmailVerificationCodeGenerate() => _securityServices.CodeGenerator();

    public bool IsValidEmail(string email) => _securityServices.IsValidEmail(email);

    public bool IsValidPhoneNumber(string phoneNumber) => _securityServices.IsValidPhoneNumber(phoneNumber);
    #endregion
}
