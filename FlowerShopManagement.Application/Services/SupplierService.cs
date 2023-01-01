using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using Org.BouncyCastle.Asn1.X509;

namespace FlowerShopManagement.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<List<SupplierModel>?> GetAllAsync(int skip = 0, int? limit = null)
    {
        var suppliers = new List<SupplierModel>();

        try
        {
            List<Supplier>? result = await _supplierRepository.GetAll(skip, limit);
            
            // There's no supplier
            if (result is null) return null;

            foreach (Supplier supplier in result)
            {
                var model = new SupplierModel(supplier);
                suppliers.Add(model);
            }

            return suppliers;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> AddOneAsync(SupplierModel model)
    {
        Supplier newSupplier = model.ToEntity();

        try
        {
            return await _supplierRepository.Add(newSupplier);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<SupplierModel?> GetOneAsync(string id)
    {
        try
        {
            Supplier? supplier = await _supplierRepository.GetById(id);

            // Supplier not found
            if (supplier is null) return null;

            return new SupplierModel(supplier);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> UpdateOneAsync(SupplierModel model)
    {
        Supplier entity = model.ToEntity();

        entity.lastModified = DateTime.Now;

        try
        {
            return await _supplierRepository.UpdateById(entity._id, entity);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> RemoveOneAsync(string id)
    {
        try
        {
            return await _supplierRepository.RemoveById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
