using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    IAppUserManager _appUserManager;
    IUserRepository _userRepository;
    ICartRepository _cartRepository;

    public AuthenticationServices(
        IUserRepository userRepository, 
        ICartRepository cartRepository,
        IAppUserManager appUserManager)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _appUserManager = appUserManager;
    }

    public async Task<UserModel?> RegisterNewCustomer(string email, string phoneNb, string password)
    {
        // Add new customer
        User newCustomer = new();
        newCustomer.email = email;
        newCustomer.phoneNumber = phoneNb;
        newCustomer.password = password;
        if (!await _userRepository.Add(newCustomer)) return null;

        // Add new customer's cart
        Cart newCart = new(newCustomer._id);
        await _cartRepository.Add(newCart);

        // Set up current customer's model
        CustomerModel currentUser = new(newCustomer, newCart);

        // Set app's current user
        _appUserManager.SetUser(currentUser);

        return currentUser;
    }

    public async Task<UserModel?> RegisterNewStaff(string email, string phoneNb, string password)
    {
        // Add new staff
        User newStaff = new();
        newStaff.email = email;
        newStaff.phoneNumber = phoneNb;
        newStaff.password = password;
        newStaff.role = Roles.staff;
        if (!await _userRepository.Add(newStaff)) return null;

        // Set up current staff's model
        StaffModel currentUser = new(newStaff);

        // Set app's current user
        _appUserManager.SetUser(currentUser);

        return currentUser;
    }

    public async Task<UserModel?> SignIn(string emailOrPhoneNb, string password)
    {
        // Try to find the authorized user in database
        var result = await _userRepository.GetByEmailOrPhoneNb(emailOrPhoneNb, password);

        // No user found
        if (result == null) return null;

        // Set up model for found user
        UserModel currentUser;

        if (result.role == Roles.customer)
        {
            var customerCart = await _cartRepository.GetByField("customerId", result._id);
            currentUser = new CustomerModel(result, customerCart);
        }
        else
            currentUser = new StaffModel(result);

        // Set up app's current user
        _appUserManager.SetUser(currentUser);

        return currentUser;
    }

    public void SignOut() => _appUserManager.SetUser(null);
}
