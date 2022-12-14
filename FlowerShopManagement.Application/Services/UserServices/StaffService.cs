using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Services.UserServices;

public class StaffService : UserService, IStaffService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ISupplierRepository _supplierRepository;

    public StaffService(IUserRepository userRepository, ICartRepository cartRepository, ISupplierRepository supplierRepository)
        : base(userRepository, cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<List<UserDetailsModel>?> GetUsersAsync()
    {
        var users = new List<UserDetailsModel>();

        try
        {
            // Get all users from database
            var result = await _userRepository.GetAll();
            
            // Entities to Models
            foreach (var user in result)
            {
                var model = new UserDetailsModel(user);
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

    public async Task<List<SupplierModel>?> GetSuppliersAsync()
    {
        var suppliers = new List<SupplierModel>();

        try
        {
            // Get all users with the role of "Customer" from database
            var result = await _supplierRepository.GetAll();

            // Entities to Models
            foreach (var supplier in result)
            {
                var model = new SupplierModel(supplier);
                suppliers.Add(model);
            }

            // Successfully got customers list
            return suppliers;
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

    public async Task<bool> RemoveUserAsync(UserDetailsModel userModel)
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

    public async Task<UserDetailsModel?> GetUserByPhone(string phoneNb)
    {

        try
        {
            // Get all users from database
            var result = await _userRepository.GetByEmailOrPhoneNb(phoneNb);

            // Entities to Models
            if (result == null) return null; ;
            var users = new UserDetailsModel(result);

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
