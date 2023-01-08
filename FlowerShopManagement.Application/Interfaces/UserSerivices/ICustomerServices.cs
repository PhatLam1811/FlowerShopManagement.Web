using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface ICustomerfService : IUserService
{
    public Task<List<OrderModel>?> GetOrdersOfUserAsync(string id, IOrderRepository orderRepository);
    public Task<CartModel?> GetCartOfUserAsync(string id);
    public Task<List<ProductModel>?> GetFavProductsAsync(string id, IAuthService authService, IProductRepository productRepository);
    public Task<bool> AddFavProduct(string userId, string productId, IAuthService authService, IPersonalService personalService);
    public Task<bool> RemoveFavProduct(string userId, string productId, IAuthService authService, IPersonalService personalService);
}
