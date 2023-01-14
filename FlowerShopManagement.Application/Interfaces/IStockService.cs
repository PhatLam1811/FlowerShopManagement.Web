using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface IStockService
{
	public Task<bool> CreateProduct(ProductDetailModel productModel);
	public Task<bool> CreateVoucher(VoucherDetailModel voucherDetailModel, IVoucherRepository voucherRepository);
	public Task<List<ProductModel>> GetUpdatedProducts();
	public Task<List<ProductDetailModel>> GetUpdatedDetailProducts();
    public Task<List<ProductModel>> GetByIdsAsync(List<string> ids);
	public Task<ProductDetailModel> GetADetailProduct(string id);
	public Task<List<VoucherDetailModel>> GetUpdatedVouchers();
	public Task<List<string>> GetCategories();
	public Task<bool> CreateCategory(string name);
	public Task<bool> CreateMaterial(string name, string description);
	public Task<List<Material>> GetDetailMaterials();
	public Task<List<string>> GetMaterials();
    public Task<List<ProductModel>> GetLowOnStockProducts();
    public Task<bool> UpdateProduct(ProductDetailModel productModel);
}
