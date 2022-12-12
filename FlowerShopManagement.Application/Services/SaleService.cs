using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Enums;
using System.Collections.Generic;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Models;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Application.Services;

public class SaleService : ISaleService
{
    public ICartRepository _cartRepository;
    public IUserRepository _userReopsitory;
    public IOrderRepository _orderRepository;
    List<User> _customers;
    //List<Order> _orders; 

    // APPLICATION SERVICES (USE CASES)
    public SaleService(ICartRepository cartRepository, IUserRepository userRepository, IOrderRepository orderRepository)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _userReopsitory = userRepository;
        _customers = _userReopsitory.GetAll().Result.ToList();
    }

    public async Task<List<Order>> GetOrderListAsync()
    {
        // return await _orderRepository.GetAll().Result.ToList();
        throw new NotImplementedException();
    }


    public List<UserModel> GetCustomerList()
    {
        // return _customers;
        throw new NotImplementedException();
    }

    public async Task<bool> VerifyOrder(string customerId, Order order)
    {
        //if (!CheckExistedCustomer(customerId)||!CheckExistedOrder(order._id))
        //{
        //    return false;
        //}
        ////update
        
        //order._isVerified = 1;
        //await _orderRepository.UpdateOrder(order);
        return true;
    }

    public bool CheckExistedCustomer(string? id)
    {
        //foreach (Customer customer in _customers)
        //{
        //    if (id == customer._id)
        //    {
        //        return true;
        //    }
        //}
        return false;

    }
    public bool CheckExistedOrder(string? id)
    {
        //if (!string.IsNullOrEmpty(id))
        //{
        //    var check = _orderRepository.GetOrderById(id);
        //    if (check != null)
        //    {
        //        return true;
        //    }
        //}
        

        return false;

    }

    public async Task<bool> CancelOrder(string customerId, Order order)
    {
        //order._isVerified = -1;
        //await _orderRepository.UpdateOrder(order);
        return true;
    }

    // this methods help staff to make an order when cus is at shop
    public async Task<bool> CreateAnOrder(string customerEmail, string customerPhoneNumnber, Order order)
    {
        //object? orderingCustomer = null;
        //foreach (Profile customer in _profiles)
        //{
        //    if (customer._email == customerEmail || customer._phoneNumber == customerPhoneNumnber)
        //    {
        //        orderingCustomer = customer;
        //    }
        //}
        //if (orderingCustomer != null)
        //{
        //    order._isVerified = 1;
        //    await _orderRepository.Add(order);
        //    return true;
        //}
        //else return false;
        return false;
    }
}
