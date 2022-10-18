using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class SupplierCRUD : ISupplierCRUD
{
    private IMongoDbDAO _mongoDbDAO;

    public SupplierCRUD(IMongoDbDAO mongoDbDAO) => _mongoDbDAO = mongoDbDAO;

    

    // Implementation
    public async Task<bool> AddNewSupplier(Supplier newCustomer)
    {
        try
        {
            await _mongoDbDAO._supplierCollection.InsertOneAsync(newCustomer);
            return true;
        }
        catch { return false; }

    }

    public async Task<List<Supplier>> GetAllSuppliers()
    {
        var results = await _mongoDbDAO._supplierCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<Supplier> GetSupplierById(string id)
    {
        var filter = Builders<Supplier>.Filter.Eq("_id", id);
        var result = await _mongoDbDAO._supplierCollection.FindAsync<Supplier>(filter);
        return result.FirstOrDefault();

    }

    public bool RemoveSupplierById(string id)
    {
        try
        {
            _mongoDbDAO._supplierCollection.DeleteOne(c => c._id == id);
            return true;
        }
        catch { return false; }

    }

    public bool UpdateSupplier(Supplier updatedCustomer)
    {
        var filter = Builders<Supplier>.Filter.Eq("_id", updatedCustomer._id);
        try
        {
            _mongoDbDAO._supplierCollection.ReplaceOne(filter, updatedCustomer, new ReplaceOptions() { IsUpsert = true });
            return true;
        }
        catch { return false; }
    }

    
}

