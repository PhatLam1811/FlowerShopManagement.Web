using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces.MongoDB;

public interface ISupplyRequestRepository : IBaseRepository<SupplyRequest> 
{
    public List<SupplyRequest> GetRequests(RequestStatus? status = null ,int skip = 0, int? limit = null);
}
