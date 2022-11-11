using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Enums;
using System.Collections.Generic;
using FlowerShopManagement.Infrustructure.Interfaces;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Core.Services
{
    public class SaleServices : ISaleServices
    {
        public ICartCRUD _customerCart;
        public ICustomerCRUD _customerCRUD;
        public IOrderCRUD _orderCRUD;
        List<Customer> _customers;
        List<Profile> _profiles;
        //List<Order> _orders; 
        public IProfileCRUD _profileCRUD;

        // APPLICATION SERVICES (USE CASES)
        public SaleServices(ICartCRUD customerCart, ICustomerCRUD customerCRUD, IOrderCRUD orderCRUD, IProfileCRUD profileCRUD)
        {
            _customerCart = customerCart;
            _orderCRUD = orderCRUD;
            _customerCRUD = customerCRUD;
            _customers = _customerCRUD.GetAllCustomers().Result;
            _profileCRUD = profileCRUD;
            _profiles = _profileCRUD.GetAllProfiles().Result;
        }

        public async Task<List<Order>> GetOrderListAsync()
        {
            return await _orderCRUD.GetAllOrders(); ;
        }

        public List<Customer> GetCustomerList()
        {
            return _customers;
        }

        public async Task<bool> VerifyOrder(string customerId, Order order)
        {
            if (!CheckExistedCustomer(customerId)||!CheckExistedOrder(order._id))
            {
                return false;
            }
            //update
            
            order._isVerified = 1;
            await _orderCRUD.UpdateOrder(order);
            return true;

        }

        public bool CheckExistedCustomer(string? id)
        {
            foreach (Customer customer in _customers)
            {
                if (id == customer._id)
                {
                    return true;
                }
            }
            return false;

        }
        public bool CheckExistedOrder(string? id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var check = _orderCRUD.GetOrderById(id);
                if (check != null)
                {
                    return true;
                }
            }
            

            return false;

        }

        public async Task<bool> CancelOrder(string customerId, Order order)
        {
            order._isVerified = -1;
            await _orderCRUD.UpdateOrder(order);
            return true;
        }

        // this methods help staff to make an order when cus is at shop
        public async Task<bool> CreateAnOrder(string customerEmail, string customerPhoneNumnber, Order order)
        {
            object? orderingCustomer = null;
            foreach (Profile customer in _profiles)
            {
                if (customer._email == customerEmail || customer._phoneNumber == customerPhoneNumnber)
                {
                    orderingCustomer = customer;
                }
            }
            if (orderingCustomer != null)
            {
                order._isVerified = 1;
                await _orderCRUD.AddNewOrder(order);
                return true;
            }
            else return false;
        }
    }
}
