namespace FlowerShopManagement.Core.Common;

public interface IBaseServices<Placeholder> where Placeholder : new()
{
    public Task<List<Placeholder>> GetAll();

    public Task<Placeholder> GetById(string id);
    
    public bool Add(Placeholder newRecord);
    
    public Task<bool> RemoveById(string id);
    
    public Task<bool> UpdateById(string id, Placeholder updatedRecord);
}
