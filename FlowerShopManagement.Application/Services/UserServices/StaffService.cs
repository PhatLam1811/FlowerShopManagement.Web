using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Hosting;

namespace FlowerShopManagement.Application.Services.UserServices;

public class StaffService : UserService, IStaffService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;


    public StaffService(
        IUserRepository userRepository,
        ICartRepository cartRepository,


    IWebHostEnvironment webHostEnvironment)
        : base(userRepository, cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _webHostEnvironment = webHostEnvironment;

    }

    public async Task<List<UserModel>?> GetUsersAsync()
    {
        var users = new List<UserModel>();

        try
        {
            // Get all users from database
            var result = await _userRepository.GetAll();

            // Entities to Models
            foreach (var user in result)
            {
                var model = new UserModel(user);
                users.Add(model);
            }

            // Successfully got staffs list
            return users;
        }
        catch
        {
            // Failed to get staffs list
            return null;
        }
    }

    public async Task<bool> AddCustomerAsync(UserModel newCustomerModel)
    {
        try
        {

            if (newCustomerModel.FormFile == null) return false;
            var customer = await newCustomerModel.ToNewEntity(
                wwwRootPath: _webHostEnvironment.WebRootPath
                );
            // Model to entity
            //var customer = newCustomerModel.ToNewEntity();

            // Set default password - "1"
            var defaultPassword = Validator.MD5Hash("1");
            customer.password = defaultPassword;

            // Generate new customer's cart
            var cart = new Cart(customer._id);

            // Add to database
            var result1 = await _userRepository.Add(customer);
            var result2 = await _cartRepository.Add(cart);

            // Successfully added new customer account
            return result1 && result2;
        }
        catch
        {
            // Failed to add new customer account
            return false;
        }
    }

    public async Task<bool> RemoveUserAsync(UserModel userModel)
    {
        var user = new User();

        try
        {
            // Model to entity
            userModel.ToEntity(ref user);

            // Remove the selected user account
            await RemoveAccountAsync(user._id, user.role.ToString());

            // Successfully removed the selected account
            return true;
        }
        catch
        {
            // Failed to remove the selected account
            return false;
        }
    }

    public async Task<UserModel?> GetUserByPhone(string phoneNb)
    {
        try
        {
            // Get all users from database
            var result = await _userRepository.GetByEmailOrPhoneNb(phoneNb);

            // Entities to Models
            if (result == null) return null; ;
            var users = new UserModel(result);

            // Successfully got staffs list
            return users;
        }
        catch
        {
            // Failed to get staffs list
            return null;
        }
    }


}
