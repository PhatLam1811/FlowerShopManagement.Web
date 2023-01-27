using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IImportService
{
    public List<SupplyRequestModel> GetSupplyRequests();

    public Task<SupplyRequestModel?> GetSupplyRequest(string id);

    public bool SendRequest(SupplyRequestModel form);
    
    public SupplyRequestModel CreateReqSupplyForm(
        List<ProductModel> products, 
        SupplierModel suppliers, 
        List<int> reqAmounts, 
        string staffId, string staffName, string htmlPath);
}
