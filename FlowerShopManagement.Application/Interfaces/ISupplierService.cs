using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface ISupplierService
{
    public Task<List<SupplierModel>> GetAllAsync(int skip = 0, int? limit = null);
    public Task<List<SupplierModel>> GetByIdsAsync(List<string> ids);
    public Task<SupplierModel?> GetOneAsync(string id);
    public Task<bool> AddOneAsync(SupplierModel model);
    public Task<bool> UpdateOneAsync(SupplierModel model);
    public Task<bool> RemoveOneAsync(string id); 
}
