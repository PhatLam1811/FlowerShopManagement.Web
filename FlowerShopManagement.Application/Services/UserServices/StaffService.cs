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

    public async Task<List<SupplierModel>?> GetAllSuppliersAsync()
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

    public async Task<List<SupplierDetailModel>?> GetAllSupplierDetailsAsync()
    {
        var suppliers = new List<SupplierDetailModel>();

        try
        {
            // Get all users with the role of "Customer" from database
            var result = await _supplierRepository.GetAll();

            // Entities to Models
            foreach (var supplier in result)
            {
                var model = new SupplierDetailModel(supplier);
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

    public async Task<SupplierModel?> GetSupplierAsync(string id)
    {
        try
        {
            var supplier = await _supplierRepository.GetById(id);

            // Entity to model
            var supplierModel = new SupplierModel(supplier);

            // Successfully got the supplier
            return supplierModel;
        }
        catch
        {
            // Failed to get the supplier
            return null;
        }
    }

    public async Task<SupplierDetailModel?> GetSupplierDetailAsync(string id)
    {
        try
        {
            var supplier = await _supplierRepository.GetById(id);

            // Entity to model
            var supplierModel = new SupplierDetailModel(supplier);

            // Successfully got the supplier
            return supplierModel;
        }
        catch
        {
            // Failed to get the supplier
            return null;
        }
    }

    public async Task<bool> AddCustomerAsync(UserModel newCustomerModel)
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
