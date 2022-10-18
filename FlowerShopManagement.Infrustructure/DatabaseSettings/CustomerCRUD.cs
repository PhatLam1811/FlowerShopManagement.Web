using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class CustomerCRUD : ICustomerCRUD
{
    private IMongoDbDAO _mongoDbDAO;

    public CustomerCRUD(IMongoDbDAO mongoDbDAO) => _mongoDbDAO = mongoDbDAO;

    

    // Implementation
    public async Task<bool> AddNewCustomer(Customer newCustomer)
    {
        try
        {
            await _mongoDbDAO._customerCollection.InsertOneAsync(newCustomer);
            return true;
        }
        catch { return false; }

    }

    public async Task<List<Customer>> GetAllCustomers()
    {
        var results = await _mongoDbDAO._customerCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<Customer> GetCustomerById(string id)
    {
        var filter = Builders<Customer>.Filter.Eq("_id", id);
        var result = await _mongoDbDAO._customerCollection.FindAsync<Customer>(filter);
        return result.FirstOrDefault();

    }

    public bool RemoveCustomerById(string id)
    {
        try
        {
            _mongoDbDAO._customerCollection.DeleteOne(c => c._id == id);
            return true;
        }
        catch { return false; }

    }

    public bool UpdateCustomer(Customer updatedCustomer)
    {
        var filter = Builders<Customer>.Filter.Eq("_id", updatedCustomer._id);
        try
        {
            _mongoDbDAO._customerCollection.ReplaceOne(filter, updatedCustomer, new ReplaceOptions() { IsUpsert = true });
            return true;
        }
        catch { return false; }
    }

    
}

