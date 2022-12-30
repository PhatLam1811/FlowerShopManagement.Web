using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;

namespace FlowerShopManagement.Application.Interfaces;

public interface IOrderServices
{
    public Task<bool> CreateOrder(OrderModel order, UserModel currentUser, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository);
}


