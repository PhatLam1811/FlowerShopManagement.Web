using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }

    public void TotalCount(DateTime beginDate, DateTime endDate, string dateFormat = "$hour", Status? status = Status.Purchased)
    {
        PipelineDefinition<Order, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$match",
                new BsonDocument
                {
                    { "_status", status is null ? 
                        new BsonDocument("$exists", true) : status },
                    { "_date",
                    new BsonDocument
                    {
                        { "$gte", beginDate },
                        { "$lte", endDate }
                    }
                }}),
            new BsonDocument("$group",
                new BsonDocument
                {
                    { "_id", new BsonDocument(dateFormat, "$_date") },
                    { "totalCount", new BsonDocument("$count", new BsonDocument()) }
                })
        };

        try
        {
            var result = _mongoDbCollection.Aggregate(pipeline).ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void TotalSum(DateTime beginDate, DateTime endDate, string dateFormat = "$hour", Status status = Status.Purchased)
    {
        PipelineDefinition<Order, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$match",
            new BsonDocument
            {
                { "_status", status },
                { "_date",
                new BsonDocument
                {
                    { "$gte", beginDate },
                    { "$lte", endDate }
                }
            }}),
            new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", new BsonDocument(dateFormat, "$_date") },
                { "totalSum", new BsonDocument("$sum", "$_total") }
            })
        };

        try
        {
            var result = _mongoDbCollection.Aggregate(pipeline).ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
