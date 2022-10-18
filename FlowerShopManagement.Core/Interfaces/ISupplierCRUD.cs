using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Core.Interfaces;

public interface ISupplierCRUD
{
    public Task<bool> AddNewSupplier(Supplier newSupplier);
    public Task<List<Supplier>> GetAllSuppliers();
    public Task<Supplier> GetSupplierById(string id);
    public bool UpdateSupplier(Supplier updatedSupplier);
    public bool RemoveSupplierById(string id);
}
