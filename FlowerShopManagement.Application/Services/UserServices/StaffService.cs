using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Services.UserServices;

public class StaffService : UserService, IStaffService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public StaffService(IUserRepository userRepository, ICartRepository cartRepository)
        : base(userRepository, cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task<List<UserDetailsModel>?> GetStaffsAsync()
    {
        var allStaffs = new List<UserDetailsModel>();

        try
        {
            // Get all users with the role of "Staff" or "Admin" from database
            var staffs = await _userRepository.GetByRole(Role.Staff);
            var admins = await _userRepository.GetByRole(Role.Admin);
            var result = staffs.Concat(admins);

            // Entities to Models
            foreach (var staff in result)
            {
                var model = new UserDetailsModel(staff);
                allStaffs.Add(model);
            }

            // Successfully got staffs list
            return allStaffs;
        }
        catch
        {
            // Failed to get staffs list
            return null;
        }
    }

    public async Task<List<UserDetailsModel>?> GetCustomersAsync()
    {
        var customers = new List<UserDetailsModel>();

        try
        {
            // Get all users with the role of "Customer" from database
            var result = await _userRepository.GetByRole(Role.Customer);

            // Entities to Models
            foreach (var customer in result)
            {
                var model = new UserDetailsModel(customer);
                customers.Add(model);
            }

            // Successfully got customers list
            return customers;
        }
        catch
        {
            // Failed to get customers list
            return null;
        }
    }

    public async Task<bool> AddCustomerAsync(UserDetailsModel newCustomerModel)
    {
        try
        {
            // Model to entity
            var customer = newCustomerModel.ToNewEntity();

            // Set default password - "1"
            var defaultPassword = Validator.MD5Hash("1");
            customer.password = defaultPassword;

            // Generate new customer's cart
            var cart = new Cart(customer._id);

            // Add to database
            await _userRepository.Add(customer);
            await _cartRepository.Add(cart);

            // Successfully added new customer account
            return true;
        }
        catch
        {
            // Failed to add new customer account
            return false;
        }
    }
}
