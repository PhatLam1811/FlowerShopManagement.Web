using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;


public class SupplyRequestRepository : BaseRepository<SupplyRequest>, ISupplyRequestRepository
{
    public SupplyRequestRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }

    public List<SupplyRequest> GetRequests(RequestStatus? status = null, int skip = 0, int? limit = null)
    {
        try
        {
            PipelineDefinition<SupplyRequest, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$match",
                new BsonDocument("status", status is null ?
                new BsonDocument("$exists", true) : status)),
                new BsonDocument("$sort",
                new BsonDocument
                {
                    { "status", -1 },
                    { "createdDate", -1 }
                })
            };

            return Aggregate<SupplyRequest>(pipeline);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
