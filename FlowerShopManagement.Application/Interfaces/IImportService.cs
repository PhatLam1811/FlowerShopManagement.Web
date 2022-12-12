using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IImportService
{
    // Make Supply Request
    public void Request(SupplyFormModel supplyForm);
    public SupplyFormModel CreateSupplyForm(List<LowOnStockProductModel> productList, List<SupplierModel> supplier);
}
