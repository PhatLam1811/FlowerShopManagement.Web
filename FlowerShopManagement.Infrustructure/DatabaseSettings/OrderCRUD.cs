using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class OrderCRUD : IOrderCRUD
{
    private IMongoDbDAO _mongoDbDAO;

    public OrderCRUD(IMongoDbDAO mongoDbDAO) => _mongoDbDAO = mongoDbDAO;

    public async Task<bool> AddNewOrder(Order newOrder)
    {
        try
        {
            await _mongoDbDAO._orderCollection.InsertOneAsync(newOrder);
            return true;
        }
        catch { return false; }
    }

    public async Task<List<Order>> GetAllOrders()
    {
        var results = await _mongoDbDAO._orderCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<Order> GetOrderById(string id)
    {
        var filter = Builders<Order>.Filter.Eq("_id", id);
        var result = await _mongoDbDAO._orderCollection.FindAsync<Order>(filter);
        return result.FirstOrDefault();
    }

    public async Task<bool> RemoveOrderById(string id)
    {
        try
        {
            await _mongoDbDAO._orderCollection.DeleteOneAsync(c => c._id == id);
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateOrder(Order updatedOrder)
    {
        var filter = Builders<Order>.Filter.Eq("_id", updatedOrder._id);
        try
        {
            await _mongoDbDAO._orderCollection.ReplaceOneAsync(filter, updatedOrder, new ReplaceOptions() { IsUpsert = true });
            return true;
        }
        catch { return false; }
    }

     
}
