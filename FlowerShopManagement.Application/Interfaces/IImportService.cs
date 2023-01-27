using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IImportService
{
    public List<ImportModel> GetRequests();

    public Task<ImportModel?> GetRequest(string id);

    public Task<string?> Verify(string id, List<int> deliveredQties, List<string> notes);

    public bool SendRequest(ImportModel form);
    
    public ImportModel CreateRequestForm(
        List<ProductModel> products, 
        SupplierModel suppliers, 
        List<int> reqAmounts, 
        string staffId, string staffName, string htmlPath);
}
