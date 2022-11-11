using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Core.Services
{
    public class CustomerManagementServices : ICustomerManagementServices
    {
        public ICartCRUD _customerCart;
        public ICustomerCRUD _customerCRUD;
        public IProfileCRUD _profileCRUD;

        // APPLICATION SERVICES (USE CASES)
        public CustomerManagementServices(ICartCRUD customerCart, ICustomerCRUD customerCRUD,IProfileCRUD profileCRUD)
        {
            _customerCart = customerCart;
            _customerCRUD = customerCRUD;
            _profileCRUD = profileCRUD;
        }

        public async Task<Cart> GetCustomerCart(string customerId)
        {
            return await _customerCart.GetCartOfCustomerIdAsync(customerId);
        }

        public Task<Profile> GetProfile(string customerId)
        {
            return _profileCRUD.GetProfileById(customerId);
        }
        public async Task<Customer> GetUser(string customerId)
        {
            return  await _customerCRUD.GetCustomerById(customerId);
        }
    }
}
