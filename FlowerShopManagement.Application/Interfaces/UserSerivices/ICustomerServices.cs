using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface ICustomerfService : IUserService
{
    public Task<List<OrderModel>?> GetOrdersOfUserAsync(string id, IOrderRepository orderRepository);
    public Task<CartModel?> GetCartOfUserAsync(string id);
}
