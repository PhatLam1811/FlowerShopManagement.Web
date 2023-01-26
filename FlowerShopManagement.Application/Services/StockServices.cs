using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using Microsoft.AspNetCore.Hosting;
using FlowerShopManagement.Core.Entities;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Application.Services;

public class StockServices : IStockService
{
    // APPLICATION SERVICES (USE CASES)
    ICategoryRepository _categoryRepository;
    IMaterialRepository _materialRepository;
    IProductRepository _productRepository;
    IWebHostEnvironment _webHostEnvironment;IVoucherRepository _voucherRepository;


	public StockServices(ICategoryRepository categoryRepository, IMaterialRepository materialRepository,
        IProductRepository productRepository, IWebHostEnvironment webHostEnvironment,IVoucherRepository voucherRepository)
    {
        _categoryRepository = categoryRepository;
        _materialRepository = materialRepository;
        _productRepository = productRepository;
        _webHostEnvironment = webHostEnvironment;
        _voucherRepository = voucherRepository;
    }

    public async Task<bool> CreateCategory(string name)
    {
        if (name == null || name == "") return false;

        Category category = new Category() { _id = Guid.NewGuid().ToString(), _name = name };
        var result = await _categoryRepository.Add(category);
        return result;
    }

    public async Task<bool> CreateMaterial(string name, string description)
    {
        if (name == null || name == "" || description == null || description == "") return false;

        Material meterial = new Material() { _id = Guid.NewGuid().ToString(), _name = name , _maintainment = description};
        var result = await _materialRepository.Add(meterial);
        return result;
    }

    public async Task<bool> CreateProduct(ProductDetailModel productModel)
    {

        if (productModel != null)
        {
            if (productModel.FormPicture == null) return false;
            var obj = await productModel.ToEntityContainingImages(
                wwwRootPath: _webHostEnvironment.WebRootPath
                );
            return await _productRepository.Add(obj);
        }
        return false;
    }

    public async Task<bool> CreateVoucher(VoucherDetailModel VoucherDetailModel, IVoucherRepository voucherRepository)
    {
        if (VoucherDetailModel != null && voucherRepository != null)
        {
            var obj = VoucherDetailModel.ToEntity();
            if (obj != null)
                return await voucherRepository.Add(obj);
        }
        return false;
    }

    public async Task<ProductDetailModel> GetADetailProduct(string id)
    {
        Product? product = await _productRepository.GetById(id);
        if (product == null) return new ProductDetailModel();
        return new ProductDetailModel(product);
    }

    public async Task<List<ProductModel>> GetByIdsAsync(List<string> ids)
    {
        var products = new List<ProductModel>();

        try
        {
            var result = await _productRepository.GetByIds(ids);

            foreach (var supplier in result)
            {
                var model = new ProductModel(supplier);
                products.Add(model);
            }

            return products;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<string>> GetCategories()
    {
        var list = await _categoryRepository.GetAll();
        if (list == null) return new List<string>() { "All" };
        return list.Select(x => x._name).ToList();
    }
    public async Task<List<Material>> GetDetailMaterials()
    {
        var list = await _materialRepository.GetAll();
        if (list == null) return new List<Material>();
        return list;
    }
    public async Task<List<ProductModel>> GetLowOnStockProducts()
    {
        // This is only a temporary value of the minimum amount
        // needed for supply request
        int minimumAmount = 20;
        List<ProductModel> lowOnStockProducts = new List<ProductModel>();

        var result = await _productRepository.GetAllLowOnStock(minimumAmount);
        if (result == null) return lowOnStockProducts;
        // Convert Product to SupplyItemModel
        foreach (var item in result)
        {
            // Convert product to model
            var model = new ProductModel(item);

            // Add model to list
            lowOnStockProducts.Add(model);
        }

        return lowOnStockProducts;
    }

    public async Task<List<string>> GetMaterials()
    {
        var list = await _materialRepository.GetAll();
        if (list == null) return new List<string>() { "All" };
        return list.Select(x => x._name).ToList();
    }

    public async Task<List<ProductModel>> GetUpdatedProducts()
    {
        List<Product>? products = await _productRepository.GetAll();
        List<ProductModel> productMs = new List<ProductModel>();

        if (products == null) return productMs;

        foreach (var o in products)
        {
            productMs.Add(new ProductModel(o));
        }
        return productMs;
    }

    public async Task<List<ProductDetailModel>> GetUpdatedDetailProducts()
    {
        List<Product>? products = await _productRepository.GetAll();
        List<ProductDetailModel> productMs = new List<ProductDetailModel>();

        if (products == null) return productMs;

        foreach (var o in products)
        {
            productMs.Add(new ProductDetailModel(o));
        }
        return productMs;
    }
    public async Task<List<VoucherDetailModel>> GetUpdatedVouchers()
    {
        List<Voucher>? vouchers = await _voucherRepository.GetAll();
        List<VoucherDetailModel> voucherMs = new List<VoucherDetailModel>();

        if (vouchers == null) return voucherMs;

        foreach (var o in vouchers)
        {
            voucherMs.Add(new VoucherDetailModel(o));
        }
        return voucherMs;
    }

    public async Task<bool> UpdateProduct(ProductDetailModel productModel)
    {
        if (productModel == null || productModel.Id == null) return false;

        var oldObj = await _productRepository.GetById(productModel.Id);
        var obj = await productModel.ToEntityContainingImages(wwwRootPath: _webHostEnvironment.WebRootPath);
        if(obj == null || obj._id == null || oldObj == null) return false;
        if(oldObj._material != obj._material)
        {
            var list = await _materialRepository.GetAll();
            var material = list.FirstOrDefault(i => i._name == obj._material);
            if (material == null) { return false; }
            obj._maintainment = material._maintainment;
        }
        
        return await _productRepository.UpdateById(obj._id,obj);
    }

    public async Task<VoucherDetailModel> GetADetailVoucher(string id)
    {

        Voucher? voucher = await _voucherRepository.GetById(id);
        if (voucher == null) return new VoucherDetailModel();
        return new VoucherDetailModel(voucher);
    }

    public Task<bool> DeleteVoucher(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ActivateVoucher(string id)
    {
        Voucher? voucher = await _voucherRepository.GetById(id);
        
        if (voucher == null) return false;

        voucher._state = Core.Enums.VoucherStatus.Using;

        return await _voucherRepository.UpdateById(voucher._id, voucher);
    }
}
