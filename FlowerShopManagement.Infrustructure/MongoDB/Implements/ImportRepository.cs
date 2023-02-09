using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class ImportRepository : BaseRepository<Import>, IImportRepository
{
    public ImportRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }

    public List<Import> GetRequests(ImportStatus? status = null, int skip = 0, int? limit = null)
    {
        try
        {
            PipelineDefinition<Import, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$match",
                new BsonDocument("status", status is null ?
                new BsonDocument("$exists", true) : status)),
                new BsonDocument("$sort",
                new BsonDocument
                {
                    { "status", 1 },
                    { "createdDate", -1 }
                })
            };

            return Aggregate<Import>(pipeline);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
