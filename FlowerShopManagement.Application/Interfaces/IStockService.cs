using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface IStockService
{
    #region Voucher
    public Task<bool> CreateVoucher(VoucherDetailModel voucherDetailModel, IVoucherRepository voucherRepository);
    public Task<VoucherDetailModel> GetADetailVoucher(string id);
    public Task<bool>DeleteVoucher(string id);
    public Task<bool>ActivateVoucher(string id);
    public Task<List<VoucherDetailModel>> GetUpdatedVouchers();

    #endregion
    #region Product

    public Task<bool> CreateProduct(ProductDetailModel productModel);
    public List<ProductModel> GetLowOnStockProducts();
    public Task<List<ProductModel>> GetUpdatedProducts();
    public Task<List<ProductDetailModel>> GetUpdatedDetailProducts();
    public Task<List<ProductModel>> GetByIdsAsync(List<string> ids);
    public Task<ProductDetailModel> GetADetailProduct(string id);
    public Task<bool> UpdateProduct(ProductDetailModel productModel);

    #endregion


    #region Category and Material

    public Task<List<string>> GetCategories();
    public Task<bool> CreateCategory(string name);
    public Task<bool> CreateMaterial(string name, string description);
    public Task<List<Material>> GetDetailMaterials();
    public Task<List<string>> GetMaterials();
    #endregion

}
