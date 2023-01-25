using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class ImportRepository : BaseRepository<GoodsReceivedNote>, IImportRepository
{
    public ImportRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) 
    {
        CreateUniqueIndex("_GRNNo");
        CreateUniqueIndex("_PoNo");
    }
}
