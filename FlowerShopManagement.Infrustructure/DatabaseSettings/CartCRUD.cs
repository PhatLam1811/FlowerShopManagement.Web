using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class CartCRUD : ICartCRUD
{
    private IMongoDbDAO _mongoDbDAO;

    public CartCRUD(IMongoDbDAO mongoDbDAO) => _mongoDbDAO = mongoDbDAO;

    public async Task<bool> AddNewCartByCustomerIdAsync(string customerId)
    {
        List<Product> cartItems = new List<Product>();
        Cart newCart = new Cart(customerId, cartItems, 0);

        try
        {
            await _mongoDbDAO._cartCollection.InsertOneAsync(newCart);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Cart> GetCartOfCustomerIdAsync(string customerId)
    {
        var result = await _mongoDbDAO._cartCollection.FindAsync<Cart>(customerId);
        return result.FirstOrDefault();
    }

    public async Task<bool> UpdateCartByCustomerIdAsync(string customerId, Cart cart)
    {
        try
        {
            await _mongoDbDAO._cartCollection.ReplaceOneAsync<Cart>(c => c._customerId == customerId, cart);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
