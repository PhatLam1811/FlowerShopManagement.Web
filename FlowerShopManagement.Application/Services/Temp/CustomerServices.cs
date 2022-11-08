//using FlowerShopManagement.Core.Interfaces;
//using FlowerShopManagement.Core.Entities;
//using FlowerShopManagement.Application.Interfaces.Temp;

//// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
//// - New adjustments could be made in future updates
//// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

//namespace FlowerShopManagement.Application.Services.Temp
//{
//    public class CustomerServices : ICustomerServices
//    {
//        public ICartDAOServices _customerCart;
//        public ICustomer _customerCRUD;

//        // APPLICATION SERVICES (USE CASES)
//        public CustomerServices(ICartDAOServices customerCart, ICustomer customerCRUD)
//        {
//            _customerCart = customerCart;
//            _customerCRUD = customerCRUD;
//        }

//        public bool AddItemToCart(Product newItem, Cart cart, string customerId)
//        {
//            cart.items.Add(newItem);

//            try
//            {
//                _customerCart.UpdateById(customerId, cart);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public async Task<Cart> GetCustomerCart(string customerId)
//        {
//            return await _customerCart.GetByCustomerId(customerId);
//        }

//        // CRUD SERVICES
//        #region CRUD
//        public Task<bool> AddNewCustomer(Customer newCustomer) => _customerCRUD.AddNewCustomer(newCustomer);

//        public Task<List<Customer>> GetAllCustomers() => _customerCRUD.GetAllCustomers();

//        public Task<Customer> GetCustomerById(string id) => _customerCRUD.GetCustomerById(id);

//        public bool RemoveCustomerById(string id) => _customerCRUD.RemoveCustomerById(id);

//        public bool UpdateCustomer(Customer updatedCustomer) => _customerCRUD.UpdateCustomer(updatedCustomer);
//        #endregion
//    }
//}
