using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;

namespace FlowerShopManagement.Application.Interfaces;

public interface IStockServices
{
    public Task<bool> CreateProduct(NewOrEditProductModel productModel, IProductRepository productRepository);
    public Task<List<ProductModel>> GetUpdatedProducts(IProductRepository productRepository);
}
