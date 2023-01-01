using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IImportService
{
    public void Request(SupplyFormModel supplyForm);
    public SupplyFormModel? CreateSupplyForm(List<ProductDetailModel> productList, List<int> amounts, List<SupplierModel> supplier);
}
