using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IImportService
{
    public bool SendRequest(SupplyFormModel form);
    public SupplyFormModel CreateReqSupplyForm(
        List<ProductModel> products, 
        List<SupplierModel> suppliers, 
        List<int> reqAmounts, 
        string staffId, string staffName, string htmlPath);
}
