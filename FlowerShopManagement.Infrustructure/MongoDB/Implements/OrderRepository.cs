using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }

    public List<ProfitableProductModel> GetProfitableProducts(DateTime beginDate, DateTime endDate, int limit = 5)
    {
        PipelineDefinition<Order, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$match",
            new BsonDocument
            {
                { "_status", Status.Purchased },
                { "_date",
                new BsonDocument
                {
                    { "$gte", beginDate },
                    { "$lte", endDate }
                } }
            }),
            new BsonDocument("$unwind",
            new BsonDocument
            {
                { "path", "$_products" },
                { "preserveNullAndEmptyArrays", false }
            }),
            new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", "$_products._name" },
                { "soldNumber",
                new BsonDocument("$sum", "$_products._amount") }
            }),
            new BsonDocument("$sort",
            new BsonDocument("soldNumber", -1)),
            new BsonDocument("$limit", limit)
        };

        try
        {
            return Aggregate<ProfitableProductModel>(pipeline);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public List<ValuableCustomerModel> GetValuableCustomers(DateTime beginDate, DateTime endDate, int limit = 5)
    {
        PipelineDefinition<Order, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$match",
            new BsonDocument
            {
                { "_status", Status.Purchased },
                { "_date",
                new BsonDocument
                {
                    { "$gte", beginDate },
                    { "$lte", endDate }
                } }
            }),
            new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", "$_customerName" },
                { "numberOfOrders",
                new BsonDocument("$count",
                new BsonDocument()) }
            }),
            new BsonDocument("$sort",
            new BsonDocument("numberOfOrders", -1)),
            new BsonDocument("$limit", limit)
        };

        try
        {
            return Aggregate<ValuableCustomerModel>(pipeline);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public int GetOrdersCount(DateTime beginDate, DateTime endDate, Status? status = Status.Purchased)
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
            new BsonDocument("$count", "numberOfOrders")
        };

        try
        {
            var aggregate = Aggregate<OrdersCountModel>(pipeline).FirstOrDefault();

            if (aggregate == null) return 0;

            return aggregate.numberOfOrders;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public List<RevenueModel> GetTotalRevenue(DateTime beginDate, DateTime endDate, string criteria = "$hour", Status status = Status.Purchased)
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
                { "_id",
                new BsonDocument(criteria, "$_date") },
                { "revenue",
                new BsonDocument("$sum", "$_total") }
            })
        };

        try
        {
            return Aggregate<RevenueModel>(pipeline);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public List<TotalOrdersModel> GetTotalOrders(DateTime beginDate, DateTime endDate, string criteria = "$hour", Status status = Status.Purchased)
    {
        PipelineDefinition<Order, BsonDocument> pipeline = new BsonDocument[]
        {
            new BsonDocument("$match",
            new BsonDocument("_date",
            new BsonDocument
            {
                { "$gte", beginDate },
                { "$lte", endDate }
            })),
            new BsonDocument("$group",
            new BsonDocument
            {
                { "_id",
                new BsonDocument(criteria, "$_date") },
                { "numberOfOrders",
                new BsonDocument("$count",
                new BsonDocument()) }
            })
        };

        try
        {
            return Aggregate<TotalOrdersModel>(pipeline);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
