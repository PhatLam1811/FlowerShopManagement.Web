//using FlowerShopManagement.Application.Interfaces.Temp;
//using FlowerShopManagement.Application.Interfaces.UseCases;
//using FlowerShopManagement.Application.Models;
//using FlowerShopManagement.Core.Common;
//using FlowerShopManagement.Core.Entities;
//using FlowerShopManagement.Core.Interfaces;
//using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
//using System.Security.Cryptography;

//namespace FlowerShopManagement.Application.Services;

//public class AuthenticationServices : IAuthenticationServices
//{
//    //private IUserDAOServices _userDAOServices;
//    //private ICartDAOServices _cartDAOServices;


//    //public AuthenticationServices(ISecurityServices securityServices, IUserDAOServices userDAOServices, ICartDAOServices cartDAOServices)
//    //{
//    //    _securityServices = securityServices;
//    //    _userDAOServices = userDAOServices;
//    //    _cartDAOServices = cartDAOServices;
//    //}

//    private IUserRepository _userRepo;
//    private ICartRepository _cartRepo;
//    private ISecurityServices _securityServices;

//    public AuthenticationServices(IUserRepository userRepo, ICartRepository cartRepo, ISecurityServices securityServices)
//    {
//        _userRepo = userRepo;
//        _cartRepo = cartRepo;
//        _securityServices = securityServices;
//    }

//    public bool Register(string email, string phoneNumber, string pasword)
//    {
//        // Encrypt password
//        string encryptedPassword;

//        using (MD5 md5Hash = MD5.Create())
//        {
//            encryptedPassword = _securityServices.Encrypt(md5Hash, pasword);
//        }

//        // Create new user with input email, phone number & encrypted password
//        User newUser = new();
//        newUser.email = email;
//        newUser.phoneNumber = phoneNumber;
//        newUser.password = encryptedPassword;

//        Cart newCart = new(newUser._id);

//        // Add newly created user to database
//        //if (!_userDAOServices.Add(newUser).Result) return false;
//        //_cartDAOServices.Add(newCart);

//        if (!_userRepo.Add(newUser).Result) return false;
//        _cartRepo.Add(newCart);

//        // Successfully registered new user
//        return true;
//    }

//    public UserModel? Login(string username, string password)
//    {
//        // Try receiving the user has the same input username
//        // var result = _userDAOServices.GetByEmail(username).Result;

//        var result = _userRepo.GetByField("email", username).Result;
//        if (result == null) return null;

//        // Decrypt password
//        // var decryptedPassword = _securityServices.Encrypt(result.password);
//        // if (!password.Equals(decryptedPassword)) return null;

//        // Login successfully & set up the current user
//        //-- Get user's cart --//
//        //Cart userCart = _cartDAOServices.GetByCustomerId(result._id).Result;
//        Cart userCart = _cartRepo.GetByField("customerId", result._id).Result;

//        //-- Create current user model object --//
//        UserModel currentUser = new();

//        return currentUser;
//    }

//    public bool Logout(string userId)
//    {
//        throw new NotImplementedException();
//    }

//    #region Considering Remove
//    public bool VerifyEmail(string generatedCode, string inputCode) => generatedCode.Equals(inputCode);

//    public string EmailVerificationCodeGenerate() => _securityServices.CodeGenerator();

//    public bool IsValidEmail(string email) => _securityServices.IsValidEmail(email);

//    public bool IsValidPhoneNumber(string phoneNumber) => _securityServices.IsValidPhoneNumber(phoneNumber);
//    #endregion
//}