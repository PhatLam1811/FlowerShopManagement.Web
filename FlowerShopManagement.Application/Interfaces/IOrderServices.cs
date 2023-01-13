using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IOrderServices
{
    public Task<bool> CreateOrder(OrderModel order, UserModel currentUser, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository);
}
