using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class CartServices : ICart
{
    private IMongoDbContext _mongoDbDAO;

    public CartServices(IMongoDbContext mongoDbDAO) => _mongoDbDAO = mongoDbDAO;

    public Task<bool> Add(Cart newRecord)
    {
        throw new NotImplementedException();
    }

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

    public Task<List<Cart>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Cart> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Cart> GetCartOfCustomerIdAsync(string customerId)
    {
        var result = await _mongoDbDAO._cartCollection.FindAsync<Cart>(customerId);
        return result.FirstOrDefault();
    }

    public Task<bool> RemoveById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateById(string id, Cart updatedRecord)
    {
        throw new NotImplementedException();
    }
}
