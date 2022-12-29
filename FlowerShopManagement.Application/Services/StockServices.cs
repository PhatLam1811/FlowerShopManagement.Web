using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Web.Helpers;
using System.IO;

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

    IWebHostEnvironment _webHostEnvironment;
    public StockServices(ICategoryRepository categoryRepository, IMaterialRepository materialRepository,
        IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)

    {
        _categoryRepository = categoryRepository;
        _materialRepository = materialRepository;
        _productRepository = productRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<bool> CreateProduct(ProductDetailModel productModel)
    {
       
        if (productModel != null)
        {
            if (productModel.FormPicture == null)
            {
                // no image case
                //using (var stream = new MemoryStream())
                //{
                //    productModel.FormPicture = new FormFile(stream, 0, 0, "name", "fileName");
                //}
                return false;
            }
            

            // Add image
            if (productModel.FormPicture.Length > 0)
            {
                using (var img = new MemoryStream())
                {

                    await productModel.FormPicture.CopyToAsync(img);
                    WebImage image = new WebImage(img);
                    image.Resize(100, 100);
                    
                    // TODO: ResizeImage(img, 100, 100);

                    byte[] imageBinary = image.GetBytes();
                    string base64String = Convert.ToBase64String(imageBinary);
                    productModel.Picture = base64String;
                }

            }

            var obj = productModel.ToEntity();
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

    public async Task<ProductDetailModel> GetADetailProduct(string id, IProductRepository productRepository)
    {
        Product? product = await productRepository.GetById(id);
        if (product == null) return new ProductDetailModel();
        return new ProductDetailModel(product);
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
    public async Task<List<ProductModel>> GetLowOnStockProducts(IProductRepository productRepository)
    {
        // This is only a temporary value of the minimum amount
        // needed for supply request
        int minimumAmount = 20;
        List<ProductModel> lowOnStockProducts = new List<ProductModel>();

        var result = await productRepository.GetAllLowOnStock(minimumAmount);
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

    public async Task<List<ProductModel>> GetUpdatedProducts(IProductRepository productRepository)
    {
        List<Product>? products = await productRepository.GetAll();
        List<ProductModel> productMs = new List<ProductModel>();

        if (products == null) return productMs;

        foreach (var o in products)
        {
            productMs.Add(new ProductModel(o));
        }
        return productMs;
    }
    public async Task<List<VoucherDetailModel>> GetUpdatedVouchers(IVoucherRepository voucherRepository)
    {
        List<Voucher>? vouchers = await voucherRepository.GetAll();
        List<VoucherDetailModel> voucherMs = new List<VoucherDetailModel>();

        if (vouchers == null) return voucherMs;

        foreach (var o in vouchers)
        {
            voucherMs.Add(new VoucherDetailModel(o));
        }
        return voucherMs;
    }


}
