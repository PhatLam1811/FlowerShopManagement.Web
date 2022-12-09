using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Enums;
using System.Collections.Generic;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Models;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Application.Services;

public class StockServices : IStockServices
{
	// APPLICATION SERVICES (USE CASES)
	public StockServices()
	{

	}

    public async Task<bool> CreateProduct(NewOrEditProductModel productModel, IProductRepository productRepository)
    {
        if(productModel!=null && productRepository != null)
        {
            var obj = productModel.ToEntity();
            return await productRepository.Add(obj);
        }
        return false;
    }

    public async Task<List<ProductModel>> GetUpdatedProducts(IProductRepository productRepository)
    {
        List<Product> products = await productRepository.GetAll();
        List<ProductModel> productMs = new List<ProductModel>();

        foreach (var o in products)
        {
            productMs.Add(new ProductModel(o));
        }
        return productMs;
    }

}
