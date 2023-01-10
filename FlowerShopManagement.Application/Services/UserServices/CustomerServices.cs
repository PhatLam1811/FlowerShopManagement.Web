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
    IProductRepository _productRepository;
    private readonly IAddressRepository _addressRepository;

    public CustomerService(IUserRepository userRepository, ICartRepository cartRepository, IAddressRepository addressRepository, IProductRepository productRepository)
        : base(userRepository, cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
        _productRepository = productRepository;
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
        var cart = await _cartRepository.GetByField("customerId", id);
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

    public async Task<bool> AddItemToCart(string userId, string productId, int amount)
    {
        var cart = await _cartRepository.GetByField("customerId", userId);
        if (cart == null)
        {
            return false;
        }

        var products = cart.items;

        if (products != null && products.Where(o => o._id == productId).Count() > 0)
        {
            // cart's not empty
            // check if duplicate
            // duplicate: yes, update amount
            // check if amount exceed to stock
            products.Where(o => o._id == productId).First().amount += amount;
            cart.items = products;
            bool result = await _cartRepository.UpdateById(cart._id, cart);
            return result;
        }
        else
        {
            // cart's empty
            // or duplicate: no
            // add new
            // check if product existed
            Product? newItem = await _productRepository.GetById(productId);
            if (newItem != null && newItem._id != null)
            {
                // if cart's empty
                if (products == null)
                {
                    products = new List<CartItem>();
                }

                newItem._amount = amount;
                products.Add(new CartItem(userId) { amount = amount, items = newItem, _productId = newItem._id });
                cart.items = products;
                bool result = await _cartRepository.UpdateById(cart._id, cart);
                return result;
            }
            else
            {
                return false;
            }
        }
    }

    public async Task<bool> UpdateAmountOfItem(string userId, string productId, int amount)
    {
        var cart = await _cartRepository.GetByField("customerId", userId);
        if (cart == null)
        {
            return false;
        }

        var products = cart.items;

        if (products != null && products.Where(o => o._productId == productId).Count() > 0)
        {
            // cart's not empty
            // update amount
            // check if amount exceed to stock
            products.Where(o => o._productId == productId).First().amount = amount;
            cart.items = products;
            bool result = await _cartRepository.UpdateById(cart._id, cart);
            return result;
        }
        return false;
    }

    public async Task<bool> RemoveItemToCart(string userId, string productId)
    {
        var cart = await _cartRepository.GetByField("customerId", userId);
        if (cart == null)
        {
            return false;
        }

        var products = cart.items;

        if (products != null && products.Where(o => o._productId == productId).Count() > 0)
        {
            // cart's not empty
            // check if product is existed
            products.RemoveAll(o => o._productId == productId);
            cart.items = products;
            bool result = await _cartRepository.UpdateById(cart._id, cart);
            return result;
        }
        else
        {
            return true;
        }
    }

    public async Task<bool> UpdateSelection(string userId, string cartItemId, bool isSelected)
    {
        var cart = await _cartRepository.GetByField("customerId", userId);
        if (cart == null)
        {
            return false;
        }

        var products = cart.items;

        if (products != null && products.Where(o => o._id == cartItemId).Count() > 0)
        {
            // cart's not empty
            // update amount
            // check if amount exceed to stock
            products.Where(o => o._id == cartItemId).First().isSelected = isSelected;
            cart.items = products;
            bool result = await _cartRepository.UpdateById(cart._id, cart);
            return result;
        }
        return false;
    }
}
