using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Enums;
using System.Collections.Generic;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using System;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Core.Services;

public class OrderServices : IOrderServices
{
	public async Task<bool> CreateOrder(OrderModel order, UserModel user, IOrderRepository orderRepository, 
		IUserRepository userRepository, IProductRepository productRepository)
	{
		//Create OrderEntity object
		var newOrder = order.ToEntity();
		var cus = await userRepository.GetByEmailOrPhoneNb(user.Email);
		if (cus != null)
		{
			newOrder._accountID = cus._id;
			newOrder._phoneNumber = cus.phoneNumber;
			newOrder._customerName = cus.name;
		}

		if (newOrder != null && newOrder._id != null)
		{

			// Successful case happens
			newOrder._status = Status.Waiting;//On waiting
			var result = await orderRepository.Add(newOrder);
			return result;

		}
		return false;
	}

	public void SetDeliveryMethod(OrderModel order, string type)
	{
		//this's also bullshit
		order.DeliveryMethod = (DeliverryMethods)Enum.Parse(typeof(DeliverryMethods), type);
	}
	public void SetAmount(OrderModel order, int amount)
	{
		//bullshit
	}
	public void SetNote(OrderModel order, string note)
	{
		//bullshit
	}
	public void SetAddress(OrderModel order, string address)
	{
		//bullshit
	}
}
