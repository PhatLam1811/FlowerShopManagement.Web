using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Services.UserServices;

public class CustomerServices : UserService, ICustomerfService
{
    IUserRepository _userRepository;
    ICartRepository _cartRepository;
    public CustomerServices(IUserRepository userRepository, ICartRepository cartRepository)
        : base(userRepository, cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task<CartModel?> GetCartOfUserAsync(string id)
    {
        var cart = await _cartRepository.GetById(id); 
        if(cart != null)
        {
            CartModel cartModel = new CartModel(cart);
            return cartModel;
        }
        return null;
    }

    public async Task<List<OrderModel>?> GetOrdersOfUserAsync(string id, IOrderRepository orderRepository)
    {
        List<OrderModel> orderModels = new List<OrderModel>();
        var orders = await orderRepository.GetAll();
        if (orders != null)
        {
            orders = orders.Where(o => o._accountID == id).ToList();

            foreach (var order in orders)
            {
                orderModels.Add(new OrderModel(order));
            }
        }
        return orderModels;
    }
}
