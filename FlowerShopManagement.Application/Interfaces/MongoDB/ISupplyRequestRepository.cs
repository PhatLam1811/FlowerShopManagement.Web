using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces.MongoDB;

public interface IImportRepository : IBaseRepository<Import> 
{
    public List<Import> GetRequests(ImportStatus? status = null ,int skip = 0, int? limit = null);
}
