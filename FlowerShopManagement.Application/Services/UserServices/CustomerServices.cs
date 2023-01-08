using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Services.UserServices;

public class CustomerService : UserService, ICustomerfService
{
    IUserRepository _userRepository;
    ICartRepository _cartRepository;
    private readonly IAddressRepository _addressRepository;

    public CustomerService(IUserRepository userRepository, ICartRepository cartRepository, IAddressRepository addressRepository)
        : base(userRepository, cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
    }

    public async Task<List<ProductModel>?> GetFavProductsAsync(string id, IAuthService authService, IProductRepository productRepository)
    {
        List<ProductModel> productModels = new List<ProductModel>();

        var user = await authService.GetAuthenticatedUserAsync(id);

        if (user != null)
        {
            var favProductIds = user.FavProductIds;
            for (int i = 0; i < favProductIds.Count; i++)
            {
                var product = await productRepository.GetById(favProductIds[i]);
                if (product != null)
                {
                    productModels.Add(new ProductModel(product) { IsLike = true });
                }
            }
        }

        return null;
    }

    public async Task<bool> AddFavProduct(string userId, string productId, IAuthService authService, IPersonalService personalService)
    {
        var user = await authService.GetAuthenticatedUserAsync(userId);

        if (user != null)
        {
            var favProductIds = user.FavProductIds;

            if (favProductIds.Contains(productId))
            {
                return false;
            }
            else
            {
                user.FavProductIds.Add(productId);
                await personalService.EditInfoAsync(user);

                return true;
            }
        }

        return false;
    }

    public async Task<bool> RemoveFavProduct(string userId, string productId, IAuthService authService, IPersonalService personalService)
    {
        var user = await authService.GetAuthenticatedUserAsync(userId);

        if (user != null)
        {
            var favProductIds = user.FavProductIds;

            if (favProductIds.Contains(productId))
            {
                user.FavProductIds.Remove(productId);
                await personalService.EditInfoAsync(user);

                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public async Task<CartModel?> GetCartOfUserAsync(string id)
    {
        var cart = await _cartRepository.GetById(id);
        if (cart != null)
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
