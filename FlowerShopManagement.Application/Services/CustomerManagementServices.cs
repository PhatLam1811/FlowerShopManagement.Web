using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Models;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Application.Services;

public class CustomerManagementServices : ICustomerManagementServices
{
    public ICartRepository _cartRepository;
    public IUserRepository _userRepository;
    // public IProfileCRUD _profileCRUD;

    // APPLICATION SERVICES (USE CASES)
    public CustomerManagementServices(ICartRepository cartRepository, IUserRepository customerRepository/*, IProfileCRUD profileCRUD */)
    {
        _cartRepository = cartRepository;
        _userRepository = customerRepository;
        // _profileCRUD = profileCRUD;
    }

    public async Task<Cart> GetCustomerCart(string customerId)
    {
        //return await _cartRepository.GetCartOfCustomerIdAsync(customerId);
        throw new NotImplementedException();
    }

    public Task<Profile> GetProfile(string customerId)
    {
        // return _profileCRUD.GetProfileById(customerId);
        throw new NotImplementedException();
    }
    public async Task<CustomerModel> GetUser(string customerId)
    {
        // return  await _userRepository.GetCustomerById(customerId);
        throw new NotImplementedException();
    }
}
