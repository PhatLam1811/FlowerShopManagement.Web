using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }

    public async Task<List<Product>?> GetAllLowOnStock(int minimumAmount)
    {
        var filter = Builders<Product>.Filter.Lte("_amount", minimumAmount);
        var result = await _mongoDbCollection.FindAsync(filter);
        return result.ToList();
    }

    public int GetLowOnStockCount(int minimumAmount)
    {
        PipelineDefinition<Product, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$match",
            new BsonDocument("_amount",
            new BsonDocument("$lte", minimumAmount))),
            new BsonDocument("$count", "amount")
        };

        try
        {
            var result = Aggregate<LowOnStocksCountModel>(pipeline).First();

            return result.amount;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<Product>?> GetProductsById(List<string?> ids)
    {
        var filter = Builders<Product>.Filter.Where(p => p._id != null && ids.Contains(p._id));
        var result = await _mongoDbCollection.FindAsync(filter);
        return result.ToList();
    }
}
