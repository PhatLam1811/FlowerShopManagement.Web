using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers;

[Authorize(Policy = "CustomerOnly")]
public class ProductDetailController : Controller
{
    readonly IProductRepository _productRepository;

    public ProductDetailController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ProductDetail(string? id)
    {
        if (id != null)
        {
            Product? product = await _productRepository.GetById(id);
            ProductDetailModel? productM;
            if (product != null)
            {
                productM = new ProductDetailModel(product);
                return View(productM);
            }
        }

        return NotFound();
    }
}
