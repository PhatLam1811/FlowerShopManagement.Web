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
	//List<Order> _orders; 

	// APPLICATION SERVICES (USE CASES)
	public SaleService()
	{

	}

    
    public async Task<bool> VerifyOnlineOrder(string? orderId, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
	{

		// Set default info for an order
		//order._phoneNumber = user.phoneNumber;
		//order._customerName = user.name;
		if (orderId == null) { return false; }
		var order = await orderRepository.GetById(orderId);
		//update
		if (order != null && order._id != null)
		{
			// Successful case happens
			List<Product> updateProductList = new List<Product>();
			//Check the amount is avaiable before updating
			if (order._products != null)
				foreach (var item in order._products)
				{
					if (item != null && item._id != null)
					{
						var product = await productRepository.GetById(item._id);
						//Amount unavaibale => false
						if (product._amount < item._amount)
							return false;
						product._amount -= item._amount;
						//Add to updateProList
						updateProductList.Add(product);
					}

				}
			//Updating product
			foreach (var item in updateProductList)
			{
				if (item != null && item._id != null)
				{
					var updateResult = await productRepository.UpdateById(item._id, item);
					if (!updateResult)
						return false;
				}
			}
			order._status = Status.Delivering;//Delivering
			return await orderRepository.UpdateById(order._id, order);
		}
		return false;
	}
	public async Task<bool> CreateOfflineOrder(OrderModel order, OfflineCustomerModel user, IOrderRepository orderRepository,
		IUserRepository userRepository, IProductRepository productRepository)
	{
		//Create OrderEntity object
		var newOrder = order.ToEntity();
		var cus = await userRepository.GetByEmailOrPhoneNb(user.PhoneNumber);
		if (cus != null)
		{
			newOrder._accountID = cus._id;
			newOrder._phoneNumber = cus.phoneNumber;
			newOrder._customerName = cus.name;
		}
		else
		{
			//Customer is a passenger
			newOrder._phoneNumber = user.PhoneNumber;
			newOrder._customerName = user.Name;
		}

		if (newOrder != null && newOrder._id != null)
		{
			List<Product> updateProductList = new List<Product>();

			if (newOrder._products != null)
			{
				// reduce the amount of product before updating
				foreach (var item in newOrder._products)
				{
					if (item != null && item._id != null)
					{
						var product = await productRepository.GetById(item._id);
						//Amount unavaibale => false
						if (product._amount < item._amount) return false;
						product._amount -= item._amount;
						//Add to updateProList
						updateProductList.Add(product);
					}

				}
			}
			// Updating...
			foreach (var item in updateProductList)
			{
				if (item != null && item._id != null)
				{
					var updateResult = await productRepository.UpdateById(item._id, item);
					if (!updateResult)
						return false;
				}
			}
			// Successful case happens
			newOrder._status = Status.Purchased;//On charging
			var result = await orderRepository.Add(newOrder);
			return result;

		}
		return false;
	}

	public Task<bool> VerifyOnlineOrder(Order order, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
	{
		throw new NotImplementedException();
	}
public async Task<List<OrderModel>> GetUpdatedOrders(IOrderRepository orderRepository)
    {
        List<Order> orders = await orderRepository.GetAll();
        List<OrderModel> orderMs = new List<OrderModel>();

        foreach (var o in orders)
        {
            orderMs.Add(new OrderModel(o));
        }
        return orderMs;
    }
	public async Task<OrderModel> GetADetailOrder(string id,IOrderRepository orderRepository)
	{
		Order order = await orderRepository.GetById(id);
		OrderModel orderMs = new OrderModel(order);
		return orderMs;
	}
}
