using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Interfaces;

public interface IStockService
{
    public List<LowOnStockProductModel> GetLowOnStockProducts();
}
