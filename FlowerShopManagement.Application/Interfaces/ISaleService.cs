using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface ISaleService
{
	public Task<bool> VerifyOnlineOrder(Order order, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository);
	public Task<bool> VerifyOnlineOrder(string orderId, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository);
	public Task<bool> CreateOfflineOrder(OrderModel order, UserModel user, IOrderRepository orderRepository,
		IUserRepository userRepository, IProductRepository productRepository);

	public Task<List<OrderModel>> GetUpdatedOrders(IOrderRepository orderRepository);
	public Task<OrderModel> GetADetailOrder(string id, IOrderRepository orderRepository);


}
