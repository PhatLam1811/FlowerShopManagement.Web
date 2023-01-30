using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }

    public List<Product>? GetAllLowOnStock(int minimumAmount)
    {
        PipelineDefinition<Product, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$sort",
            new BsonDocument("_amount", 1))
        };

        return Aggregate<Product>(pipeline);
    }

    public List<CategoryStatisticModel>? GetCategoryStatistic()
    {
        PipelineDefinition<Product, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", "$_category" },
                { "numberOfProducts",
                new BsonDocument("$count",
                new BsonDocument()) }
            })
        };

        try
        {
            return Aggregate<CategoryStatisticModel>(pipeline);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public int GetLowOnStockCount(int minimumAmount)
    {
        PipelineDefinition<Product, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$match",
            new BsonDocument("_amount", minimumAmount >= 0 ?
            new BsonDocument("$lte", minimumAmount) :
            new BsonDocument("$exists", true))),
            new BsonDocument("$count", "amount")
        };

        try
        {
            var result = Aggregate<LowOnStocksCountModel>(pipeline).FirstOrDefault();

            if (result == null) return 0;

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
