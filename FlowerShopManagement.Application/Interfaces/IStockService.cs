using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface IStockService
{
	public Task<bool> CreateProduct(ProductDetailModel productModel, IProductRepository productRepository);
	public Task<bool> CreateVoucher(VoucherDetailModel voucherDetailModel, IVoucherRepository voucherRepository);
	public Task<List<ProductModel>> GetUpdatedProducts(IProductRepository productRepository);
	public Task<ProductDetailModel> GetADetailProduct(string id, IProductRepository productRepository);
	public Task<List<VoucherDetailModel>> GetUpdatedVouchers(IVoucherRepository voucherRepository);
	public Task<List<string>> GetCategories();
	public Task<List<Material>> GetDetailMaterials();
	public Task<List<string>> GetMaterials();
    public Task<List<ProductModel>> GetLowOnStockProducts(IProductRepository productRepository);
}
