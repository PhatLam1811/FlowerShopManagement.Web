using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;

namespace FlowerShopManagement.Core.Services;

public class StockService : IStockService
{
    private readonly IProductRepository _productRepository;

    public StockService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<LowOnStockProductModel> GetLowOnStockProducts()
    {
        // This is only a temporary value of the minimum amount
        // needed for supply request
        int minimumAmount = 20;
        List<LowOnStockProductModel> lowOnStockProducts = new List<LowOnStockProductModel>();

        var result = _productRepository.GetAllLowOnStock(minimumAmount).Result;

        // Convert Product to SupplyItemModel
        foreach (var item in result)
        {
            // Convert product to model
            var model = new LowOnStockProductModel(item);

            // Add model to list
            lowOnStockProducts.Add(model);
        }

        return lowOnStockProducts;
    }
}
