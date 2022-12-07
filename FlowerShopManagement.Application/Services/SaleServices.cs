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

public class SaleServices : ISaleServices
{
	//List<Order> _orders; 

	// APPLICATION SERVICES (USE CASES)
	public SaleServices()
	{

	}


	public async Task<bool> VerifyOnlineOrder(string orderId, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
	{

		// Set default info for an order
		//order._phoneNumber = user.phoneNumber;
		//order._customerName = user.name;
		var order = await orderRepository.GetById(orderId);
		//update
		if (order != null && order._id != null)
		{
			//Check if the order has been on DB 
			var updateOrder = await orderRepository.GetById(order._id);
			if (updateOrder != null)
			{
				// Successful case happens
				List<Product> updateProductList = new List<Product>();
				//Update product 
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
				foreach (var item in updateProductList)
				{
					if (item != null && item._id != null)
					{
						var updateResult = await productRepository.UpdateById(item._id, item);
						if (!updateResult)
							return false;
					}
				}
				order._isVerified = 1;
				order._status = Status.sampleStatus;//Delivering
				return await orderRepository.UpdateById(order._id, order);
			}

		}
		return false;
	}
	public async Task<bool> CreateOfflineOrder(OrderModel order, UserModel user, IOrderRepository orderRepository,
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
			newOrder._customerName = user.FullName;
		}

		if (newOrder != null && newOrder._id != null)
		{
			List<Product> updateProductList = new List<Product>();
			//Update product 
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
			newOrder._isVerified = 1;
			newOrder._status = Status.sampleStatus;//On charging
			var result = await orderRepository.Add(newOrder);
			return result;

		}
		return false;
	}

	public Task<bool> VerifyOnlineOrder(Order order, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
	{
		throw new NotImplementedException();
	}
}
