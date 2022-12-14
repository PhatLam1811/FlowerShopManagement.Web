using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces;

public interface IOrderServices
{
    public Task<bool> CreateOrder(OrderModel order, UserModel currentUser, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository);


