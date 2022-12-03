using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface IImportServices
{
    // Make Supply Request
    public void RequestSupply(List<Product> productList, Supplier supplier);

}
