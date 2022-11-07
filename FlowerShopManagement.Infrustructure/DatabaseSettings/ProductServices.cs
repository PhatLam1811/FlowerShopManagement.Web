using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class ProductServices : IProduct
{
    private IMongoDbContext _mongoDbDAO;

    public ProductServices(IMongoDbContext mongoDbDAO) => _mongoDbDAO = mongoDbDAO;

    public async Task<bool> AddNewProduct(Product product)
    {
        try
        {
            await _mongoDbDAO._productCollection.InsertOneAsync(product);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Product> GetProductById(string id)
    {
        var result = await _mongoDbDAO._productCollection.FindAsync<Product>(id);
        return result.FirstOrDefault();
    }

    public bool UpdateProduct(Product updatedProduct)
    {
        var filter = Builders<Product>.Filter.Eq("_id", updatedProduct._id);
        try
        {
            _mongoDbDAO._productCollection.ReplaceOne(filter, updatedProduct, new ReplaceOptions() { IsUpsert = true });
            return true;
        }
        catch 
        { 
            return false; 
        }
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var results = await _mongoDbDAO._productCollection.FindAsync(_ => true);
        return results.ToList();
    }


    public bool RemoveProductById(string id)
    {
        try
        {
            _mongoDbDAO._productCollection.DeleteOne(c => c._id == id);
            return true;
        }
        catch { return false; }
    }
}
