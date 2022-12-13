using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;

namespace FlowerShopManagement.Application.Interfaces;

public interface IStockServices
{
	public Task<bool> CreateProduct(ProductDetailModel productModel, IProductRepository productRepository);
	public Task<bool> CreateVoucher(VoucherDetailModel voucherDetailModel, IVoucherRepository voucherRepository);
	public Task<List<ProductModel>> GetUpdatedProducts(IProductRepository productRepository);
	public Task<ProductDetailModel> GetADetailProduct(string id, IProductRepository productRepository);
	public Task<List<VoucherDetailModel>> GetUpdatedVouchers(IVoucherRepository voucherRepository);
}
